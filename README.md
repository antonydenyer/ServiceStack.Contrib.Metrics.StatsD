C# ServiceStack StatsD Response Time
================

A really simple bit of middleware that will pump response times into StatsD from ServiceStack


Installation
------------

Build it and add a reference 

To get requsts per second in graphite use scaleToSeconds(derivative(stats.gauges.[prefix].GET.Query.servicestack.handles),1)

Usage
------
```csharp
 Plugins.Add(new ResponseTimesFeature(
                new Statsd(
                    new StatsdUDP(
                        ConfigurationManager.AppSettings["metrics.host"], 8125),
                        ConfigurationManager.AppSettings["metrics.prefix"])));
```
