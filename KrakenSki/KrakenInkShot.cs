using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollisionDetection;

namespace KrakenSki
{
    class KrakenInkShot : TrashBag
    {
        public KrakenInkShot(Game1 game, Level level, Vector2 anchorPos, float shootingAngle) : base(game, level, anchorPos, shootingAngle)
        {
            Text2D = Game.Content.Load<Texture2D>("characters/krakenInkShot");
            timeUntilDelete = 4f;
        }

        public override string Name() => "KrakenInkShot";
        public override void CollisionWith(ICollider other)
        {
            if (other.Name() == "Boat" || other.Name() == "Ski" || 
                other.Name() == "Trash" || other.Name() == "TrashBag")
            {
                isOnWater = false;
            }
        }
    }
}
