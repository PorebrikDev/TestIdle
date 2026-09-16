using System.Collections.Generic;

public interface IAnalyticsProvider
{
    void Track(string eventName, Dictionary<string, object> parameters = null);
}