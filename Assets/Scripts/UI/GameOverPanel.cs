using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace platformer_2d.demo
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private Button restartButton;

        private void Start()
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        public void Show()
        {
            fadeImage.DOFade(1, 0.2f).OnComplete(() =>
            {
                fadeImage.DOFade(0, 0.1f);
                UIManager.Instance.HideMainMenu();
                restartButton.gameObject.SetActive(true);
                
            }).SetEase(Ease.InBounce);
        }

        private void OnRestartButtonClicked()
        {
            restartButton.gameObject.SetActive(false);
            GameManager.Instance.RestartGame();
        }
    }
}