using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CollisionDetection;

namespace KrakenSki
{
    public class FloatingStuffManager
    {
        protected Game1 Game;
        protected Level Level;
        protected List<FloatingStuff> floatingStuffList;
        float delta = 2f;

        public KrakenBoss krakenBoss;
        float timerUntilKraken = GameOptions.TimeUntilKrakenBoss;
        bool krakenBossIsHere = false;

        public FloatingStuffManager(Game1 game, Level level)
        {
            Game = game;
            Level = level;
            floatingStuffList = new List<FloatingStuff>();

            krakenBoss = new KrakenBoss(Game, Level);
        }

        public void AddFloatingStuff(FloatingStuff floatingStuff)
        {
            floatingStuffList.Add(floatingStuff);
            Level.colliders.Add(floatingStuff as ICollider);
        }

        public void RemoveFloatingStuff(FloatingStuff floatingStuff)
        {
            floatingStuffList.Remove(floatingStuff);
            Level.colliders.Remove(floatingStuff as ICollider);
        }

        public void Update(GameTime gameTime)
        {
            timerUntilKraken -= UsefulFuncs.Delta(gameTime);
            /// ADD KRAKEN BOSS ?
            if (GameOptions.FightKrakenBoss && timerUntilKraken <= 0f && !krakenBossIsHere)
            {
                AddFloatingStuff(krakenBoss);
                krakenBossIsHere = true;
            }

            /// ADD
            delta -= UsefulFuncs.Delta(gameTime);
            if (delta < 0)
            {
                FloatingStuff floatingStuff;
                if (UsefulFuncs.RandomFloat(0.0f, 1.0f) < GameOptions.KrakenProbability && !krakenBossIsHere)
                    floatingStuff = new Kraken(Game, Level);
                else
                    floatingStuff = new Trash(Game, Level);

                AddFloatingStuff(floatingStuff);

                delta = new Random().Next(1, 4);
            }

            /// REMOVE
            foreach (FloatingStuff floatingStuff in floatingStuffList.ToArray())
                if (!floatingStuff.Update(gameTime))
                    RemoveFloatingStuff(floatingStuff);
        }

        public void Draw(SpriteBatch sb)
        {
            /// DRAW EVERY FLOATING STUFF EXCEPT THE KRAKEN BOSS
            foreach (FloatingStuff floatingStuff in floatingStuffList)
                if(!(floatingStuff is KrakenBoss))
                    floatingStuff.Draw(sb);

            /// DRAW KRAKEN BOSS AFTER ALL THE "TRASH"
            if(krakenBossIsHere) krakenBoss.Draw(sb);
        }
    }
}
