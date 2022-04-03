using System;
using Microsoft.Xna.Framework.Graphics;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame
{
    public class PlayerShip : Entity
    {
        public static PlayerShip Instance => _instance ??= new PlayerShip();

        private static PlayerShip _instance;
        private static readonly Random Random = new Random();

        private const float Speed = 8;
        private const int AttackCooldownFrames = 6;
        private int _attackCooldown;
        
        private int _framesUntilRespawn;
        public bool IsDead => _framesUntilRespawn > 0;

        private PlayerShip()
        {
            Image = Art.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;
        }

        public override void Update()
        {
            if (IsDead)
            {
                _framesUntilRespawn--;
                return;
            }

            Velocity = Speed * Input.GetMovementDirection();
            Position += Velocity;
            
            // Clamp uniemożliwia wyjście za ekran
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);
            
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            var aim = Input.GetAimDirection();
            if (aim.LengthSquared() > 0 && _attackCooldown <= 0)
                Attack(aim);

            if (_attackCooldown > 0)
                _attackCooldown--;
        }

        private void Attack(Vector2 aim)
        {
            _attackCooldown = AttackCooldownFrames;

            var aimAngle = aim.ToAngle();
            var aimQuaternion = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

            var randomSpread = Random.NextFloat(-0.04f, 0.04f) + Random.NextFloat(-0.04f, 0.04f);
            var velocity = Extensions.FromPolar(aimAngle + randomSpread, 12f);

            var offset = Vector2.Transform(new Vector2(25, -8), aimQuaternion);
            EntityManager.Add(new Bullet(Position + offset, velocity));

            offset = Vector2.Transform(new Vector2(25, 8), aimQuaternion);
            EntityManager.Add(new Bullet(Position + offset, velocity));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead) return;
            
            base.Draw(spriteBatch);
        }
        
        public void Kill()
        {
            _framesUntilRespawn = 60;
            EnemySpawner.Reset();
        }
    }
}