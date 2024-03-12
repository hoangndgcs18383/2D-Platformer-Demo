using platformer_2d.Demo;
using UnityEngine;

namespace platformer_2d.demo
{
    public class PlayerManager
    {
        private PlayerController _playerController;
        private PlayerView _playerView;

        public void Initialize()
        {
            //CreatePlayer();
        }

        public PlayerView CreatePlayer(PlayerConfig playerConfig = null)
        {
            var playerModel = new PlayerModel
            {
                Health = 100,
                MaxHealth = 100,
                Score = 0
            };
            ScoreSystem.Instance.SetData(playerModel);
            _playerView = new GameObject("Player").AddComponent<PlayerView>();
            _playerView.SetConfig(playerConfig);
            _playerView.CreatePlayer();

            _playerController = PlayerController.Build(playerModel, _playerView).WithConfig(playerConfig).EnableInput();
            return _playerView;
        }

        public void Update()
        {
            _playerController?.Update();
        }
        
        public void Dispose()
        {
            _playerController?.DisableInput();
        }

        public void ResetPlayer()
        {
            var playerModel = new PlayerModel
            {
                Health = 100,
                MaxHealth = 100,
                Score = 0
            };

            ScoreSystem.Instance.SetData(playerModel);
            _playerController?.SetDefaultPosition(Vector3.zero);
            _playerController?.EnableInput();
            _playerView.Dispose();
        }
    }
}