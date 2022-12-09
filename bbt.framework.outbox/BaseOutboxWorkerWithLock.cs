using bbt.framework.data;
using bbt.framework.redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbt.framework.outbox
{
    public abstract class BaseOutboxWorkerWithLock<TModel, TOutboxModel>
        where TOutboxModel : BaseOutboxModel<TModel>, new()        
    {
        protected readonly IBaseOutboxProducer<TOutboxModel> baseOutboxProducer;
        protected readonly IBaseRepository<TOutboxModel> baseRepository;
        protected readonly BBTRedislock bBTRedislock;


        public BaseOutboxWorkerWithLock(
            IBaseRepository<TOutboxModel> _baseRepository,
            IBaseOutboxProducer<TOutboxModel> _baseOutboxProducer,
            BBTRedislock _bBTRedislock
            )
        {
            baseRepository = _baseRepository;
            baseOutboxProducer = _baseOutboxProducer;
            bBTRedislock = _bBTRedislock;
        }

        public async Task Process()
        {
            List<TOutboxModel> modelList = await GetData();



            var options = new ParallelOptions { MaxDegreeOfParallelism = 10 };
            await Parallel.ForEachAsync(modelList, options, async (model, token) =>
            {
                //await bBTRedislock.ExecuteMethod(async () =>
                //{
                    TOutboxModel model1 = await baseRepository.FirstOrDefault(x => x.Id == model.Id);
                    if (model1 != null)
                        await Send(model);

                //}, typeof(TModel).Name+"_" + model.Id, TimeSpan.FromSeconds(10));
            });

        }
        public abstract Task<List<TOutboxModel>> GetData();

        public async Task Send(TOutboxModel model)
        {
            await baseOutboxProducer.Send(model);
        }
    }
}
