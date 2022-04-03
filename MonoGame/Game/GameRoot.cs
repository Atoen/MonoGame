using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame
{
    public class GameRoot : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport => Instance._graphics.GraphicsDevice.Viewport;
        public static Vector2 ScreenSize => new Vector2(Viewport.Width, Viewport.Height);
        public static GameTime GameTime { get; private set; }
        
        private bool _paused;
        
        public GameRoot()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            
            Instance = this;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
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
            GameTime = gameTime;
            Input.Update();
            
            if (Input.WasKeyPressed(Keys.Escape))
                Exit();
            
            if (Input.WasKeyPressed(Keys.P))
                _paused = !_paused;

            if (!_paused)
            {
                PlayerStatus.Update();
                EnemySpawner.Update();
                EntityManager.Update();
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(_spriteBatch);
            
            _spriteBatch.DrawString(Art.Font, "Lives: " + PlayerStatus.Lives, new Vector2(5, 5), Color.White);
            DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
            DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 55);
            
            _spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);

            if (PlayerStatus.IsGameOver)
            {
                var text = "Game Over\n" +
                              "Your Score: " + PlayerStatus.Score + "\n" +
                              "High Score: " + PlayerStatus.HighScore;

                var textSize = Art.Font.MeasureString(text);
                _spriteBatch.DrawString(Art.Font, text, ScreenSize / 2 - textSize / 2, Color.White);
            }
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Art.Font.MeasureString(text).X;
            _spriteBatch.DrawString(Art.Font, text, new Vector2(Viewport.Width - textWidth - 5, y), Color.White);
        }
    }
}