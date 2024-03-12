using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer_2d.demo
{
    public class RigidbodyDisplay
    {
        private Rigidbody2D _rigidbody2D;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        public RigidbodyDisplay()
        {
        }

        public void AddRigidbody2D(GameObject gameObject, float gravity)
        {
            _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = gravity;
            _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
        public void AddBoxCollider2D(GameObject gameObject)
        {
            var boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.autoTiling = true;
        }
        
    }
}