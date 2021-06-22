using Microsoft.Xna.Framework;

namespace KrakenSki
{
    public class Scene : DrawableGameComponent
    {
        protected string NextSceneName;
        public string GetNextSceneName() => NextSceneName;

        public Scene(Game game, string nextSceneName) : base(game)
        {
            NextSceneName = nextSceneName;
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            Enabled = Visible = active;
        }
    }
}
