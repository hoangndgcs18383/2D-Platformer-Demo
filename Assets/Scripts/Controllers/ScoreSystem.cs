using System;

namespace platformer_2d.demo
{
    public class ScoreSystem
    {
        private static ScoreSystem _instance;

        public static ScoreSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScoreSystem();
                }

                return _instance;
            }
        }

        public event Action<int> OnScoreChanged;
        public event Action<int> OnHealthChanged;
        public event Action OnPlayerDied;

        private PlayerModel _data;

        public int Score
        {
            get { return _data.Score; }
            private set
            {
                OnScoreChanged?.Invoke(value);
                _data.Score = value;
            }
        }

        public int Health
        {
            get { return _data.Health; }
            private set
            {
                OnHealthChanged?.Invoke(value);
                if (value <= 0)
                {
                    OnPlayerDied?.Invoke();
                }

                _data.Health = value;
            }
        }

        public int MaxHealth
        {
            get { return _data.MaxHealth; }
        }

        public void SetData(PlayerModel data)
        {
            _data = data;
        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public void DecreaseHp(int hp)
        {
            Health -= hp;
        }
    }
}