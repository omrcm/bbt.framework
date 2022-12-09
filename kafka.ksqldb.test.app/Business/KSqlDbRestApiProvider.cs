using ksqlDB.RestApi.Client.KSql.RestApi;
using ksqlDB.RestApi.Client.KSql.RestApi.Http;
using ksqlDB.RestApi.Client.KSql.RestApi.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafka.ksqldb.test.app.Business
{
    //public class KSqlDbRestApiProvider : KSqlDbRestApiClient
    //{
    //    internal static string KsqlDbUrl { get; } = @"http:\\localhost:8088";

    //    public static KSqlDbRestApiProvider Create(string ksqlDbUrl = null)
    //    {
    //        var uri = new Uri(ksqlDbUrl ?? KsqlDbUrl);

    //        return new KSqlDbRestApiProvider(new HttpClientFactory(uri));
    //    }

    //    public KSqlDbRestApiProvider(IHttpClientFactory httpClientFactory)
    //      : base(httpClientFactory)
    //    {
    //    }

    //    public Task<HttpResponseMessage> DropStreamAndTopic(string streamName)
    //    {
    //        var statement = $"DROP STREAM IF EXISTS {streamName} DELETE TOPIC;";

    //        KSqlDbStatement ksqlDbStatement = new(statement);

    //        return ExecuteStatementAsync(ksqlDbStatement);
    //    }

    //    public Task<HttpResponseMessage> DropTableAndTopic(string tableName)
    //    {
    //        var statement = $"DROP TABLE IF EXISTS {tableName} DELETE TOPIC;";

    //        KSqlDbStatement ksqlDbStatement = new(statement);

    //        return ExecuteStatementAsync(ksqlDbStatement);
    //    }
    //}
}