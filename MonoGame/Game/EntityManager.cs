using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame
{
    public static class EntityManager
    {
        private static readonly List<Enemy> Enemies = new List<Enemy>();
        private static readonly List<Bullet> Bullets = new List<Bullet>();

        private static readonly List<Entity> Entities = new List<Entity>();
        private static readonly List<Entity> AddedEntities = new List<Entity>();
        private static bool _isUpdating;

        public static int Count => Entities.Count;

        public static void Add(Entity entity)
        {
            if (!_isUpdating)
                AddEntity(entity);
            else
                AddedEntities.Add(entity);
        }
        
        private static void AddEntity(Entity entity)
        {
            Entities.Add(entity);

            switch (entity)
            {
                case Enemy enemy:
                    Enemies.Add(enemy);
                    break;
                
                case Bullet bullet:
                    Bullets.Add(bullet);
                    break;
            }
        }
        
        public static void Update()
        {
            _isUpdating = true;
            
            HandleCollisions();
            
            foreach (var entity in Entities)
                entity.Update();
            
            _isUpdating = false;

            foreach (var entity in AddedEntities)
                AddEntity(entity);
            
            AddedEntities.Clear();

            Entities.RemoveAll(e => e.IsExpired);
        }
        
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in Entities)
                entity.Draw(spriteBatch);
        }
        
        private static void HandleCollisions()
        {
            // Kolizje pomiędzy przeciwnikami
            for (var i = 0; i < Enemies.Count; i++)
            for (var j = i + 1; j < Enemies.Count; j++)
            {
                if (!IsColliding(Enemies[i], Enemies[j])) continue;
                
                Enemies[i].HandleCollision(Enemies[j]);
                Enemies[j].HandleCollision(Enemies[i]);
            }
            
            // Kolizje pomiędzy przeciwnikami i pociskami
            for (var i = 0; i < Enemies.Count; i++)
            for (var j = 0; j < Bullets.Count; j++)
            {
                if (!IsColliding(Enemies[i], Bullets[j])) continue;
                
                Enemies[i].WasShot();
                Bullets[j].IsExpired = true;
            }
            
            // Kolizje pomiędzy graczem i przeciwnikami
            for (var i = 0; i < Enemies.Count; i++)
            {
                if (!Enemies[i].IsAlive || !IsColliding(PlayerShip.Instance, Enemies[i])) continue;
                
                PlayerShip.Instance.Kill();
                    
                Enemies.ForEach(e => e.WasShot());
            }
        }
        
        private static bool IsColliding(Entity entity1, Entity entity2)
        {
            var radius = entity1.Radius + entity2.Radius;

            return !entity1.IsExpired && !entity2.IsExpired &&
                   Vector2.DistanceSquared(entity1.Position, entity2.Position) < radius * radius;
        }
    }
}