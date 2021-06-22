using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using CollisionDetection;

namespace KrakenSki
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Camera loadingCamera;

        ReloadLevelsScene reloadLevelsScene;
        SplashScreen splashScreen, creditsScreen;
        // Menu menu;
        public Level[] levels;

        public ScenesManager scenesManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }
        
        protected override void Initialize()
        {
            Window.Title = "KrakenSki";
            Window.AllowUserResizing = false;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 500; // 1366
            graphics.PreferredBackBufferHeight = 600; //768
            graphics.ApplyChanges();

            new KeyManager();
            Pixel.Initialize(graphics.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loadingCamera = new Camera(graphics, GameOptions.WorldSize);

            GameSounds.LoadAudio(this);

            reloadLevelsScene = new ReloadLevelsScene(this, "splashScreen");
            splashScreen = new SplashScreen(this, "backgrounds/splashScreen", "creditsScreen");
            creditsScreen = new SplashScreen(this, "backgrounds/creditsScreen", GameOptions.InitialLevel);
            // menu = new Menu(this, "");   // TO IMPLEMENT AFTER
            levels = new Level[4];
            for (int levelNum = 1; levelNum <= levels.Length; levelNum++)
            {
                string nextLevelName;
                if (levelNum == levels.Length)
                    nextLevelName = "reloadLevelsScene";
                else
                    nextLevelName = String.Format("level{0}", levelNum + 1);

                levels[levelNum - 1] = new Level(this, levelNum, nextLevelName);
            }

            /// SCENES
            scenesManager = new ScenesManager(this);
            scenesManager.RegisterScene("reloadLevelsScene", reloadLevelsScene);
            scenesManager.RegisterScene("splashScreen", splashScreen);
            scenesManager.RegisterScene("creditsScreen", creditsScreen);
            // scenesManager.RegisterScene("menu", menu);   // TO IMPLEMENT AFTER
            for (int levelNum = 1; levelNum <= levels.Length; levelNum++)
            {
                string levelName = String.Format("level{0}", levelNum);
                scenesManager.RegisterScene(levelName, levels[levelNum - 1]);
            }

            /// FIRST SCENE
            scenesManager.LoadScene(new Scene(this, ""), "reloadLevelsScene");
            // The first scene above is for some Level's content that can only be loaded after fully concluding its constructor ...
            // ... because, for instance, when Boat is created, Boat trys to add its collider 
            // to Level's colliders list while Level is still being constructed
            // "Why not simply use the foreach below instead?"
            /*foreach (Level level in levels)
                level.LoadContent();*/
            // Because we have it in the reloadLevelsScene and this way we can reload the levels once the game is restarted after the last level
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyManager.Update();

            Scene currentScene = scenesManager.GetCurrentScene().Value;
            string currentSceneName = scenesManager.GetCurrentScene().Key;

            /// IGNORE SPLASH SCREENS
            if (KeyManager.GetKey(Keys.X) && (currentSceneName == "splashScreen" || currentSceneName == "creditsScreen"))
                scenesManager.LoadScene(currentScene, currentScene.GetNextSceneName());

            /// SHOW CREDITS PAUSING THE GAME
            if (KeyManager.GetKey(Keys.C))
                scenesManager.LoadScene(currentScene, "creditsScreen");

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
