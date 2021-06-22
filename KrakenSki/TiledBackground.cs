using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace KrakenSki
{
    class TiledBackground
    {

        Texture2D texture;
        float width, height;
        Camera camera;

        public TiledBackground(ContentManager content, Camera cam, string image, float w)
        {
            texture = content.Load<Texture2D>(image);
            width = w;
            height = texture.Height * width / texture.Width;
            camera = cam;
        }

        private Point PositionToTile(Vector2 pt)
        {
            if (pt.X < 0f) pt.X -= width;
            if (pt.Y < 0f) pt.Y -= height;
            return new Point((int)(pt.X / width), (int)(pt.Y / height));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Point bottom_left = PositionToTile(camera.target - camera.WorldSize / 2f);
            Point top_right = PositionToTile(camera.target + camera.WorldSize / 2f);

            Point size = camera.LengthToPixels(new Vector2(width, height)).ToPoint();
            for (int x = bottom_left.X; x <= top_right.X; x++)
            {
                for (int y = bottom_left.Y; y <= top_right.Y; y++)
                {
                    Vector2 pos = camera.ToPixels(new Vector2(x * width, (y + 1) * height));
                    spriteBatch.Draw(texture,
                                new Rectangle(pos.ToPoint(), size),
                                null,
                                Color.White);
                }
            }
        }

    }
}