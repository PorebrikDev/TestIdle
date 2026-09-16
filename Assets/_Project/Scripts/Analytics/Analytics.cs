using System.Collections.Generic;

public static class Analytics
{
    private static readonly List<IAnalyticsProvider> _providers = new();

    public static void Init()
    {
        _providers.Add(new DebugAnalyticsProvider());
    }

    public static void Track(string eventName, Dictionary<string, object> parameters = null)
    {
        foreach (var p in _providers)
            p.Track(eventName, parameters);
    }
}