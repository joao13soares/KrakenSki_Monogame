using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace KrakenSki
{
    public static class GameSounds
    {
        static float levelSongTime;

        static Song levelSong;
        static SoundEffect healthDamage;
        static SoundEffect collecTrash;
        static SoundEffect hit;
        static SoundEffect throwTrashBag;
        static SoundEffect canThrowTrashBag;

        public static Song  LevelSong { get { return levelSong; } }
        public static void  PlayLevelSong() { MediaPlayer.Play(levelSong); MediaPlayer.IsRepeating = true; }
        public static float LevelSongTime { get { return levelSongTime; } }
        public static void  HealthDamage() { healthDamage.Play(); }
        public static void  CollecTrash() { collecTrash.Play(); }
        public static void  Hit() { hit.Play(); }
        public static void  ThrowTrashBag() { throwTrashBag.Play(); }
        public static void  CanThrowTrashBag() { canThrowTrashBag.Play(); }

        public static void LoadAudio(Game1 game)
        {
            levelSong = game.Content.Load<Song>("songs/Beach_01");
            levelSongTime = (float)levelSong.Duration.TotalSeconds;
            healthDamage = game.Content.Load<SoundEffect>("sound effects/healthDamage");
            collecTrash = game.Content.Load<SoundEffect>("sound effects/collectTrash");
            hit = game.Content.Load<SoundEffect>("sound effects/hit");
            throwTrashBag = game.Content.Load<SoundEffect>("sound effects/throwTrashBag");
            canThrowTrashBag = game.Content.Load<SoundEffect>("sound effects/canThrowTrashBag");
        }
    }
}
