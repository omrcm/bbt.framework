using System.Threading.Tasks;

namespace bbt.framework.outbox
{
    public interface IBaseOutboxProducer<TOutboxModel>
    {
        Task Send(TOutboxModel model);
    }
}
