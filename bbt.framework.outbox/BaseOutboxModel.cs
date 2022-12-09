using bbt.framework.data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bbt.framework.outbox
{
    public record BaseOutboxModel<T> : BaseDBModel
    {

        public bool IsSuccess { get; set; }

        public int TimeBetweenRetrySeconds { get; set; }

        #region retry policy
        public int? Retry { get; set; }
        public int? MaxRetry { get; set; }
        #endregion

        public DateTime? LastExecution { get; set; }
        public DateTime? NextExecution { get; set; }

        #region timeout
        public DateTime? Timeout { get; set; }
        #endregion

        public T? Data { get; set; }
    }
}
