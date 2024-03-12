using System;

namespace platformer_2d.demo
{
    [Serializable]
    public class PlayerModel
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Score { get; set; }
    }
}