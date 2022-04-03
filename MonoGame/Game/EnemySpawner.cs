using System;
using Microsoft.Xna.Framework;

namespace MonoGame
{
    public static class EnemySpawner
    {
        private static readonly Random Random = new Random();
        private static float _inverseSpawnChance = 60f;

        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.Count < 200)
            {
                if (Random.Next((int) _inverseSpawnChance) == 0)
                {
                    EntityManager.Add(Enemy.CreateSeeker(GetSpawnPoint()));
                }
                
                if (Random.Next((int) _inverseSpawnChance) == 0)
                {
                    EntityManager.Add(Enemy.CreateWanderer(GetSpawnPoint()));
                }
            }
            
            // Zwiększanie tempa pojawiania się przeciwników
            if (_inverseSpawnChance > 20)
                _inverseSpawnChance -= 0.005f;
        }

        private static Vector2 GetSpawnPoint()
        {
            Vector2 position;

            do
            {
                position = new Vector2(Random.Next((int) GameRoot.ScreenSize.X), 
                                       Random.Next((int) GameRoot.ScreenSize.Y));
            } while (Vector2.DistanceSquared(position, PlayerShip.Instance.Position) < 250 * 250);

            return position;
        }
        
        public static void Reset()
        {
            _inverseSpawnChance = 60f;
        }
    }
}