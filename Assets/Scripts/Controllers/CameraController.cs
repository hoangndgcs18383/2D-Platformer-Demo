using UnityEngine;

namespace platformer_2d.demo
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float minZoom = 40f;
        [SerializeField] private float maxZoom = 10f;
        [SerializeField] private float minXOffset = 10f;
        [SerializeField] private float maxXOffset = 40f;

        private Transform _target;
        private Transform _transform;
        private Camera _camera;


        private void Start()
        {
            _transform = transform;
            _camera = GetComponent<Camera>();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            var targetPos = _target.position;
            Vector3 desiredPosition = targetPos + offset;
            var position = _transform.position;
            Vector3 smoothedPosition = Vector3.Lerp(position, desiredPosition, smoothSpeed);
            position = smoothedPosition;
            _transform.position = position;

            float zoom = Mathf.Lerp(maxZoom, minZoom, Vector3.Distance(position, targetPos) / 10);
            _camera.orthographicSize = zoom;

            _transform.LookAt(_target);
        }
    }
}