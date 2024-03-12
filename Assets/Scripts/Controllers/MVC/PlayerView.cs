using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace platformer_2d.demo
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerConfig _playerConfig;
        private BoxCollider2D _boxCollider2D;
        private Vector2 _movement;
        private Vector2 _velocity;
        private Transform _characterTransform;
        private bool _isDirectionRight = true;
        private SpriteRenderer _bodySpriteRenderer;
        private Color _defaultColor;
        private RigidbodyDisplay _rigidbodyDisplay;
        private bool _isTriggerDeadZone;

        public void SetConfig(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
        }

        public void CreatePlayer()
        {
            if (_playerConfig == null) return;

            if (_playerConfig.characterPrefab != null)
            {
                var player = Instantiate(_playerConfig.characterPrefab, transform.position, Quaternion.identity);
                player.transform.SetParent(transform);
                _bodySpriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
                _defaultColor = _bodySpriteRenderer.color;
                gameObject.layer = LayerMask.NameToLayer(GameConstant.PLAYER);
                tag = GameConstant.PLAYER;

                _characterTransform = transform;

                _rigidbodyDisplay = new RigidbodyDisplay();
                _rigidbodyDisplay.AddRigidbody2D(gameObject, _playerConfig.gravity);
                _rigidbodyDisplay.AddBoxCollider2D(gameObject);
            }
            else
            {
                Debug.LogError("Character sprite is missing");
            }

        }

        public void Move(Vector2 movement)
        {
            if (_rigidbodyDisplay.Rigidbody2D == null) return;
            _movement.x = movement.x;
            
            if (IsFontOfWall())
            {
                _movement.x = 0;
            }
        }

        public void Jump()
        {
            if (_rigidbodyDisplay.Rigidbody2D == null) return;
            if (IsGrounded())
            {
                _rigidbodyDisplay.Rigidbody2D.velocity = new Vector2(_rigidbodyDisplay.Rigidbody2D.velocity.x, _playerConfig.jumpForce);
            }
        }

        public bool IsGrounded()
        {
            if (_rigidbodyDisplay.Rigidbody2D == null) return false;

            return Physics2D.Raycast(_characterTransform.position, Vector2.down, _playerConfig.groundCheckDistance,
                _playerConfig.groundLayer);
        }

        public bool IsFontOfWall()
        {
            if (_rigidbodyDisplay.Rigidbody2D == null) return false;

            return Physics2D.Raycast(_characterTransform.position, Vector2.right, _playerConfig.wallCheckDistance,
                _playerConfig.groundLayer);
        }

        public void UpdatePlayer()
        {
            if (_rigidbodyDisplay.Rigidbody2D == null) return;
            _velocity.x = _movement.x * _playerConfig.moveSpeed;
            _velocity.y = _rigidbodyDisplay.Rigidbody2D.velocity.y;
            _rigidbodyDisplay.Rigidbody2D.velocity = _velocity;
            _characterTransform.rotation = Quaternion.identity;

            if (!_isDirectionRight && _velocity.x > 0)
            {
                FlipX();
            }
            else if (_isDirectionRight && _velocity.x < 0)
            {
                FlipX();
            }
            
            if(!_isTriggerDeadZone && _characterTransform.position.y < _playerConfig.deadZone)
            {
                _isTriggerDeadZone = true;
                ScoreSystem.Instance.DecreaseHp(100);
            }
        }

        private void FlipX()
        {
            _isDirectionRight = !_isDirectionRight;
            var localScale = _characterTransform.localScale;
            localScale.x *= -1;
            _characterTransform.localScale = localScale;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag(GameConstant.ENEMY))
            {
                FlockPlayer(col);
            }
        }

        private void FlockPlayer(Collision2D collision2D)
        {
            var enemy = collision2D.gameObject.GetComponent<AIBehaviour>();
            if (enemy != null)
            {
                var enemyTransform = enemy.transform;
                var playerTransform = _characterTransform;
                var direction = (playerTransform.position - enemyTransform.position).normalized;
                var force = direction * _playerConfig.knockbackForce;
                _rigidbodyDisplay.Rigidbody2D.DOMove((playerTransform.position + force), 0.5f);
                EffectFlock();
                ScoreSystem.Instance.DecreaseHp(enemy.Damage);
                Debug.Log("Player knocked back");
            }
        }

        private void EffectFlock()
        {
            _bodySpriteRenderer.DOColor(Color.red, 0.05f).SetLoops(Random.Range(2, 4), LoopType.Yoyo)
                .OnComplete(() => _bodySpriteRenderer.DOColor(_defaultColor, 0.1f));
        }
        
        public void Dispose()
        {
            _isTriggerDeadZone = false;
        }
    }
}