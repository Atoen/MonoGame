using System;
using SharpDX;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame
{
    public class PlayerShip : Entity
    {
        public static PlayerShip Instance => _instance ??= new PlayerShip();
        
        private static PlayerShip _instance;
        private static Random _random = new Random();

        private const float Speed = 8;
        private const int AttackCooldownFrames = 6;
        private int _attackCooldown = 0;

        private PlayerShip()
        {
            Image = Art.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;
        }

        public override void Update()
        {
            Velocity = Speed * Input.GetMovementDirection();
            Position += Velocity;
            
            // Clamp uniemożliwia wyjście za ekran
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);
            
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            var aim = Input.GetAimDirection();
            if (aim.LengthSquared() > 0 && _attackCooldown <= 0)
            {
                _attackCooldown = AttackCooldownFrames;
                
                var aimAngle = aim.ToAngle();
                var aimQuaternion = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                
                var randomSpread = _random.NextFloat(-0.04f, 0.04f) + _random.NextFloat(-0.04f, 0.04f);
                var vel = Extensions.FromPolar(aimAngle + randomSpread, 12f);
                
                var offset = Vector2.Transform(new Vector2(25, -8), aimQuaternion);
                EntityManager.Add(new Bullet(Position + offset, vel));
                
                offset = Vector2.Transform(new Vector2(25, 8), aimQuaternion);
                EntityManager.Add(new Bullet(Position + offset, vel));
            }
            
            if (_attackCooldown > 0)
                _attackCooldown--;
        }
    }
}