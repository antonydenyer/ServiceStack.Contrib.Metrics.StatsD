C# ServiceStack StatsD Response Time
================

A really simple bit of middleware that will pump response times into StatsD from ServiceStack


Installation
------------

You can [get the ServiceStack Measurements package on nuget](http://nuget.org/packages/ServiceStack.Contrib.Measurement.StatsD)
Or you can get the source from here on Github and build it.

Usage
------
```csharp
 Plugins.Add(new ResponseTimesFeature(
                new Statsd(
                    new StatsdUDP(
                        ConfigurationManager.AppSettings["metrics.host"], 8125),
                        ConfigurationManager.AppSettings["metrics.prefix"])));
```