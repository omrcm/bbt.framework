using Elastic.Apm.Api;

namespace bbt.worker.template
{
    public class SampleWorker : BackgroundService
    {
        private readonly ILogger<SampleWorker> _logger;
        private readonly ITracer _tracer;

        public SampleWorker(ILogger<SampleWorker> logger, ITracer tracer)
        {
            _logger = logger;
            _tracer = tracer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int waitSeconds = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                await _tracer.CaptureTransaction("CaptureTransaction1", ApiConstants.ActionExec, async () =>
                {
                    await _tracer.CurrentTransaction.CaptureSpan("CaptureSpan1", ApiConstants.ActionExec, async () =>
                    {
                        waitSeconds = Random.Shared.Next(3, 10);
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        await Task.Delay(waitSeconds * 1000, stoppingToken);
                    });
                    await _tracer.CurrentTransaction.CaptureSpan("CaptureSpan2", ApiConstants.ActionExec, async () =>
                    {
                        waitSeconds = Random.Shared.Next(3, 10);
                        await Task.Delay(waitSeconds * 1000, stoppingToken);
                    });
                    await _tracer.CurrentTransaction.CaptureSpan("CaptureSpan3", ApiConstants.ActionExec, async () =>
                    {
                        waitSeconds = Random.Shared.Next(3, 10);
                        _logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
                        await Task.Delay(waitSeconds * 1000, stoppingToken);
                    });
                });
            }
        }
    }
}