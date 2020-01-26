using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Reflection;
using System.Diagnostics;
using AuroraFlare.State;
using AuroraFlare.Utilities;

namespace AuroraFlare
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static KeyboardState oldKeyState;
        public static KeyboardState newKeyState = Keyboard.GetState();
        public static MouseState oldMouseState;
        public static MouseState newMouseState = Mouse.GetState();

        public static Viewport viewport;

        SpriteFont Debug;

        public Main()
        {
            FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            String vers = version.ToString();
            Window.Title = "Aurora Flare - Alpha Build (2/18/2012)";
            this.IsMouseVisible = Settings.ShowMouse;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = Settings.ScreenResolutionWidth;
            this.graphics.PreferredBackBufferHeight = Settings.ScreenResolutionHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            viewport = GraphicsDevice.Viewport;
            new StateManager(Content, 3, (int)StateManager.GameStates.MENU_SCREEN);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Debug = Content.Load<SpriteFont>(@"Fonts/Debug");
            // Loads all of the game state classes.
            StateManager.InitializeStates(Content);

            // Enter the first game state.
            StateManager.EnterState(StateManager.GetCurrentState());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Settings.ShouldExit)
            {
                this.Exit();
            }
            if (!this.IsActive)
            {
                Settings.GameIsActive = this.IsActive;
                return;
            }
            // this will control whether or not the mouse is visible.
            if (this.IsMouseVisible != Settings.ShowMouse)
            {
                this.IsMouseVisible = Settings.ShowMouse;
            }
            // End of mouse visibility control.
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();
            StateManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // Begin Drawing
            this.spriteBatch.Begin();
            StateManager.Draw(gameTime, spriteBatch, GraphicsDevice);
            if (Settings.Debugging)
                spriteBatch.DrawString(Debug, "MouseX: " + newMouseState.X + ", MouseY: " + newMouseState.Y, new Vector2(10, 10), Color.Red);
            // End Drawing
            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

