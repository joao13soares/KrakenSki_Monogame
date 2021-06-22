using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using CollisionDetection;

namespace KrakenSki
{
    public class Boat : ICollider
    {
        public Vector2 Position { get { return position; } }
        public Vector2 Size { get { return size; } }

        Game1 game;
        Level level;
        Camera camera;
        Texture2D boat, rope;
        Vector2 position;
        Vector2 size;
        float ropeRotation = 0f, prevRopeRotation;
        OBB collider;
        Vector2 ropePos, ropePixelPos, ropeSize;
        
        public Ski ski;
        WaterTail waterTail;

        public Boat(Game1 game, Level level)
        {
            this.game = game;
            this.level = level;
            camera = level.camera;
            boat = game.Content.Load<Texture2D>("characters/boat");
            rope = game.Content.Load<Texture2D>("characters/rope");
            size = new Vector2(2.5f, 2.5f * boat.Height / boat.Width);
            position = new Vector2(0, -size.Y / 2f);
            ropeSize = new Vector2(0.2f, 0.2f * rope.Height / rope.Width);
            collider = new OBB(position, size, 0);
            
            ski = new Ski(game, level);
            level.colliders.Add(ski as ICollider);

            waterTail = new WaterTail(game, level);
        }

        public void Update(GameTime gameTime)
        {
            #region MOVE BOAT
            Vector2 velocity = Vector2.One * GameOptions.BoatVelocityFactor * UsefulFuncs.Delta(gameTime);
            /// Vertical movement (unconditional)
            position.Y += velocity.Y;
            camera.LookAt(camera.target + Vector2.UnitY * velocity.Y);

            /// Horizontal movement
            float limit1 = (camera.WorldSize.X - size.X) / 2f;
            float limit2 = (camera.WorldSize.X - ski.Size.X) / 2f;
            float dist = limit1; 
            if ((position.X > camera.target.X && ropeRotation < 0) || (position.X < camera.target.X && ropeRotation > 0))
                dist = limit2 - (float)Math.Sin(Math.Abs(ropeRotation)) * ropeSize.Y;
            
            // Boat to the left
            if (KeyManager.GetKey(Keys.A) && position.X > camera.target.X - dist)
                position.X -= velocity.X;
            // Avoiding glitches on the left limit
            if (position.X < camera.target.X - limit1)
                position.X = camera.target.X - limit1;

            // Boat to the right
            if (KeyManager.GetKey(Keys.D) && position.X < camera.target.X + dist)
                position.X += velocity.X;
            // Avoiding glitches on the right limit
            if (position.X > camera.target.X + limit1)
                position.X = camera.target.X + limit1;

            collider.Center = position;
            #endregion

            #region MOVE & ROTATE ROPE
            /// Rope Position
            ropePos = new Vector2(position.X, position.Y - size.Y / 2f);
            ropePixelPos = camera.ToPixels(ropePos);

            /// Rope Rotation
            prevRopeRotation = ropeRotation;
            Vector2 mousePos = Mouse.GetState().Position.ToVector2();
            ropeRotation = -(float)Math.Atan2(mousePos.X - ropePixelPos.X, Math.Abs(mousePos.Y - ropePixelPos.Y));

            /// Rope Rotation Exceptions
            // Strict to angle limit on both left and right sides
            float angleLimitRadians = UsefulFuncs.DegreesToRadians(GameOptions.AngleLimitDegrees); // conversao graus para radianos
            if (ropeRotation < -angleLimitRadians) ropeRotation = -angleLimitRadians;
            if (ropeRotation > angleLimitRadians) ropeRotation = angleLimitRadians;

            ski.Update(gameTime, ropePos, ropeSize, ropeRotation);
            // If the Ski is beyond the world limits => go back to the previous angle and update the Ski again
            if (ski.Position.X > camera.target.X + limit2 || ski.Position.X < camera.target.X - limit2)
            {
                ropeRotation = prevRopeRotation;
                ski.Update(gameTime, ropePos, ropeSize, ropeRotation);
            }
            #endregion

            waterTail.Update(gameTime, position, size);
        }

        /// Update "NO WORLD LIMITS FOR SKI" VERSION
        /*
        public void Update(GameTime gameTime)
        {
            #region MOVE BOAT
            Vector2 velocity = Vector2.One * GameOptions.BoatVelocityFactor * UsefulFuncs.Delta(gameTime);
            /// Vertical movement (unconditional)
            position.Y += velocity.Y;
            camera.LookAt(camera.target + Vector2.UnitY * velocity.Y);

            /// Horizontal movement
            float dist = (camera.WorldSize.X - size.X) / 2f;
            //float dist = (camera.WorldSize.X - ski.Size.X) / 2f - (float)Math.Sin(Math.Abs(ropeRotation)) * ropeSize.Y;

            // Boat to the left
            if (KeyManager.GetKey(Keys.A) && position.X > camera.target.X - dist)
                position.X -= velocity.X;
            // Boat to the right
            if (KeyManager.GetKey(Keys.D) && position.X < camera.target.X + dist)
                position.X += velocity.X;

            collider.Center = position;
            #endregion

            #region MOVE & ROTATE ROPE
            /// Rope Position
            ropePos = new Vector2(position.X, position.Y - size.Y / 2f);
            ropePixelPos = camera.ToPixels(ropePos);

            /// Rope Rotation
            Vector2 mousePos = Mouse.GetState().Position.ToVector2();
            ropeRotation = -(float)Math.Atan2(mousePos.X - ropePixelPos.X, Math.Abs(mousePos.Y - ropePixelPos.Y));

            /// Rope Rotation Exceptions
            // Strict to angle limit on both left and right sides
            float angleLimitRadians = UsefulFuncs.DegreesToRadians(GameOptions.AngleLimitDegrees); // conversao graus para radianos
            if (ropeRotation < -angleLimitRadians) ropeRotation = -angleLimitRadians;
            if (ropeRotation > angleLimitRadians) ropeRotation = angleLimitRadians;
            #endregion

            ski.Update(gameTime, ropePos, ropeSize, ropeRotation);
        }
        */

        public void Draw(SpriteBatch sb)
        {
            Vector2 pixelPos = camera.ToPixels(position);
            Vector2 pixelSize = camera.LengthToPixels(size);

            waterTail.Draw(sb);

            ski.Draw(sb);

            sb.Draw(boat,
                    new Rectangle(pixelPos.ToPoint(), pixelSize.ToPoint()),
                    null,  // whole texture
                    Color.White,
                    0f,   // rotacao nula
                    new Vector2(boat.Width, boat.Height) / 2f,  // origem no centro da sprite
                    SpriteEffects.None,  // efeitos (nenhum)
                    0);   // layer

            sb.Draw(rope,
                    new Rectangle(ropePixelPos.ToPoint(), camera.LengthToPixels(ropeSize).ToPoint()),
                    null,
                    Color.White,
                    ropeRotation,
                    Vector2.UnitX * rope.Width / 2f,
                    SpriteEffects.None,
                    0);
        }

        /// ICollider
        public string Name() => "Boat";
        public bool CollidesWith(ICollider other) => collider.CollidesWith(other);
        public void CollisionWith(ICollider other)
        {
            if (other.Name() == "Kraken" || other.Name() == "KrakenInkShot")
            {
                ski.healthBar.Damage(GameOptions.DamageGivenByKraken);
                GameSounds.HealthDamage();
            }
            if (other.Name() == "TrashBag")
            {
                ski.healthBar.Damage(0.5f * GameOptions.DamageGivenByKraken);
                GameSounds.HealthDamage();
            }
        }
        public ICollider GetCollider() => collider;

    }
}
