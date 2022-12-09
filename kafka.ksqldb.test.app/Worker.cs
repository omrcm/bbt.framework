using kafka.ksqldb.test.app.Business;
using kafka.ksqldb.test.app.Business.NewFolder;
using ksqlDB.RestApi.Client.KSql.Linq;
using ksqlDB.RestApi.Client.KSql.Linq.PullQueries;
using ksqlDB.RestApi.Client.KSql.Linq.Statements;
using ksqlDB.RestApi.Client.KSql.Query;
using ksqlDB.RestApi.Client.KSql.Query.Context;
using ksqlDB.RestApi.Client.KSql.Query.Context.Options;
using ksqlDB.RestApi.Client.KSql.Query.Options;
using ksqlDB.RestApi.Client.KSql.RestApi;
using ksqlDB.RestApi.Client.KSql.RestApi.Http;
using ksqlDB.RestApi.Client.KSql.RestApi.Parameters;
using ksqlDB.RestApi.Client.KSql.RestApi.Serialization;
using ksqlDB.RestApi.Client.KSql.RestApi.Statements;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace kafka.ksqldb.test.app
{
    public class DataModel
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public Int64 id { get; set; }
        public double distance { get; set; }
        public double altitude { get; set; }
        public double hr { get; set; }
        public double cadence { get; set; }
        public DateTime time { get; set; }
        public double rawTime { get; set; }
    }

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        IKSqlDbRestApiClient restApiClient;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        protected async Task OK()
        {
            string mainStream = "mainStream";
            string streamFromMain = "streamFromMain";

            BKsql bKsql = new BKsql(_logger);
            // var ksqlResponse =  await bKsql.ksql(new KsqlRequest() { ksql = "LIST STREAMS;", streamsProperties = new StreamsProperties() });


            KsqlRequest ksqlRequest = new KsqlRequest();
            ksqlRequest.ksql = $"CREATE OR REPLACE STREAM {mainStream} " +
                $"(lat DOUBLE, lon DOUBLE,id BIGINT,distance DOUBLE,altitude DOUBLE) " +
                $"WITH (kafka_topic='activities-realtime', value_format='json', partitions=1);";

            ksqlRequest.streamsProperties.Add("ksql.streams.auto.offset.reset", "earliest");

            var ksqlResponse = await bKsql.ksql(ksqlRequest);

            ksqlRequest = new KsqlRequest();
            ksqlRequest.ksql = $"CREATE OR REPLACE STREAM {streamFromMain} AS " +
                $"Select * from {mainStream}" +
                $" EMIT CHANGES;";
            ksqlRequest.streamsProperties.Add("ksql.streams.auto.offset.reset", "earliest");


            ksqlResponse = await bKsql.ksql(ksqlRequest);





            ksqlRequest = new KsqlRequest();
            ksqlRequest.ksql = @"CREATE OR REPLACE TABLE table_fromcode16 AS
  SELECT id,
         LATEST_BY_OFFSET(lat) AS lat,
         LATEST_BY_OFFSET(lon) AS lon,
         LATEST_BY_OFFSET(distance) AS distance,
         LATEST_BY_OFFSET(altitude) AS altitude
  FROM fromcode16
  GROUP BY id;";
            ksqlRequest.streamsProperties.Add("ksql.streams.auto.offset.reset", "earliest");

            ksqlResponse = await bKsql.ksql(ksqlRequest);

            Console.WriteLine("");
            // await TAble();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await OK();

            //var httpClientFactory = new HttpClientFactory(new Uri(@"http:\\localhost:8088"));

            //var restApiClient = new KSqlDbRestApiClient(httpClientFactory);

            //EntityCreationMetadata metadata = new()
            //{
            //    EntityName = "fromName",
            //    KafkaTopic = "activities-realtime",
            //    ValueFormat = SerializationFormats.Json,
            //    Partitions = 1,
            //    Replicas = 1
            //};

            //var createTypeResponse = await restApiClient.CreateTypeAsync<DataModel>();


            //var httpResponseMessage = await restApiClient.CreateStreamAsync<DataModel>(metadata)
            //  .ConfigureAwait(false);






            //var creationMetadata = new EntityCreationMetadata
            //{
            //    KafkaTopic = "activities-realtime",
            //    KeyFormat = SerializationFormats.Json,
            //    ValueFormat = SerializationFormats.Json,
            //    EntityName = "fromCodeStream",
            //    ShouldPluralizeEntityName = false
            //};


            //var httpClientFactory = new HttpClientFactory(new Uri(@"http:\\localhost:8088"));
            //var restApiClient = new KSqlDbRestApiClient(httpClientFactory);


            //var httpResponseMessage = await restApiClient.CreateOrReplaceStreamAsync<DataModel>(creationMetadata);








            //KSqlDBContextOptions ContextOptions;
            //KSqlDBContext Context;


            //ContextOptions = new KSqlDBContextOptions(KSqlDbRestApiProvider.KsqlDbUrl);
            //ContextOptions.QueryParameters.AutoOffsetReset = AutoOffsetReset.Earliest;
            //ContextOptions.ShouldPluralizeFromItemName = false;


            //Context = new KSqlDBContext(ContextOptions);



            //var statement = Context.CreateOrReplaceStreamStatement("StreamFromCode1").As<DataModel>("fromCodeStream").Select(c => new { c.id, c.lat, c.lon, c.distance, c.altitude });
            //                                                                                       //          .PartitionBy(c => c.id);



            //var query = statement.ToStatementString();

            //var response = await statement.ExecuteStatementAsync();










        }
        protected async Task SelectStatement()
        {
            //KSqlDBContextOptions ContextOptions;
            //KSqlDBContext Context;


            //ContextOptions = new KSqlDBContextOptions(KSqlDbRestApiProvider.KsqlDbUrl)
            //{
            //    ShouldPluralizeFromItemName = false
            //};

            //Context = new KSqlDBContext(ContextOptions);

            //var pullQuery = Context.CreatePullQuery<DataModel>("table_fromcode16")
            //  .Where(c => c.distance > 10)
            //  //.Where(c => Bounds.WindowStart > windowStart && Bounds.WindowEnd <= windowEnd)
            //  .Take(5);

            //var list = await pullQuery.GetManyAsync().OrderBy(c => c.id).ToListAsync();
        }

        private async Task TAble()
        {
            //string url = @"http:\\localhost:8088";
            //await using var context = new KSqlDBContext(url);

            //var http = new HttpClientFactory(new Uri(url));
            //restApiClient = new KSqlDbRestApiClient(http);
            ////string windowStart = "2019-10-03T21:31:16";
            ////string windowEnd = "2025-10-03T21:31:16";

            //string ksql = "SELECT * FROM table_activities_realtime_latest1;";
            //var result2 = await context.CreateQuery<DataModel>(new ksqlDB.RestApi.Client.KSql.RestApi.Parameters.QueryParameters()
            //{
            //    Sql = ksql
            //}).Where(x => x.distance > 10).ToListAsync();


            //IPullable<DataModel> pullQuery = context.CreatePullQuery<DataModel>("stream_activities_realtime_1");
            //string sql = pullQuery.ToQueryString();




            //var pullQuery = context.CreatePullQuery<DataModel>("table_activities_realtime_latest1");
            ////  .Where(c => c.distance >10)
            ////  //.Where(c => Bounds.WindowStart > windowStart && Bounds.WindowEnd <= windowEnd)
            ////  .Take(5);

            //var sql = pullQuery.ToQueryString();

            //await foreach (var item in pullQuery.GetManyAsync().OrderBy(c => c.id).ConfigureAwait(false))
            //    Console.WriteLine($"Pull query - GetMany result => Id: {item?.id} - Avg Value: {item?.altitude} - Window Start {item?.distance}");

            //var list = await pullQuery.GetManyAsync().OrderBy(c => c.id).ToListAsync();


        }
    }
}