using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace KrakenSki
{
    static class Pixel {
        static Texture2D pixel;

        public static void Initialize(GraphicsDevice gd) {
            pixel = new Texture2D(gd, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
        }
        public static Texture2D GetPixel() {
            if (pixel == null) { throw new Exception("Pixel not initialized!"); }
            return pixel;
        }
    }
}