using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame
{
    public abstract class Entity
    {
        protected Texture2D Image;
        protected Color Color = Color.White;    // Kolor maski
        
        public Vector2 Position;
        public Vector2 Velocity;
        
        public float Orientation;
        public float Radius;
        
        public bool IsExpired;
        
        // Rozmiar grafiki obiektu
        public Vector2 Size => Image == null ? Vector2.Zero : new Vector2(Image.Width, Image.Height);

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color, Orientation, Size / 2, 1, SpriteEffects.None, 0);
        }
    }
}