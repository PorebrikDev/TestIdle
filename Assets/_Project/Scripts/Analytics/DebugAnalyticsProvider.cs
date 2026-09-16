using System.Collections.Generic;
using UnityEngine;

public class DebugAnalyticsProvider : IAnalyticsProvider
{
    public void Track(string eventName, Dictionary<string, object> parameters = null)
    {
        if (parameters == null || parameters.Count == 0)
        {
            Debug.Log($"[Analytics] {eventName}");
            return;
        }

        var sb = new System.Text.StringBuilder($"[Analytics] {eventName} {{ ");
        foreach (var kv in parameters)
            sb.Append($"{kv.Key}={kv.Value} ");
        sb.Append("}");
        Debug.Log(sb.ToString());
    }
}