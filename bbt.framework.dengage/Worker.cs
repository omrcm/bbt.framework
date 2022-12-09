using bbt.framework.dengage.Business;

namespace bbt.framework.dengage
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}


            BDengageLogin bDengageLogin = new BDengageLogin(_logger);
            DengageLoginResponseModel dengageLoginResponseModel = await bDengageLogin.Login(new DengageLoginRequestModel()
            {
                userkey = "",
                password = ""
            });



            BDengageSMS bDengageSMS = new BDengageSMS(_logger, dengageLoginResponseModel.access_token);
            DengageSMSResponseModel dengageSMSResponseModel = await bDengageSMS.TransactionalSMS(
                new DengageSMSRequestModel()
                {
                    content = new DengageSmsMessageModel() { message = "" },
                    send = new DengageSmsSendModel() { to = "" },
                    tags = new List<string>() { "" }
                });




        }
    }
}