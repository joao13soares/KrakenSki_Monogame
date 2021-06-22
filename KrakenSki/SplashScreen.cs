using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KrakenSki
{
    public class SplashScreen : Scene
    {
        SpriteBatch sb;
        Texture2D background;
        float timer = 0f;
        GUI gui;

        public SplashScreen(Game game, string texturePath, string nextSceneName) : base(game, nextSceneName)
        {
            sb = new SpriteBatch(GraphicsDevice);
            background = Game.Content.Load<Texture2D>(texturePath);
            gui = new GUI(Game as Game1, (Game as Game1).loadingCamera);
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= 6f)
            {
                timer = 0f;
                (Game as Game1).scenesManager.LoadScene(this, NextSceneName);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            sb.Begin();

            sb.Draw(background,
                    Vector2.Zero,
                    new Color(
                        MathHelper.Lerp(0, 1, timer / 4f),
                        MathHelper.Lerp(0, 1, timer / 4f),
                        MathHelper.Lerp(0, 1, timer / 4f)
                    ));

            gui.DrawShortCutMsg(sb);

            sb.End();
        }
    }
}
