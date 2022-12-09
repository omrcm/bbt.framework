using bbt.framework.data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace bbt.framework.outbox
{
    public abstract class BaseOutboxProducer<TModel, TOutboxModel> : IBaseOutboxProducer<TOutboxModel>
        where TOutboxModel : BaseOutboxModel<TModel>, new()
    {
        IBaseRepository<TOutboxModel> baseRepository;
        public BaseOutboxProducer(IBaseRepository<TOutboxModel> _baseRepository)
        {
            baseRepository = _baseRepository;
        }
        protected abstract Task<TOutboxModel> Process(TOutboxModel model);

        public async Task Send(TOutboxModel model)
        {
            TOutboxModel result = await Process(model);

            if (!result.IsSuccess)
            {
                model.LastExecution = DateTime.UtcNow;
                model.NextExecution = model.LastExecution.Value.AddSeconds(model.TimeBetweenRetrySeconds);

                if (model.Id < 1)
                {
                    model.Retry = 1;
                    await baseRepository.Insert(result);
                    await baseRepository.Save();
                }
                else
                {
                    model.Retry++;
                    await baseRepository.Update(result);
                    await baseRepository.Save();
                }
            }
            else
            {
                if (model.Id > 1)
                {
                    await baseRepository.Delete(model);
                    await baseRepository.Save();
                }
            }




        }
    }
}
