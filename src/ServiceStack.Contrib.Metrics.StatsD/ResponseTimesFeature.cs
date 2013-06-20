using System;
using ServiceStack.WebHost.Endpoints;
using StatsdClient;

namespace ServiceStack.Contrib.Metrics.StatsD
{
    public class ResponseTimesFeature : IPlugin
    {
        readonly IStatsd _statsD;

        public ResponseTimesFeature(IStatsd statsD)
        {
            _statsD = statsD;
        }

        public void Register(IAppHost appHost)
        {
            appHost.PreRequestFilters.Insert(0, (request, response) => request.Headers.Add("ResponseTime", DateTime.UtcNow.ToString("o")));

            appHost.ResponseFilters.Add((request, response, item) =>
            {
                var started = DateTime.Parse(request.Headers["ResponseTime"]);
                var responseTime = started.Subtract(DateTime.UtcNow);

                _statsD.Send<StatsdClient.Statsd.Timing>(string.Format(".{0}.{1}", request.HttpMethod,request.OperationName), responseTime.Milliseconds);
            });
        }
    }
}
