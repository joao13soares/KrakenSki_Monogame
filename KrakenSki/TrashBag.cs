using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollisionDetection;

namespace KrakenSki
{
    class TrashBag : FloatingStuff
    {
        protected Vector2 dir;

        public TrashBag(Game1 game, Level level, Vector2 anchorPos, float shootingAngle) : base(game, level)
        {
            Text2D = Game.Content.Load<Texture2D>("characters/trashBag");
            Size = new Vector2(1.5f, 1.5f * Text2D.Height / Text2D.Width);
            
            Position = anchorPos;
            
            Collider = new Circle(Position, Size.X / 2f);

            dir = -new Vector2((float)Math.Sin(shootingAngle), (float)Math.Cos(shootingAngle));
            dir.Normalize(); // get unit vector while keeping the original direction
        }

        public override bool Update(GameTime gameTime)
        {
            float velocity = GameOptions.TrashBagsVelocityFactor * UsefulFuncs.Delta(gameTime);
            if(this is KrakenInkShot)
                velocity = GameOptions.KrakenVelocityFactor * UsefulFuncs.Delta(gameTime);

            Position += dir * velocity;

            Circle c = Collider as Circle;
            c.Center = Position;

            TryToClean(gameTime);

            return isOnWater;
        }

        public override string Name() => "TrashBag";
        public override void CollisionWith(ICollider other)
        {
            if (other.Name() == "Boat" || other.Name() == "Trash")
            {
                isOnWater = false;
            }
            if (other.Name() == "Kraken")
            {
                isOnWater = false;
                Level.boat.ski.points += 10;
                GameSounds.Hit();
            }
            if(other.Name() == "KrakenBoss")
            {
                isOnWater = false;
                Level.boat.ski.points += 30;
                GameSounds.Hit();
            }
            if (other.Name() == "KrakenInkShot")
            {
                isOnWater = false;
                GameSounds.Hit();
                Console.WriteLine("Wasted TrashBag");
            }
        }
    }
}
