using System;
using UnityEngine;

namespace platformer_2d.demo
{
    public class AIBehaviour : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float moveDistance;
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] protected int damage = 10;

        protected bool isMovingRight = true;
        private Vector2 _startPosition;
        private Transform _transform;
        private Vector2[] _path;

        public int Damage
        {
            get => damage;
            private set => damage = value;
        }

        private void OnEnable()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            _transform = transform;
            _startPosition = _transform.position;
            _path = new Vector2[2];
            _path[0] = _startPosition;
            _path[1] = new Vector2(_startPosition.x + moveDistance, _startPosition.y);
        }

        private void Update()
        {
            OnUpdate();
        }

        public virtual void OnUpdate()
        {
            MovementFollowPath();
        }

        public virtual void OnDispose()
        {
        }

        protected virtual void MovementFollowPath()
        {
            if (isMovingRight)
            {
                if (_transform.position.x >= _path[1].x)
                {
                    Flip();
                }
            }
            else
            {
                if (_transform.position.x <= _path[0].x)
                {
                    Flip();
                }
            }

            rb.velocity = isMovingRight
                ? new Vector2(moveSpeed, rb.velocity.y)
                : new Vector2(-moveSpeed, rb.velocity.y);
        }

        protected virtual void Flip()
        {
            isMovingRight = !isMovingRight;
            var localScale = _transform.localScale;
            localScale.x *= -1;
            _transform.localScale = localScale;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                rb.velocity = Vector2.zero;
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_path == null) return;
            _path = new Vector2[2];
            _path[0] = transform.position;
            _path[1] = new Vector2(transform.position.x + moveDistance, transform.position.y);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_path[0], _path[1]);
#endif
        }
    }
}