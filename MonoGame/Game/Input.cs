using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MonoGame
{
    public static class Input
    {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;
        
        private static bool _isAimingWithMouse = false;
        
        public static Vector2 MousePosition => new Vector2(_currentMouseState.X, _currentMouseState.Y);
        
        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            
            // Wyłączenie myszy gdy jest wciśnięty klawisz strzałek
            if (new[] {Keys.Left, Keys.Right, Keys.Up, Keys.Down}.Any(key => _currentKeyboardState.IsKeyDown(key)))
                _isAimingWithMouse = false;
            
            // Włączenie myszy gdy zmieniła się jej pozycja
            else if (_currentMouseState.LeftButton == ButtonState.Pressed)
                _isAimingWithMouse = true;
        }
        
        // Sprawdzenie czy klawisz został wciśnięty w ostatniej klatce
        public static bool WasKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }
        
        public static Vector2 GetMovementDirection()
        {
            var direction = Vector2.Zero;
            
            if (_currentKeyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (_currentKeyboardState.IsKeyDown(Keys.D))
                direction.X += 1;
            if (_currentKeyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (_currentKeyboardState.IsKeyDown(Keys.S))
                direction.Y += 1;
            
            // Wektor musi byc =< 1 dla zachowania maksymalnej prędkości
            if (direction.LengthSquared() > 1)
                direction.Normalize();

            return direction;
        }

        public static Vector2 GetAimDirection()
        {
            if (_isAimingWithMouse)
            {
                return GetMouseAimDirection();
            }
            
            var direction = Vector2.Zero;
            
            if (_currentKeyboardState.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (_currentKeyboardState.IsKeyDown(Keys.Right))
                direction.X += 1;
            if (_currentKeyboardState.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (_currentKeyboardState.IsKeyDown(Keys.Down))
                direction.Y += 1;
            
            if (direction == Vector2.Zero)
                return Vector2.Zero;

            return Vector2.Normalize(direction);
        }

        private static Vector2 GetMouseAimDirection()
        {
            var direction = MousePosition - PlayerShip.Instance.Position;
            
            if (direction == Vector2.Zero)
                return Vector2.Zero;
            
            return Vector2.Normalize(direction);
        }
        
        public static bool WasBombButtonPressed()
        {
            return WasKeyPressed(Keys.Space);
        }
    }
}