using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using CollisionDetection;

namespace KrakenSki
{
    public class Level : Scene
    {
        Texture2D levelOver, pauseOverlay;
        Texture2D crosshair;

        int levelDifficulty;
        float delayAfterKrakenBossKilled = 0f;
        bool delayCounting = false;

        Game1 game;
        public Camera camera;
        SpriteBatch sb;
        TiledBackground water;
        public Boat boat;
        public FloatingStuffManager floatingStuffManager;
        public List<ICollider> colliders; // System.Collections.Generic

        public Level(Game1 game, int levelDifficulty, string nextSceneName) : base(game, nextSceneName)
        {
            this.game = game;
            this.levelDifficulty = levelDifficulty;
        }

        public new void LoadContent()
        {
            camera = new Camera(game.graphics, GameOptions.WorldSize);

            levelOver = game.Content.Load<Texture2D>("backgrounds/levelOver");

            pauseOverlay = game.Content.Load<Texture2D>("gui/pauseOverlay");
            crosshair = game.Content.Load<Texture2D>("gui/crosshair");

            sb = new SpriteBatch(GraphicsDevice);
            water = new TiledBackground(game.Content, camera, "backgrounds/waterBackground", 10);

            colliders = new List<ICollider>();

            boat = new Boat(game, this);
            colliders.Add(boat);
            floatingStuffManager = new FloatingStuffManager(game, this);

            GameSounds.PlayLevelSong();

            GameOptions.GamePaused = false;
        }

        public override void Update(GameTime gameTime)
        {
            GameOptions.SetDifficulty(levelDifficulty);

            /// GO TO NEXT LEVEL IF PLAYER DEAD OR IF KRAKEN BOSS KILLED
            if (!delayCounting && 
                (boat.ski.healthBar.Health <= 0 || 
                (GameOptions.FightKrakenBoss && floatingStuffManager.krakenBoss.krakenHealth.Health <= 0) || 
                (!GameOptions.FightKrakenBoss && boat.ski.points >= GameOptions.NumPointsToFinishLevel)) &&
                game.scenesManager.ScenesList.ContainsKey(NextSceneName))
            {
                if(floatingStuffManager.krakenBoss.krakenHealth.Health <= 0)
                    levelOver = game.Content.Load<Texture2D>("backgrounds/krakenBossDefeated");
                else if(boat.ski.healthBar.Health <= 0)
                    levelOver = game.Content.Load<Texture2D>("backgrounds/levelOver");

                // a little delay after the Kraken Boss is killed
                delayCounting = true;
                if (boat.ski.healthBar.Health <= 0 || 
                    (floatingStuffManager.krakenBoss.krakenHealth.Health <= 0 && delayAfterKrakenBossKilled >= 3f) ||
                    (!GameOptions.FightKrakenBoss && boat.ski.points >= GameOptions.NumPointsToFinishLevel))
                {
                    delayCounting = false;

                    // wait Enter to be pressed
                    if (KeyManager.GetKeyDown(Keys.Enter))
                    {
                        delayAfterKrakenBossKilled = 0f;
                        game.scenesManager.LoadScene(this, NextSceneName);
                    }
                }
            }
            else
            {
                if (delayCounting)
                {
                    delayAfterKrakenBossKilled += UsefulFuncs.Delta(gameTime);
                    if (delayAfterKrakenBossKilled >= 3f)
                        delayCounting = false;
                }

                if (!GameOptions.GamePaused)
                {
                    boat.Update(gameTime);
                    floatingStuffManager.Update(gameTime);

                    /// TEST COLISIONS
                    for (int i = 0; i < colliders.Count - 1; i++)
                    {
                        for (int j = i + 1; j < colliders.Count; j++)
                        {
                            if (colliders[i].CollidesWith(colliders[j]))
                            {
                                colliders[i].CollisionWith(colliders[j]);
                                colliders[j].CollisionWith(colliders[i]);
                            }
                        }
                    }
                }

                /// PAUSE LEVEL
                if (KeyManager.GetKeyDown(Keys.Space))
                {
                    GameOptions.GamePaused = !GameOptions.GamePaused;
                    if (MediaPlayer.State == MediaState.Playing) MediaPlayer.Pause();
                    else if (MediaPlayer.State == MediaState.Paused) MediaPlayer.Resume();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            water.Draw(sb);
            floatingStuffManager.Draw(sb);    // desenhar os krakens antes do boat, para que passem por baixo da rope do boat
            boat.Draw(sb);

            if ((boat.ski.healthBar.Health <= 0 ||
                (GameOptions.FightKrakenBoss && floatingStuffManager.krakenBoss.krakenHealth.Health <= 0 && delayAfterKrakenBossKilled >= 3f) ||
                (!GameOptions.FightKrakenBoss && boat.ski.points >= GameOptions.NumPointsToFinishLevel))
                && game.scenesManager.ScenesList.ContainsKey(NextSceneName))
            {
                sb.Draw(levelOver,
                        Vector2.Zero,
                        Color.White * 0.75f);
            }

            if (GameOptions.GamePaused)
            {
                sb.Draw(pauseOverlay,
                        Vector2.Zero,
                        Color.White * 0.75f);
            }
            if (GameOptions.UseCrosshair || GameOptions.GamePaused)
            {
                sb.Draw(crosshair,
                        Mouse.GetState().Position.ToVector2() - new Vector2(crosshair.Width, crosshair.Height) / 2f,
                        Color.White * 0.75f);
            }

            sb.End();
        }
    }
}
