using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace platformer_2d.demo
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Player/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public GameObject characterPrefab;
        public float moveSpeed;
        public float jumpForce;
        public float maxJumpTime;
        public float gravity;
        public float maxFallSpeed;
        public float groundCheckDistance;
        public LayerMask groundLayer;
        public float knockbackForce;
        public float wallCheckDistance;
        public float deadZone;
    }
}