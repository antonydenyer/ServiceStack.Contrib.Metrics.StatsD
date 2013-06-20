using ServiceStack.WebHost.Endpoints;
using StatsdClient;

namespace ServiceStack.Contrib.Metrics.StatsD
{
    public class RequestsPerSecondFeature
    {
        readonly IStatsd _statsD;
        /// <summary>
        /// To get requsts per second in graphite use scaleToSeconds(derivative(stats.[prefix].servicestack.handles),1)
        /// </summary>
        /// <param name="statsD"></param>
        public RequestsPerSecondFeature(IStatsd statsD)
        {
            _statsD = statsD;
        }

        public void Register(IAppHost appHost)
        {
            appHost.PreRequestFilters
                .Insert(0,(request, response) 
                    => _statsD.Send<Statsd.Counting>(".servicestack.handles", 1));
        }
    }
}
