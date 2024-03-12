using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace platformer_2d.demo
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private MapController mapController;

        public static GameManager Instance { get; private set; }

        private PlayerManager _playerManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            ObjectPoolManager.Instance.Initialize();
            ScoreSystem.Instance.OnPlayerDied += OnPlayerDied;

            _playerManager = new PlayerManager();
            _playerManager.Initialize();
            PlayerView player = _playerManager.CreatePlayer(playerConfig);
            cameraController.SetTarget(player.transform);
            mapController.SetPlayerView(player);
            mapController.InitializeMap();
            StartCoroutine(IEDelayShowMainMenu());
        }

        private void OnPlayerDied()
        {
            StartCoroutine(IEEndGame());
        }

        private IEnumerator IEEndGame()
        {
            _playerManager.Dispose();
            Time.timeScale = 0.5f;
            yield return new WaitForSecondsRealtime(0.5f);
            Time.timeScale = 1f;
            UIManager.Instance.ShowGameOver();
        }

        private IEnumerator IEDelayShowMainMenu()
        {
            yield return new WaitForSeconds(1f);
            UIManager.Instance.ShowMainMenu();
        }

        private void Update()
        {
            _playerManager?.Update();
        }

        public void RestartGame()
        {
            _playerManager.ResetPlayer();
            StartCoroutine(IEDelayShowMainMenu());
        }
        
        public void RemoveCoinInMap(Vector2Int coinPosition)
        {
            mapController.RemoveCoin(coinPosition);
        }
    }
}