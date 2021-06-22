using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollisionDetection;

namespace KrakenSki
{
    public class FloatingStuff : ICollider
    {
        protected Game1 Game;
        protected Level Level;
        protected Camera camera;
        protected Texture2D Text2D;
        protected Vector2 Position, Size;
        protected ICollider Collider;
        protected bool isOnWater = true;
        protected float timeUntilDelete = 0;

        public FloatingStuff(Game1 game, Level level)
        {
            Game = game;
            Level = level;
            camera = level.camera;
        }

        public void TryToClean(GameTime gameTime)
        {
            timeUntilDelete += UsefulFuncs.Delta(gameTime);
            if (timeUntilDelete > (camera.WorldSize.Y / GameOptions.BoatVelocityFactor) * (28.0f / 25.0f))   // 28seg para percorrer um mundo de jogo com 25 de altura
            {
                isOnWater = false;
                Console.WriteLine(Name() + " removed");
            }
        }

        public virtual bool Update(GameTime gameTime)
        {
            TryToClean(gameTime);
            Console.WriteLine(Collider);
            return isOnWater;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            Vector2 pixelPos = camera.ToPixels(Position);
            Vector2 pixelSize = camera.LengthToPixels(Size);

            sb.Draw(Text2D,
                    new Rectangle(pixelPos.ToPoint(), pixelSize.ToPoint()),
                    null,
                    Color.White,
                    0f,
                    new Vector2(Text2D.Width, Text2D.Height) / 2f,
                    SpriteEffects.None,
                    0);
        }

        public virtual string Name() => "FloatingStuff";
        public bool CollidesWith(ICollider other) => Collider.CollidesWith(other);
        public virtual void CollisionWith(ICollider other) { }
        public ICollider GetCollider() => Collider;
    }
}