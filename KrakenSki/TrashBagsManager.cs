using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollisionDetection;

namespace KrakenSki
{
    class TrashBagsManager : FloatingStuffManager
    {
        public int TrashBagCounter { get; set; }

        ButtonState prevState, currState;

        public TrashBagsManager(Game1 game, Level level) : base(game, level) { TrashBagCounter = GameOptions.InitialTrashBags; }

        public void Update(GameTime gameTime, Vector2 anchorPos, float angle)
        {
            /// ADD
            // https://stackoverflow.com/questions/9712932/2d-xna-game-mouse-clicking
            prevState = currState;
            currState = Mouse.GetState().LeftButton;
            if (currState == ButtonState.Pressed && prevState == ButtonState.Released && TrashBagCounter >= GameOptions.TrashPerTrashBag)
            {
                TrashBag trashBag = new TrashBag(Game, Level, anchorPos, angle);
                AddFloatingStuff(trashBag);
                TrashBagCounter -= GameOptions.TrashPerTrashBag;
                GameSounds.ThrowTrashBag();
            }

            /// REMOVE
            foreach (TrashBag trashBag in floatingStuffList.ToArray())
            {
                if (!trashBag.Update(gameTime))
                    RemoveFloatingStuff(trashBag);
            }
        }
    }
}
