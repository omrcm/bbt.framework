using bbt.framework.outbox;
using data.test.app.Database;

namespace outbox.test.app.Outbox
{
    public class TestOutboxProducer : BaseOutboxProducer<TestModel, TestOutboxModel>
    {
        // IBaseOutboxProducerStorage<BaseOutboxModel<TestModel>> baseOutboxProducerStorage;
        public TestOutboxProducer(ITestRepository repository) : base(repository)
        {
            // baseOutboxProducerStorage = _baseOutboxProducerStorage;
        }

        protected override async Task<TestOutboxModel> Process(TestOutboxModel model)
        {
           //  model.IsSuccess = false;

            model.IsSuccess = true;
            // model.LastAccessTime = DateTime.UtcNow;

            return model;
        }
    }
}
