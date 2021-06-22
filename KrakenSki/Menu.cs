using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KrakenSki
{
    public class Menu : Scene
    {
        SpriteBatch sb;

        public Menu(Game1 game, string nextSceneName) : base(game, nextSceneName)
        {
            sb = new SpriteBatch(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            System.Console.WriteLine("Update do Menu");
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            System.Console.WriteLine("Draw do Menu");

            sb.End();
        }
    }
}
