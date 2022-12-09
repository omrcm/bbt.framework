using bbt.framework.data;
using bbt.framework.redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbt.framework.outbox
{
    public abstract class BaseOutboxWorker<TModel, TOutboxModel>
        where TOutboxModel : BaseOutboxModel<TModel>, new()        
    {
        protected readonly IBaseOutboxProducer<TOutboxModel> baseOutboxProducer;
        protected readonly IBaseRepository<TOutboxModel> baseRepository;


        public BaseOutboxWorker(
            IBaseRepository<TOutboxModel> _baseRepository,
            IBaseOutboxProducer<TOutboxModel> _baseOutboxProducer
            )
        {
            baseRepository = _baseRepository;
            baseOutboxProducer = _baseOutboxProducer;
        }

        public async Task Process()
        {
            List<TOutboxModel> modelList = await GetData();

            var options = new ParallelOptions { MaxDegreeOfParallelism = 10 };
            await Parallel.ForEachAsync(modelList, options, async (model, token) =>
            {
                    TOutboxModel model1 = await baseRepository.FirstOrDefault(x => x.Id == model.Id);
                    if (model1 != null)
                        await Send(model);
            });

        }
        public abstract Task<List<TOutboxModel>> GetData();

        public async Task Send(TOutboxModel model)
        {
            await baseOutboxProducer.Send(model);
        }
    }
}
