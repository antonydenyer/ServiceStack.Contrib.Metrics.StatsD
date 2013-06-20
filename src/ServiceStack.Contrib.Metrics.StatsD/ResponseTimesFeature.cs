using System;
using ServiceStack.ServiceHost;
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
            appHost.PreRequestFilters.Add((request, response) => PreRequestFilter(request));
            appHost.ResponseFilters.Add((request, response, item) => ResponseFilter(request,response));
        }

        private void ResponseFilter(IHttpRequest request, IHttpResponse response)
        {
            var started = DateTime.Parse(request.Headers["ResponseTime"]);
            var responseTime = started.Subtract(DateTime.UtcNow);

            _statsD.Send<Statsd.Timing>(BuildIdentifier(request), responseTime.Milliseconds);
        }


        private void PreRequestFilter(IHttpRequest request)
        {
            request.Headers.Add("ResponseTime", DateTime.UtcNow.ToString("o"));
            _statsD.Send<Statsd.Counting>(string.Format("{0}.handles", BuildIdentifier(request)), 1);
            _statsD.Send<Statsd.Counting>(string.Format("{0}.requestLengthKb", BuildIdentifier(request)), ContentLengthInKb(request.ContentLength));
        }

        private static int ContentLengthInKb(long contentLength)
        {
            return (int)Math.Round((decimal)contentLength / 1000);
        }

        private static string BuildIdentifier(IHttpRequest request)
        {
            return string.Format(".{0}.{1}", request.HttpMethod, request.OperationName);
        }
    }
}
