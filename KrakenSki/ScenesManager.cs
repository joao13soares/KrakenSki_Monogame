using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace KrakenSki
{
    public class ScenesManager
    {
        Game1 game;
        Dictionary<string, Scene> Scenes;

        public Dictionary<string, Scene> ScenesList { get { return Scenes; } }

        public ScenesManager(Game1 game)
        {
            this.game = game;

            Scenes = new Dictionary<string, Scene>();
        }

        public void RegisterScene(string name, Scene scene)
        {
            Scenes.Add(name, scene);
            game.Components.Add(scene);
        }

        public void LoadScene(Scene caller, string name)
        {
            caller.SetActive(false);
            if (Scenes.ContainsKey(name))
            {
                Scenes[name].SetActive(true);
            }
            else throw new Exception($"Scene {name} not found");
        }

        public KeyValuePair<string, Scene> GetCurrentScene()
        {
            foreach (KeyValuePair<string, Scene> entry in Scenes)
            {
                if (entry.Value.Enabled) return entry;
            }

            throw new Exception($"This will never happen but I have to return all possibilities, so... Hey! There's no Current Scene apparently! Run for your lifes!");
        }
    }
}