using DG.Tweening;
using UnityEngine;

namespace platformer_2d.demo
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int coinValue = 1;
        [SerializeField] private AudioClip coinPickupSound;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private void OnEnable()
        {
            spriteRenderer.DOFade(0.3f, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            spriteRenderer.DOKill();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ScoreSystem.Instance.AddScore(coinValue);
                GameManager.Instance.RemoveCoinInMap(new Vector2Int((int)transform.position.x, (int)transform.position.y));
                ObjectPoolManager.Instance.ReturnObjectToPool(GameConstant.COIN, gameObject);
                Debug.Log("Coin picked up");
            }
        }
    }
}