using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KrakenSki
{
    class GUI
    {
        Game1 game;
        Camera camera;
        SpriteFont arialBlack30, arialBlack20;

        public GUI(Game1 game, Camera camera)
        {
            this.game = game;
            this.camera = camera;
            arialBlack30 = game.Content.Load<SpriteFont>("fonts/ArialBlack30");
            arialBlack20 = game.Content.Load<SpriteFont>("fonts/ArialBlack20");
        }

        public void DrawLevel(SpriteBatch sb, HealthBar health, Texture2D bottomBar, int trashCounter, int points)
        {
            Vector2 bottomBarSize = new Vector2(25f, 25f * bottomBar.Height / bottomBar.Width);
            Vector2 offsetPos = camera.LengthToPixels(camera.WorldSize) - Vector2.UnitY * camera.LengthToPixels(bottomBarSize);

            sb.Draw(bottomBar,
                    Vector2.UnitY * offsetPos,
                    Color.White * 0.75f);

            string text = String.Format("Difficulty: {0}", GameOptions.Difficulty);
            Vector2 dim = arialBlack30.MeasureString(text);
            sb.DrawString(arialBlack30,
                        text,
                        offsetPos * new Vector2(1 / 3f, 1.0f) + dim * new Vector2(-1.3f, 0.35f),
                        GameOptions.DifficultyColors[GameOptions.Difficulty - 1] * 0.75f);

            text = String.Format("Trash: {0}", trashCounter);
            dim = arialBlack30.MeasureString(text);
            sb.DrawString(arialBlack30,
                        text,
                        offsetPos * new Vector2(1 / 2f, 1.0f) + dim * new Vector2(-0.5f, 0.2f),
                        trashCounter >= GameOptions.TrashPerTrashBag ? Color.Red * 0.75f : Color.DarkSlateGray * 0.75f);

            text = String.Format("Points: {0}", points);
            dim = arialBlack30.MeasureString(text);
            sb.DrawString(arialBlack30,
                        text,
                        offsetPos * new Vector2(2 / 3f, 1.0f) + dim * new Vector2(0.5f, 0.35f),
                        Color.Green * 0.75f);

            text = String.Format("--> PAUSE: Space\n--> Credits Screen & Level's Score: C");
            dim = arialBlack30.MeasureString(text);
            sb.DrawString(arialBlack20,
                        text,
                        offsetPos * new Vector2(0, 1.0f) + dim * new Vector2(0.02f, -0.75f),
                        Color.Brown * 0.75f);

            health.Draw(sb);
        }

        public void DrawShortCutMsg(SpriteBatch sb)
        {
            Vector2 anchorPos = camera.LengthToPixels(camera.WorldSize);
            string text = "--> Press X to skip Splash and Credits Screens!";
            Vector2 dim = arialBlack30.MeasureString(text);
            sb.DrawString(arialBlack20,
                        text,
                        anchorPos * new Vector2(0, 1.0f) + dim * new Vector2(0.02f, -0.75f),
                        Color.Brown);
        }
    }
}
