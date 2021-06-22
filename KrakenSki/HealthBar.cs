using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KrakenSki
{
    public class HealthBar
    {
        Level level;

        public float Health { get; protected set; }
        public Vector2 Position { get; set; }
        Vector2 size;

        public HealthBar(Level level, Vector2 pos, Vector2 size)
        {
            this.level = level;

            Health = 1;
            Position = pos;
            this.size = size;
        }

        public float Damage(float damage)
        {
            Health -= damage;
            return Health;
        }

        public float Heal(float heal)
        {
            Health += heal;
            return Health;
        }

        public void Draw(SpriteBatch sb)
        {
            Color color;
            if (Health < .25f) color = new Color(Color.DarkRed, 0.1f);
            else if (Health < .5f) color = new Color(Color.Red, 0.1f);
            else color = new Color(Color.DarkGreen, 0.1f);
            
            Point topLeft = level.camera.ToPixels(Position - Vector2.UnitX * (size / 2f)).ToPoint();
            Point pixSize = new Vector2(level.camera.LengthToPixels(size * Health).X, 5).ToPoint();
            sb.Draw(Pixel.GetPixel(), new Rectangle(topLeft, pixSize), color);
        }
    }
}