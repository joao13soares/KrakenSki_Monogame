using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollisionDetection;

namespace KrakenSki
{
    public class Ski : ICollider
    {
        public Vector2 Position { get { return position; } }
        public Vector2 Size { get { return size; } }

        Game1 game;
        Camera camera;
        Level level;
        Texture2D ski, bottomBar;
        Vector2 position = Vector2.Zero;
        Vector2 size;
        Circle collider;

        TrashBagsManager trashBagManager;

        GUI gui;
        public HealthBar healthBar;
        public int points = 0;

        public Ski(Game1 game, Level level)
        {
            this.game = game;
            this.level = level;
            camera = level.camera;
            this.level = level;
            bottomBar = game.Content.Load<Texture2D>("gui/bottomBar");
            ski = game.Content.Load<Texture2D>("characters/ski");
            size = new Vector2(1.5f, 1.5f * ski.Height / ski.Width);
            collider = new Circle(position, size.X / 2f);

            trashBagManager = new TrashBagsManager(game, level);

            gui = new GUI(game, camera);
            healthBar = new HealthBar(level, position - Vector2.UnitY * size / 2f, 2 * size);
        }

        public void Update(GameTime gameTime, Vector2 ropePos, Vector2 ropeSize, float ropeRotation)
        {
            /// MOVE THE SKI
            Vector2 dir = -new Vector2((float)Math.Sin(ropeRotation), (float)Math.Cos(ropeRotation));
            dir.Normalize(); // get unit vector while keeping the original direction
            position = (ropePos + dir * ropeSize.Y) - Vector2.UnitY * (size.Y / 2f);

            collider.Center = position;
            healthBar.Position = position - Vector2.UnitY * size / 2f;

            trashBagManager.Update(gameTime, position + Vector2.UnitY * (size.Y / 2f), UsefulFuncs.DegreesToRadians(180f));
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 pixelPos = camera.ToPixels(position);
            Vector2 pixelSize = camera.LengthToPixels(size);

            sb.Draw(ski,
                    new Rectangle(pixelPos.ToPoint(), pixelSize.ToPoint()),
                    null,
                    Color.White,
                    0f,
                    new Vector2(ski.Width, ski.Height) / 2f,
                    SpriteEffects.None,
                    0);
            
            trashBagManager.Draw(sb);

            /// --- UI ---
            gui.DrawLevel(sb, healthBar, bottomBar, trashBagManager.TrashBagCounter, points);
        }

        /// ICollider
        public string Name() => "Ski";
        public bool CollidesWith(ICollider other) => collider.CollidesWith(other);
        public void CollisionWith(ICollider other)
        {
            if (other.Name() == "Kraken" || other.Name() == "KrakenInkShot")
            {
                healthBar.Damage(GameOptions.DamageGivenByKraken);
                GameSounds.HealthDamage();
            }
            if (other.Name() == "Trash")
            {
                trashBagManager.TrashBagCounter++;
                points += 5;

                if (trashBagManager.TrashBagCounter == GameOptions.TrashPerTrashBag)
                    GameSounds.CanThrowTrashBag();
                else
                    GameSounds.CollecTrash();
            }
        }
        public ICollider GetCollider() => collider;

    }
}
