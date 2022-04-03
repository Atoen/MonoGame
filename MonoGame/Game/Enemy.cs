using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame
{
    public class Enemy : Entity
    {
        private readonly List<IEnumerator<int>> _behaviours = new List<IEnumerator<int>>();
        private static readonly Random Random = new Random();

        // Płynna animacja pojawiania się przez 60 klatek
        private int _timeUntilStart = 60;
        public bool IsAlive => _timeUntilStart <= 0;
        private int PointValue { get; set; }

        private Enemy(Texture2D image, Vector2 position)
        {
            Image = image;
            Position = position;
            Radius = Image.Width / 2f;
            Color = Color.Transparent;
        }
        
        public static Enemy CreateSeeker(Vector2 position)
        {
            var enemy = new Enemy(Art.Seeker, position);
            enemy.AddBehaviour(enemy.FollowPlayer());
            enemy.PointValue = 100;
            
            return enemy;
        }
        
        public static Enemy CreateWanderer(Vector2 position)
        {
            var enemy = new Enemy(Art.Wanderer, position);
            enemy.AddBehaviour(enemy.MoveRandomly());
            enemy.PointValue = 50;
            
            return enemy;
        }

        public override void Update()
        {
            if (_timeUntilStart <= 0)
            {
                ApplyBehaviours();
            }

            else
            {
                _timeUntilStart--;
                Color = Color.White * (1 - _timeUntilStart / 60f);
            }

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            // Zmniejszenie prędkości "tarcie"
            Velocity *= 0.8f;
        }

        public void WasShot()
        {
            IsExpired = true;
            PlayerStatus.AddPoints(PointValue);
            PlayerStatus.AddMultiplier();
        }
        
        public void HandleCollision(Enemy other)
        {
            var distance = Position - other.Position;
            Velocity += 10 * distance / (distance.LengthSquared() + 1);
        }

        private IEnumerable<int> FollowPlayer(float acceleration = 1f)
        {
            while (true)
            {
                Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }

        private IEnumerable<int> MoveRandomly()
        {
            var direction = Random.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                direction += Random.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);
                
                for (var i = 0; i < 6; i++)
                {
                    Velocity += Extensions.FromPolar(direction, 0.4f);
                    Orientation -= 0.05f;
                    
                    var bounds = GameRoot.Viewport.Bounds;
                    
                    // Jeśli dotknie krawędzi to zmienia kierunek
                    bounds.Inflate(-Image.Width, -Image.Height);
                    
                    if (!bounds.Contains(Position.ToPoint()))
                        direction = (GameRoot.ScreenSize / 2 - Position).ToAngle() + Random.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);

                    yield return 0;
                }
            }
        }

        private void AddBehaviour(IEnumerable<int> behaviour)
        {
            _behaviours.Add(behaviour.GetEnumerator());
        }
        
        private void ApplyBehaviours()
        {
            for (var i = 0; i < _behaviours.Count; i++)
            {
                if (!_behaviours[i].MoveNext())
                    _behaviours.RemoveAt(i--);
            }
        }
    }
}