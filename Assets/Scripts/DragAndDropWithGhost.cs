using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace software.anthill
{
    public class DragAndDropWithGhost : MonoBehaviour
    {
        public PathRenderer pathRenderer;
        [SerializeField] private float maxDistance = 5.0f;
        private bool _isBeingDragged;
        private Plane _dragPlane;
        private GameObject _startGhost;
        private NavMeshAgent _meshAgent;

        private void Start()
        {
            _meshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnMouseDown()
        {
            if (!_isBeingDragged)
            {
                _isBeingDragged = true;
                _startGhost = CreateGhost();
                ChangeTransparency(gameObject, 0.5f);
                _meshAgent.SetDestination(_startGhost.transform.position);
                _dragPlane = new Plane(Vector3.up, transform.position);
            }
        }

        private void OnMouseDrag()
        {
            if (!_isBeingDragged) return;
            if (Camera.main == null) return;
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!_dragPlane.Raycast(camRay, out var planeDist)) return;
            transform.position = camRay.GetPoint(planeDist);
            var calculatedPath = new NavMeshPath();

            if (_meshAgent.CalculatePath(_startGhost.transform.position, calculatedPath))
            {
                _meshAgent.isStopped = true;
                var reversedPath = calculatedPath.corners.Reverse().ToArray();
                if (calculatedPath.Length() > maxDistance)
                {
                    var limitedPath = calculatedPath.LimitFromEnd(maxDistance); 
                    transform.position = new Vector3(
                            limitedPath[0].x,
                            transform.position.y,
                            limitedPath[0].z
                        );
                    pathRenderer.UpdatePath(limitedPath);
                }
                else
                {
                    pathRenderer.UpdatePath(calculatedPath.corners);
                }
            }
            else
            {
                print(calculatedPath.status);
            }
        }

        private void OnMouseUp()
        {
            _isBeingDragged = false;
            pathRenderer.UpdatePath(Array.Empty<Vector3>());
            ChangeTransparency(gameObject, 1.0f);
            Destroy(_startGhost);
        }

        private GameObject CreateGhost()
        {
            var ghost = Instantiate(gameObject, transform.parent, true);
            Destroy(ghost.GetComponent<DragAndDropWithGhost>());
            Destroy(ghost.GetComponent<Collider>());
            Destroy(ghost.GetComponent<NavMeshAgent>());
            return ghost;
        }

        private GameObject ChangeTransparency(GameObject ghost, float alpha)
        {
            var meshRenderer = ghost.GetComponentInChildren<MeshRenderer>();
            var materialColor = meshRenderer.material.color;
            materialColor.a = alpha;
            meshRenderer.material.color = materialColor;
            return ghost;
        }
    }
}