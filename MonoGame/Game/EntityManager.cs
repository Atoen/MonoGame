using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame
{
    public static class EntityManager
    {
        private static List<Entity> _entities = new List<Entity>();

        private static bool _isUpdating;
        private static List<Entity> _addedEntities = new List<Entity>();

        public static int Count => _entities.Count;

        public static void Add(Entity entity)
        {
            if (!_isUpdating)
                _entities.Add(entity);
            else
                _addedEntities.Add(entity);
        }
        
        public static void Update()
        {
            _isUpdating = true;
            
            foreach (var entity in _entities)
                entity.Update();
            
            _isUpdating = false;

            foreach (var entity in _addedEntities)
                _entities.Add(entity);
            
            _addedEntities.Clear();
            
            // _entities.RemoveAll(entity => entity.IsDead);
        }
        
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in _entities)
                entity.Draw(spriteBatch);
        }
    }
}