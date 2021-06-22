using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KrakenSki
{
    class WaterTail
    {
        Game1 game;
        Level level;
        Camera camera;
        Texture2D[] waterTail;
        Texture2D currentTexture;
        Vector2 position, size;
        float animationTimer = 0f;
        int textureIndex = 0;

        public WaterTail(Game1 game, Level level)
        {
            this.game = game;
            this.level = level;
            camera = level.camera;

            waterTail = new Texture2D[3];
            waterTail[0] = game.Content.Load<Texture2D>("characters/waterTail0");
            waterTail[1] = game.Content.Load<Texture2D>("characters/waterTail1");
            waterTail[2] = game.Content.Load<Texture2D>("characters/waterTail2");

            currentTexture = waterTail[textureIndex];

            size = new Vector2(1.5f, 1.5f * currentTexture.Height / currentTexture.Width);
        }
        
        public void Update(GameTime gameTime, Vector2 anchorPos, Vector2 anchorSize)
        {
            animationTimer += UsefulFuncs.Delta(gameTime);
            if(animationTimer >= 0.33f)
            {
                textureIndex++;
                if (textureIndex >= waterTail.Length) textureIndex = 0;
                currentTexture = waterTail[textureIndex];
                animationTimer = 0f;
            }

            position = anchorPos - new Vector2(size.X / 2f, anchorSize.Y / 2f - 0.05f);
        }
        
        public void Draw(SpriteBatch sb)
        {
            Vector2 pixelPos = camera.ToPixels(position);

            sb.Draw(currentTexture,
                    pixelPos,
                    Color.White);
        }
    }
}
