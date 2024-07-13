using System;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace software.anthill
{
    public class DragAndDropWithGhost : MonoBehaviour
    {
        [SerializeField] private PathRenderer pathRenderer;
        [SerializeField] private TMP_Text maxLengthText;
        [SerializeField] private TMP_Text pathLengthText;
        [SerializeField] private TMP_Text dragLengthText;
        [SerializeField] private float maxDistance = 5.0f;
        private bool _isBeingDragged;
        private Plane _dragPlane;
        private GameObject _startGhost;
        private NavMeshAgent _meshAgent;

        private void Start()
        {
            _meshAgent = GetComponent<NavMeshAgent>();
            UpdateTexts(0.0f, 0.0f);
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
                var dragLength = calculatedPath.Length();
                dragLengthText.text = "drag length: " + dragLength;
                if (dragLength > maxDistance)
                {
                    var limitedPath = calculatedPath.LimitFromEnd(maxDistance); 
                    transform.position = new Vector3(
                            limitedPath[0].x,
                            transform.position.y,
                            limitedPath[0].z
                        );
                    UpdateTexts(maxDistance, dragLength);
                    pathRenderer.UpdatePath(limitedPath);
                }
                else
                {
                    UpdateTexts(dragLength, dragLength);
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
            UpdateTexts(0.0f, 0.0f);
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
            var material = new Material(meshRenderer.material);
            var materialColor = material.color;
            materialColor.a = alpha;
            material.color = materialColor;
            meshRenderer.material = material;
            return ghost;
        }

        private void UpdateTexts(float pathLength, float dragLength)
        {
            maxLengthText.text = "max length: " + maxDistance.ToString("F1");
            pathLengthText.text = "path length: " + pathLength.ToString("F1");;
            dragLengthText.text = "drag length: " + dragLength.ToString("F1");;
        }
    }
}