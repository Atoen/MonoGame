using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame
{
    public static class Art
    {
        public static Texture2D Player { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Wanderer { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static SpriteFont Font { get; private set; }
        
        public static void Load(ContentManager content)
        {
            Player = content.Load<Texture2D>("player");
            Seeker = content.Load<Texture2D>("seeker");
            Wanderer = content.Load<Texture2D>("wanderer");
            Bullet = content.Load<Texture2D>("bullet");
            Pointer = content.Load<Texture2D>("pointer");
            Font = content.Load<SpriteFont>("novasquare");
        }
    }
}