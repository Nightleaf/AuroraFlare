using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuroraFlare.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AuroraFlare.Utilities;
using Microsoft.Xna.Framework.Input;

namespace AuroraFlare.Model.State.impl
{
    class MenuState : GameState
    {
        private static KeyboardState oldKeyState;
        private static KeyboardState newKeyState = Keyboard.GetState();
        private static MouseState oldMouseState;
        private static MouseState newMouseState = Mouse.GetState();

        SpriteFont MenuFont;
        SpriteFont Debug;

        Texture2D MenuBG;
        Texture2D[] MenuHovers;

        Color HighlightedOptionColor;

        bool TextEffectOne;
        bool TextEffectTwo;
        bool TextEffectThree;

        public override void Enter()
        {
            Settings.ShowMouse = true;
        }

        public override void Leave()
        {
            Initialized = false;
        }

        public override void Initialize(ContentManager content)
        {
            MenuFont = content.Load<SpriteFont>("Fonts/MenuFont");
            Debug = content.Load<SpriteFont>("Fonts/Debug");
            MenuBG = content.Load<Texture2D>("Menu/MenuBG");
            MenuHovers = new Texture2D[3];
            MenuHovers[0] = content.Load<Texture2D>("Menu/PlayHover");
            MenuHovers[1] = content.Load<Texture2D>("Menu/OptionsHover");
            MenuHovers[2] = content.Load<Texture2D>("Menu/ExitHover");
            HighlightedOptionColor = new Color(255, 255, 0);
            Initialized = true;
        }

        public override void Update(GameTime gameTime)
        {
            // User Input
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();
            HandleMouseInput();
            HandleKeyboardInput();
            // End of user input  
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(MenuBG, new Vector2(0, 0), Color.White);
            if (TextEffectOne)
            {
                spriteBatch.Draw(MenuHovers[0], new Vector2(561, 244), Color.White);
            }
            if (TextEffectTwo)
            {
                spriteBatch.Draw(MenuHovers[1], new Vector2(561, 317), Color.White);
            }
            if (TextEffectThree)
            {
                spriteBatch.Draw(MenuHovers[2], new Vector2(561, 392), Color.White);
            }
        }


        private void HandleMouseInput()
        {
            int x = newMouseState.X;
            int y = newMouseState.Y;
            if (x >= 565 && x <= 716 && y >= 248 && y <= 288)
            {
                TextEffectOne = true;
            }
            else
            {
                TextEffectOne = false;
            }
            if (x >= 565 && x <= 716 && y >= 319 && y <= 360)
            {
                TextEffectTwo = true;
            }
            else
            {
                TextEffectTwo = false;
            }
            if (x >= 565 && x <= 716 && y >= 396 && y <= 436)
            {
                TextEffectThree = true;
            }
            else
            {
                TextEffectThree = false;
            }

            int PressedX = 0;
            int PressedY = 0;
            // Mouse left button has been pressed
            if (newMouseState.LeftButton == ButtonState.Pressed)
            {
                if (oldMouseState.LeftButton != ButtonState.Pressed)
                {
                    PressedX = newMouseState.X;
                    PressedY = newMouseState.Y;
                }
            }
            if (PressedX >= 565 && PressedX <= 716 && PressedY >= 248 && PressedY <= 288)
            {
                StateManager.EnterState((int)StateManager.GameStates.PROFILE_SCREEN);
            }
            if (PressedX >= 565 && PressedX <= 716 && PressedY >= 319 && PressedY <= 360)
            {
            }
            if (PressedX >= 565 && PressedX <= 702 && PressedY >= 396 && PressedY <= 436)
            {
                Settings.ShouldExit = true;
            }
        }

        private void HandleKeyboardInput()
        {
        }
    }
}
