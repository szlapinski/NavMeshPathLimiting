using UnityEngine;
using UnityEngine.AI;

public static class NavMeshPathExtension
{
    public static float Length(this NavMeshPath navMeshPath)
    {
        return Path.Length(navMeshPath.corners);
    }

    public static Vector3[] Limit(this NavMeshPath navMeshPath, float maxLength)
    {
        return Path.Limit(navMeshPath.corners, maxLength);
    }
        
    public static Vector3[] LimitFromEnd(this NavMeshPath navMeshPath, float maxLength)
    {
        return Path.LimitFromEnd(navMeshPath.corners, maxLength);
    }
}