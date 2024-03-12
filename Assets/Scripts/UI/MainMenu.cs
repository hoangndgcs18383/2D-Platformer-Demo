using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace platformer_2d.demo
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainMenuCanvasGroup;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private bool isHideInAwake = true;

        private void Awake()
        {
            if (isHideInAwake)
            {
                AnimShowHide(false, 0);
            }
        }

        private void Start()
        {
            UpdateScore(0);
        }

        public void Show()
        {
            AnimShowHide();
            hpSlider.maxValue = ScoreSystem.Instance.MaxHealth;
            hpSlider.value = ScoreSystem.Instance.Health;
            ScoreSystem.Instance.OnScoreChanged += UpdateScore;
            ScoreSystem.Instance.OnHealthChanged += UpdateHp;
        }

        public void Hide()
        {
            AnimShowHide(false);
            ScoreSystem.Instance.OnScoreChanged -= UpdateScore;
            ScoreSystem.Instance.OnHealthChanged -= UpdateHp;
        }

        public void UpdateScore(int score)
        {
            scoreText.SetText($"Score: {score}");
        }

        public void UpdateHp(int hp)
        {
            hpSlider.value = hp;
        }

        private void AnimShowHide(bool isShow = true, float duration = 1f)
        {
            mainMenuCanvasGroup.DOFade(isShow ? 1 : 0, duration).OnComplete(() =>
            {
                mainMenuCanvasGroup.interactable = isShow;
            });
        }
    }
}