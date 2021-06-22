using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;

namespace KrakenSki
{
    /// <summary>
    /// A keyboard manager.
    /// Allows two different kind of usage: direct querying a state of a key, or programing by events.
    /// It is implemented as a Singleton. Its Update() method must be called in the Game Update method.
    /// </summary>
    class KeyManager
    {

        private static KeyManager instance = null; // Singleton instance

        private enum KeyState { NONE, PRESSED, HELD, RELEASED }

        private Dictionary<Keys, KeyState> keyState;

        private Dictionary<Tuple<Keys, KeyState>, List<Action>> events;

        /// <summary>
        /// Constructor of the singleton class. Needs to be called once (for example in the Initialize method).
        /// </summary>
        public KeyManager()
        {
            if (instance == null)
            {
                keyState = new Dictionary<Keys, KeyState>();
                events = new Dictionary<Tuple<Keys, KeyState>, List<Action>>();
                // save singleton instance
                instance = this;
            }
            else
            {
                throw new Exception("Singleton KeyManager invoked twice!");
            }
        }

        /// <summary>
        /// Must be called during the Update method of the game. It updates the
        /// key states, and calls event actions if required. 
        /// </summary>
        public static void Update()
        {
            KeyboardState state = Keyboard.GetState();

            Keys[] pressedKeys = state.GetPressedKeys();
            foreach (Keys k in pressedKeys)
            {
                // Is this a new key?
                if (!instance.keyState.ContainsKey(k))
                    instance.keyState[k] = KeyState.NONE;
                // Existing key
                switch (instance.keyState[k])
                {
                    case KeyState.NONE:
                    case KeyState.RELEASED:
                        instance.keyState[k] = KeyState.PRESSED;
                        break;

                    case KeyState.HELD:
                    case KeyState.PRESSED:
                        instance.keyState[k] = KeyState.HELD;
                        break;
                }
            }
            // VALIDAR TECLAS QUE DEIXARAM DE ESTAR PREMIDAS
            foreach (Keys k in instance.keyState.Keys.ToArray())
            {
                if (!pressedKeys.Contains(k))
                {
                    switch (instance.keyState[k])
                    {
                        case KeyState.NONE:
                        case KeyState.RELEASED:
                            instance.keyState[k] = KeyState.NONE;
                            break;
                        case KeyState.PRESSED:
                        case KeyState.HELD:
                            instance.keyState[k] = KeyState.RELEASED;
                            break;
                    }
                }
                // Check if there are events for each key state
                Tuple<Keys, KeyState> key = new Tuple<Keys, KeyState>(k, instance.keyState[k]);
                if (instance.events.ContainsKey(key))
                {
                    foreach (Action action in instance.events[key])
                    {
                        action();
                    }
                }
            }
        }

        /// <summary>
        /// Validates if a key was pressed exactly in this frame. 
        /// </summary>
        /// <param name="key">The key to validate</param>
        /// <returns>A true value if the key was pressed exactly in this frame.</returns>
        public static bool GetKeyDown(Keys key)
        {
            return instance.keyState.ContainsKey(key) && instance.keyState[key] == KeyState.PRESSED;
        }

        /// <summary>
        /// Validates if a key was released exactly in this frame.
        /// </summary>
        /// <param name="key">The key to validate</param>
        /// <returns>A true value if the key was released exactly in this frame.</returns>
        public static bool GetKeyUp(Keys key)
        {
            return instance.keyState.ContainsKey(key) && instance.keyState[key] == KeyState.RELEASED;
        }

        /// <summary>
        /// Validates if a key is pressed during this frame.
        /// </summary>
        /// <param name="key">The key to validate</param>
        /// <returns>A true value if the key was pressed during this frame.</returns>
        public static bool GetKey(Keys key)
        {
            return instance.keyState.ContainsKey(key) &&
                (instance.keyState[key] == KeyState.PRESSED || instance.keyState[key] == KeyState.HELD);
        }

        /// <summary>
        /// Registers an action for the event of a specific key being active during a frame.
        /// </summary>
        /// <param name="key">The key to validate</param>
        /// <param name="action">The action to execute</param>
        public static void OnKey(Keys key, Action action)
        {
            _SaveAction(key, KeyState.HELD, action);
        }

        /// <summary>
        /// Registers an action for the event of a specific key being pressed during a frame.
        /// </summary>
        /// <param name="key">The key to validate</param>
        /// <param name="action">The action to execute</param>
        public static void OnKeyDown(Keys key, Action action)
        {
            _SaveAction(key, KeyState.PRESSED, action);
        }

        /// <summary>
        /// Registers an action for the event of a specific key being released during a frame.
        /// </summary>
        /// <param name="key">The key to validate</param>
        /// <param name="action">The action to execute</param>
        public static void OnKeyUp(Keys key, Action action)
        {
            _SaveAction(key, KeyState.RELEASED, action);
        }

        private static void _SaveAction(Keys key, KeyState evnt, Action action)
        {
            Tuple<Keys, KeyState> k = new Tuple<Keys, KeyState>(key, evnt);
            if (!instance.events.ContainsKey(k))
            {
                instance.events[k] = new List<Action>();
            }
            instance.events[k].Add(action);
        }

    }
}