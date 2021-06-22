using Microsoft.Xna.Framework;
using System;

namespace KrakenSki
{
    public class Camera
    {

        float WorldWidth, WorldHeight;

        public Vector2 WorldSize { get { return new Vector2(WorldWidth, WorldHeight); } }
        int vH, vW;
        public Vector2 target { get; private set; } = Vector2.Zero;
        GraphicsDeviceManager graphicsDeviceManager;
        public Camera(GraphicsDeviceManager g, float ww, float wh)
        {
            // Save window/viewport size
            graphicsDeviceManager = g;
            vW = g.PreferredBackBufferWidth;
            vH = g.PreferredBackBufferHeight;
            // Save world size
            WorldWidth = ww;
            WorldHeight = wh;
        }

        public Camera(GraphicsDeviceManager g, float ww)
        {
            // Save window/viewport size
            graphicsDeviceManager = g;
            vW = g.PreferredBackBufferWidth;
            vH = g.PreferredBackBufferHeight;
            // Save world size
            WorldWidth = ww;
            WorldHeight = vH * WorldWidth / vW; // keep aspect ratio 
        }


        /// <summary>
        /// Transform world distances to pixel distances.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public Vector2 LengthToPixels(Vector2 length)
        {
            length.X = length.X * vW / WorldWidth;
            length.Y = length.Y * vH / WorldHeight;

            return length;
        }

        public Vector2 ToPixels(Vector2 pos)
        {
            // Recenter points
            pos.X = (pos.X - target.X) + WorldWidth / 2f;
            pos.Y = (pos.Y - target.Y) + WorldHeight / 2f;

            // Proportion
            pos.X = (vW * pos.X) / WorldWidth;  // world to pixel coordinates
            pos.Y = (vH * pos.Y) / WorldHeight; // world to pixel coordinates

            // Invert Y axis
            pos.Y = vH - pos.Y;

            return pos;
        }

        public void LookAt(Vector2 destination)
        {
            target = destination;
        }

        // se factor = 1 --> tudo na mesma
        // se factor > 1, aumenta area do mundo visualizada (diminui objetos)
        // se factor < 0, diminui area do mundo visualizada (aumenta objetos)
        public void Zoom(float factor)
        {
            WorldWidth = WorldWidth * factor;
            WorldHeight = WorldHeight * factor;
        }
    }
}