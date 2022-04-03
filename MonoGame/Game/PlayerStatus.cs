using System.IO;
using Microsoft.Xna.Framework.Input;

namespace MonoGame
{
    public static class PlayerStatus
    {
        private const float MultiplierExpiryTime = 1f;
        private const int MaxMultiplier = 20;
        private const string HighScoreFilename = "highscore.dat";

        public static bool IsGameOver => Lives == 0;
        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int HighScore { get; private set; }
        public static int Multiplier { get; private set; }
        
        private static float _multiplierTimeLeft;
        private static int _scoreForExtraLife;

        static PlayerStatus()
        {
            HighScore = LoadHighScore();
            Reset();
        }

        public static void Reset()
        {
            if (Score > HighScore)
                SaveHighScore(HighScore = Score);
            
            Lives = 4;
            Score = 0;
            Multiplier = 1;
            _multiplierTimeLeft = 0;
            _scoreForExtraLife = 2000;
        }

        public static void Update()
        {
            if (Multiplier <= 1) return;

            if (!((_multiplierTimeLeft -= (float) GameRoot.GameTime.ElapsedGameTime.TotalSeconds) <= 0)) return;
            
            _multiplierTimeLeft = MultiplierExpiryTime;
            Multiplier = 0;
        }
        
        public static void AddPoints(int basePoints)
        {
            if (PlayerShip.Instance.IsDead) return;
            
            Score += basePoints * Multiplier;
            
            while (Score >= _scoreForExtraLife)
            {
                Lives++;
                _scoreForExtraLife += 4000;
            }
        }
        
        public static void AddMultiplier()
        {
            if (PlayerShip.Instance.IsDead) return;
            
            _multiplierTimeLeft = MultiplierExpiryTime;
            
            if (Multiplier < MaxMultiplier)
                Multiplier++;
        }

        public static void RemoveLife()
        {
            Lives--;
        }
        
        private static int LoadHighScore()
        {
            if (File.Exists(HighScoreFilename) && int.TryParse(File.ReadAllText(HighScoreFilename), out var score))
                return score;

            return 0;
        }
        
        private static void SaveHighScore(int score)
        {
            File.WriteAllText(HighScoreFilename, score.ToString());
        }
    }
}