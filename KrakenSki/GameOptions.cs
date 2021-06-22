using Microsoft.Xna.Framework;

namespace KrakenSki
{
    public static class GameOptions
    {
        public static bool GamePaused = false;
        static string   initialLevel = "level1";
        static int      numPointsToFinishLevel = 100;   // ADICIONADO DEPOIS DA APRESENTAÇÃO POR SUGESTÃO DO PROFESSOR
        static Color[]  difficultyColors = { Color.Green, Color.Yellow, Color.Orange, Color.Red };
        static int      difficulty = 1;
        static float    worldSize = 25.0f;
        static bool     useCrosshair = true;
        static float    timePerLevel = GameSounds.LevelSongTime;
        static float    boatVelocityFactor = 4.0f;
        static float    angleLimitDegrees = 60.0f;
        static int      initialTrashBags = 0;
        static int      trashPerTrashBag = 2;
        static float    trashBagsVelocityFactor = 12.0f;
        static float    krakenProbability = 0.40f;
        static bool     krakenMoves = false;
        static float    krakenVelocityFactor = 2.0f;
        static float    damageGivenByKraken = 0.1f;
        static bool     fightKrakenBoss = false;
        static float    timeUntilKrakenBoss = 15f;
        static float    delayBetweenInkShots = 4f;

        public static string    InitialLevel { get { return initialLevel; } }
        public static int       NumPointsToFinishLevel { get { return numPointsToFinishLevel; } }   // ADICIONADO DEPOIS DA APRESENTAÇÃO POR SUGESTÃO DO PROFESSOR
        public static Color[]   DifficultyColors { get { return difficultyColors; } }
        public static int       Difficulty { get { return difficulty; } }
        public static float     WorldSize { get { return worldSize; } }
        public static bool      UseCrosshair { get { return useCrosshair; } }
        public static float     TimePerLevel { get { return timePerLevel; } }
        public static float     BoatVelocityFactor { get { return boatVelocityFactor; } }
        public static float     AngleLimitDegrees { get { return angleLimitDegrees; } }
        public static int       InitialTrashBags { get { return initialTrashBags; } }
        public static int       TrashPerTrashBag { get { return trashPerTrashBag; } }
        public static float     TrashBagsVelocityFactor { get { return trashBagsVelocityFactor; } }
        public static float     KrakenProbability { get { return krakenProbability; } }
        public static bool      KrakenMoves { get { return krakenMoves; } }
        public static float     KrakenVelocityFactor { get { return krakenVelocityFactor; } }
        public static float     DamageGivenByKraken { get { return damageGivenByKraken; } }
        public static bool      FightKrakenBoss { get { return fightKrakenBoss; } }
        public static float     TimeUntilKrakenBoss { get { return timeUntilKrakenBoss; } }
        public static float     DelayBetweenInkShots { get { return delayBetweenInkShots; } }

        public static void SetDifficulty(int levelDifficulty)
        {
            difficulty = levelDifficulty;

            if (difficulty == 1)
            {
                trashPerTrashBag = 2;
                krakenMoves = false;
                damageGivenByKraken = 0.1f;
                krakenVelocityFactor = 2.0f;
                fightKrakenBoss = false;
            }
            else if (difficulty == 2)           // "<--" means "Changed from the previous level"
            {
                trashPerTrashBag = 2;
                krakenMoves = true;             // <--
                damageGivenByKraken = 0.2f;     // <--
                krakenVelocityFactor = 2.0f;
                fightKrakenBoss = false;
            }
            else if (difficulty == 3)
            {
                trashPerTrashBag = 3;           // <--
                krakenMoves = true;
                damageGivenByKraken = 0.2f;
                krakenVelocityFactor = 4.0f;    // <--
                fightKrakenBoss = false;
            }
            else if (difficulty == 4)
            {
                trashPerTrashBag = 3;
                krakenMoves = true;
                damageGivenByKraken = 0.3f;     // <--
                krakenVelocityFactor = 4.0f;
                fightKrakenBoss = true;         // <--
            }
        }
    }
}
