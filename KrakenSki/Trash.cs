using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollisionDetection;

namespace KrakenSki
{
    class Trash : FloatingStuff, ICollider
    {
        public Trash(Game1 game, Level level) : base(game, level)
        {
            Text2D = Game.Content.Load<Texture2D>("characters/trash");
            Size = new Vector2(1.5f, 1.5f * Text2D.Height / Text2D.Width);

            float limit = (camera.WorldSize.X - Size.X) / 2f;
            Position = new Vector2(UsefulFuncs.RandomFloat(-limit, limit), camera.target.Y + (camera.WorldSize.Y / 2f) + Size.Y);
            
            Collider = new Circle(Position, Size.X / 2f);
        }

        public override string Name() => "Trash";
        public override void CollisionWith(ICollider other)
        {
            if (other.Name() == "Ski")
            {
                isOnWater = false;
                Console.WriteLine("Trash collected");
            }
            if (other.Name() == "TrashBag")
            {
                isOnWater = false;
                GameSounds.Hit();
                Console.WriteLine("TrashBag wasted");
            }
            if (other.Name() == "KrakenInkShot")
            {
                isOnWater = false;
                GameSounds.Hit();
                Console.WriteLine("KrakenBoss shot a Trash");
            }
        }
    }
}