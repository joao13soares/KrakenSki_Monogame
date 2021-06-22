using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollisionDetection;

namespace KrakenSki
{
    public class KrakenBoss : FloatingStuff
    {
        Texture2D[] krakenBoss;
        Texture2D krakenBossDead;
        public HealthBar krakenHealth;
        float randDir;
        Vector2 velocity;

        KrakenInkShot krakenInkShot;
        float delayBetweenInkShots = 0f;

        public KrakenBoss(Game1 game, Level level) : base(game, level)
        {
            krakenBoss = new Texture2D[4];
            krakenBoss[0] = Game.Content.Load<Texture2D>("characters/krakenBoss0");
            krakenBoss[1] = Game.Content.Load<Texture2D>("characters/krakenBoss1");
            krakenBoss[2] = Game.Content.Load<Texture2D>("characters/krakenBoss2");
            krakenBossDead = Game.Content.Load<Texture2D>("characters/krakenBossDead");

            Text2D = krakenBoss[1];
            Size = new Vector2(7f, 7f * Text2D.Height / Text2D.Width);
            
            Position = new Vector2(0, camera.target.Y + (camera.WorldSize.Y / 2f) + Size.Y * 0.75f * GameOptions.TimeUntilKrakenBoss);

            Collider = new Circle(Position, Size.X / 2f);

            randDir = UsefulFuncs.RandomFrom2Numbers(-1.0f, 1.0f);

            krakenHealth = new HealthBar(Level, Position - Vector2.UnitY * Size / 2f, Size);
            
            krakenInkShot = new KrakenInkShot(Game, Level, camera.target + camera.ToPixels(camera.WorldSize), 0f);
        }

        public override bool Update(GameTime gameTime)
        {
            /// HORIZONTAL MOVEMENT
            velocity = new Vector2(randDir * GameOptions.KrakenVelocityFactor * UsefulFuncs.Delta(gameTime), 0f);

            /// VERTICAL MOVEMENT
            if (Position.Y <= camera.target.Y + (camera.WorldSize.Y - Size.Y) / 2f - 1f)
                velocity.Y = GameOptions.BoatVelocityFactor * UsefulFuncs.Delta(gameTime);

            Position += velocity;

            Circle c = Collider as Circle;
            c.Center = Position;
            
            // BOUNCE ON WORLD SIDE LIMITS
            float limit = (camera.WorldSize.X - Size.X) / 2f;
            if (Position.X < camera.target.X - limit || Position.X > camera.target.X + limit)
                randDir = -randDir;

            float shootingAngle = shootingAngle = -(float)Math.Atan2(Level.boat.Position.X - Position.X, Math.Abs(Level.boat.Position.Y - Position.Y));
            delayBetweenInkShots += UsefulFuncs.Delta(gameTime);
            // SHOOT INKSHOTS TO THE PLAYER
            if (!krakenInkShot.Update(gameTime) && delayBetweenInkShots >= GameOptions.DelayBetweenInkShots)
            {
                shootingAngle = -(float)Math.Atan2(Level.boat.Position.X - Position.X, Math.Abs(Level.boat.Position.Y - Position.Y));
                krakenInkShot = new KrakenInkShot(Game, Level, Position - Vector2.UnitY * (Size.Y / 2f), shootingAngle);
                (Level as Level).floatingStuffManager.AddFloatingStuff(krakenInkShot);

                delayBetweenInkShots = 0f;
            }

            // UPDATE CURRENT SPRITE
            if (shootingAngle < -0.5f) Text2D = krakenBoss[0];
            if (shootingAngle >= -0.5f && shootingAngle <= 0.5f) Text2D = krakenBoss[1];
            if (shootingAngle > 0.5f) Text2D = krakenBoss[2];

            krakenHealth.Position = Position - Vector2.UnitY * Size / 2f;

            if (krakenHealth.Health <= 0)
            {
                Text2D = krakenBossDead;
                isOnWater = false;
            }

            return isOnWater;
        }

        public override void Draw(SpriteBatch sb)
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

            krakenHealth.Draw(sb);
        }

        public override string Name() => "KrakenBoss";
        public override void CollisionWith(ICollider other)
        {
            if (other.Name() == "TrashBag")
            {
                krakenHealth.Damage(GameOptions.DamageGivenByKraken);
                Console.WriteLine("KrakenBoss was hit");
            }
        }
    }
}
