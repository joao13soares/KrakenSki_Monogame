using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace KrakenSki
{
    static class UsefulFuncs
    {
        static Random r = new Random();

        public static float RandomFrom2Numbers(float opt1, float opt2)
        {
            return (float)r.Next(2) == 0 ? opt1 : opt2;
        }

        public static float RandomFloat(float minimum, float maximum)
        {
            return (float)r.NextDouble() * (maximum - minimum) + minimum;
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * 180.0f / (float)Math.PI;
        }

        public static float Delta(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
