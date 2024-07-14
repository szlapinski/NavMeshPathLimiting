using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PathRenderer : MonoBehaviour
{
    private SplineContainer _splineContainer;

    private Vector3[] _points;

    private void Start()
    {
        _splineContainer = GetComponent<SplineContainer>();
    }


    public void UpdatePath(Vector3[] points)
    {
        _points = points;
    }

    private void Update()
    {
        _splineContainer.Spline.Clear();
        if (_points != null)
        {
            foreach (var corner in _points)
            {
                _splineContainer.Spline.Add(new float3(corner.x, corner.y, corner.z));
            }
        }
    }
}