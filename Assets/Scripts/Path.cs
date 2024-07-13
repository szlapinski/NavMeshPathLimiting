using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Path
{
    public static float Length(Vector3[] path)
    {
        var length = 0f;
        if (path.Length <= 1) return length;
        var prevPoint = path[0];
        for (var i=1; i<path.Length;i++)
        {
            length += Vector3.Distance(prevPoint, path[i]);
            prevPoint = path[i];
        }
        return length;
    }
        
    public static Vector3[] Limit(Vector3[] path, float maxLength)
    {
        if (path.Length == 0) return path;
        float length = 0;
        var prevPoint = path[0];
        var limitedPath = new List<Vector3> { prevPoint };
        for (var i = 1; i < path.Length; i++)
        {
            var point = path[i];
            var segmentLength = Vector3.Distance(prevPoint, point);
            if (length + segmentLength > maxLength)
            {
                var pathLeft = maxLength - length;
                var newPoint = Vector3.LerpUnclamped(prevPoint, point, pathLeft / segmentLength);
                limitedPath.Add(newPoint);
                return limitedPath.ToArray();
            }
            length += segmentLength;
            limitedPath.Add(point);
            if (Mathf.Approximately(length, maxLength))
            {
                return limitedPath.ToArray();
            }
            prevPoint = point;
        }

        return limitedPath.ToArray();
    }

    public static Vector3[] LimitFromEnd(Vector3[] path, float maxLength)
    {
        return Limit(path.Reverse().ToArray(), maxLength).Reverse().ToArray();
    }
}