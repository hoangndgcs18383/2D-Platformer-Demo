using System;
using Unity.VisualScripting;
using UnityEngine;

namespace platformer_2d.demo
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private GameOverPanel gameOverPanel;

        private static UIManager _instance;

        public static UIManager Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowMainMenu()
        {
            mainMenu.Show();
        }

        public void HideMainMenu()
        {
            mainMenu.Hide();
        }
        
        public void ShowGameOver()
        {
            gameOverPanel.Show();
        }
    }
}