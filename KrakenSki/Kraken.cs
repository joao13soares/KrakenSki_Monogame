using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollisionDetection;

namespace KrakenSki
{
    class Kraken : FloatingStuff
    {
        Texture2D[] kraken;
        float animationTimer = 0f;
        int textureIndex = 0;

        float randFactor;
        float delay = 0;

        public Kraken(Game1 game, Level level) : base(game, level)
        {
            kraken = new Texture2D[4];
            kraken[0] = Game.Content.Load<Texture2D>("characters/kraken0");
            kraken[1] = Game.Content.Load<Texture2D>("characters/kraken1");
            kraken[2] = Game.Content.Load<Texture2D>("characters/kraken2");
            kraken[3] = Game.Content.Load<Texture2D>("characters/kraken1");

            Text2D = kraken[0];
            Size = new Vector2(2f, 2f * Text2D.Height / Text2D.Width);

            float limit = (camera.WorldSize.X - Size.X) / 2f;
            Position = new Vector2(UsefulFuncs.RandomFloat(-limit, limit), camera.target.Y + (camera.WorldSize.Y / 2f) + Size.Y);

            Collider = new Circle(Position, Size.X / 2f);

            // randFactor -> Random float from [-1.0f, -0.5f] U [0.5f, 1.0f]
            // Gives us random velocity:
            // Quantity: from Half to Full
            // Direction: Left or Right
            randFactor = UsefulFuncs.RandomFloat(0.5f, 1.0f);
            randFactor = UsefulFuncs.RandomFrom2Numbers(-randFactor, randFactor);
        }

        public override bool Update(GameTime gameTime)
        {
            animationTimer += UsefulFuncs.Delta(gameTime);
            if (animationTimer >= 0.33f)
            {
                textureIndex++;
                if (textureIndex >= kraken.Length) textureIndex = 0;
                Text2D = kraken[textureIndex];
                animationTimer = 0f;
            }

            if (GameOptions.KrakenMoves)
            {
                float velocity = randFactor * GameOptions.KrakenVelocityFactor * UsefulFuncs.Delta(gameTime);
                Position.X += velocity;

                Circle c = Collider as Circle;
                c.Center = Position;

                // BOUNCE EVERY 3sec OR ON WORLD SIDE LIMITS
                delay += UsefulFuncs.Delta(gameTime);
                float limit = (camera.WorldSize.X - Size.X) / 2f;
                if (delay > 3 || Position.X > camera.target.X + limit || Position.X < camera.target.X - limit)
                {
                    randFactor = -randFactor;
                    delay = 0;
                }
            }

            TryToClean(gameTime);

            return isOnWater;
        }

        public override string Name() => "Kraken";
        public override void CollisionWith(ICollider other)
        {
            if (other.Name() == "Boat" || other.Name() == "Ski" || other.Name() == "TrashBag")
            {
                isOnWater = false;
                Console.WriteLine("Kraken killed");
            }
        }
    }
}