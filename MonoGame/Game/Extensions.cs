using System;
using Microsoft.Xna.Framework;

namespace MonoGame
{
    public static class Extensions
    {
        public static float ToAngle(this Vector2 vector)
        {
            return (float) Math.Atan2(vector.Y, vector.X);
        }
        
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
        }
        
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float) random.NextDouble() * (max - min) + min;
        }
    }
}