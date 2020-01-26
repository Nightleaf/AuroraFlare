using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AuroraFlare.Model.State.impl;

namespace AuroraFlare.State
{
    class StateManager
    {
        private static ContentManager contentManager;

        public static GameState[] StateList;

        private static int CurrentState = 0;
        private static int PreviousGameState = 0;

        public enum GameStates
        {
            MENU_SCREEN = 0,
            GAME_SCREEN = 1,
            PROFILE_SCREEN = 2
        }

        public StateManager(ContentManager content, int amount, int start)
        {
            contentManager = content;
            StateList = new GameState[amount];
            CurrentState = start;
        }

        public static void InitializeStates(ContentManager content)
        {
            StateList[0] = new MenuState();
            StateList[1] = new PlayState();
            StateList[2] = new ProfileState();
        }

        public static void EnterState(int state)
        {
            GameState gamestate = StateList[state];
            if (gamestate == null)
            {
                Console.WriteLine("[Error] Unable to enter state " + state + ".");
                return;
            }
            if (!gamestate.Initialized)
            {
                gamestate.Initialize(contentManager);
            }
            GameState previousGameState = StateList[CurrentState];
            if (previousGameState.Initialized && CurrentState != state)
            {
                previousGameState.Leave();
            }
            gamestate.Enter();
            PreviousGameState = CurrentState;
            CurrentState = state;
        }

        public static void Update(GameTime gameTime)
        {
            StateList[CurrentState].Update(gameTime);
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice device)
        {
            StateList[CurrentState].Draw(gameTime, spriteBatch, device);
        }

        public static int GetCurrentState()
        {
            return CurrentState;
        }

        public static int GetPreviousState()
        {
            return PreviousGameState;
        }
    }
}
