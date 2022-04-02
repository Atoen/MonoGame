using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame
{
    public class GameRoot : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        public static GameRoot instance { get; private set; }
        public static Viewport Viewport => instance._graphics.GraphicsDevice.Viewport;
        public static Vector2 ScreenSize => new Vector2(Viewport.Width, Viewport.Height);
        
        public GameRoot()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            instance = this;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            base.Initialize();
            
            EntityManager.Add(PlayerShip.Instance);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            
            if (keyState.IsKeyDown(Keys.Escape))
                Exit();
            
            EntityManager.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}