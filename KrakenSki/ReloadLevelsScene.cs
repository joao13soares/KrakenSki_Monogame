using Microsoft.Xna.Framework;

namespace KrakenSki
{
    class ReloadLevelsScene : Scene
    {
        public ReloadLevelsScene(Game1 game, string nextSceneName) : base(game, nextSceneName)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Level level in (Game as Game1).levels)
                level.LoadContent();

            (Game as Game1).scenesManager.LoadScene(this, NextSceneName);
        }
    }
}
