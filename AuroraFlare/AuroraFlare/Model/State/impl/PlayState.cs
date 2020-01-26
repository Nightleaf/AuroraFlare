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
using Microsoft.Xna.Framework.Audio;
using AuroraFlare.Model.Entities;
using Microsoft.Xna.Framework.Media;

namespace AuroraFlare.Model.State.impl
{
    class PlayState : GameState
    {
        private static KeyboardState oldKeyState;
        private static KeyboardState newKeyState = Keyboard.GetState();
        private static MouseState oldMouseState;
        private static MouseState newMouseState = Mouse.GetState();
        Random Random = new Random();

        Player MyPlayer;

        SpriteFont Debug;
        SpriteFont MenuFont;
        SpriteFont ScoreFont;

        Texture2D[] StarFields;
        Texture2D[] Cursors;
        Texture2D[] PlayerShips;
        Texture2D[] HoverSprites;

        Texture2D Fade;
        Texture2D PauseMenu;

        AudioEngine MyAudioEngine;
        WaveBank[] WaveBanks;
        SoundBank[] SoundBanks;
        Cue CurrentSong;

        int ScreenWidth;
        int ScreenHeight;
        int randomStarField = 0;

        float ElapsedTime;

        bool[] Boosts;
        bool DrawResumeHover;
        bool DrawOptionsHover;
        bool DrawExitHover;

        public override void Enter()
        {
            Settings.ShowMouse = false;
            Settings.IsGamePaused = false;
        }

        public override void Leave()
        {
            Initialized = false;
        }

        public override void Initialize(ContentManager content)
        {
            // Clear the screen just incase
            ProjectileManager.EntityProjectileList.Clear();
            EntityManager.EntityList.Clear();
            // Initialize the player
            MyPlayer = new Player();
            MyPlayer.Initialize(Settings.PROFILE_TO_USE);
            // Fonts
            MenuFont = content.Load<SpriteFont>("Fonts/MenuFont");
            Debug = content.Load<SpriteFont>("Fonts/Debug");
            ScoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");
            // Sprites
            ProjectileManager.LoadProjectileContent(content);
            NPCManager.LoadNPCContent(content);
            HoverSprites = new Texture2D[3];
            HoverSprites[0] = content.Load<Texture2D>("Menu/ResumeHover");
            HoverSprites[1] = content.Load<Texture2D>("Menu/OptionsHover");
            HoverSprites[2] = content.Load<Texture2D>("Menu/ExitHover");
            Fade = content.Load<Texture2D>("Menu/Fade");
            PauseMenu = content.Load<Texture2D>("Menu/PauseMenu");
            PlayerShips = new Texture2D[1];
            PlayerShips[0] = content.Load<Texture2D>("Game/PlayerShip");
            Cursors = new Texture2D[1];
            Cursors[0] = content.Load<Texture2D>("Game/AttackCursor");
            StarFields = new Texture2D[4];
            StarFields[0] = content.Load<Texture2D>("Game/Starfields/Starfield_blue");
            StarFields[1] = content.Load<Texture2D>("Game/Starfields/Starfield_green");
            StarFields[2] = content.Load<Texture2D>("Game/Starfields/Starfield_purple");
            StarFields[3] = content.Load<Texture2D>("Game/Starfields/Starfield_red");
            // Audio
            MyAudioEngine = new AudioEngine(content.RootDirectory + "/Sounds/AuroraFlare.xgs");
            WaveBanks = new WaveBank[1];
            WaveBanks[0] = new WaveBank(MyAudioEngine, content.RootDirectory + "/Sounds/WB1.xwb");
            SoundBanks = new SoundBank[1];
            SoundBanks[0] = new SoundBank(MyAudioEngine, content.RootDirectory + "/Sounds/SB1.xsb");
            // Boosts/Powerups
            Boosts = new Boolean[4];
            Boosts[Settings.DAMAGE_BOOST] = false;
            Boosts[Settings.HEALTH_BOOST] = false;
            Boosts[Settings.SHIELD_BOOST] = false;
            Boosts[Settings.ACCELERATION_BOOST] = false;
            // Settings
            randomStarField = Random.Next(0, 3);
            MyPlayer.Position = new Vector2((Settings.ScreenResolutionWidth / 2 - PlayerShips[0].Width / 2), (Settings.ScreenResolutionHeight / 2 - PlayerShips[0].Height / 2));
            ElapsedTime = 0f;
            Initialized = true;
            StartMusic();
        }

        public override void Update(GameTime gameTime)
        {
            if (Settings.IsGamePaused)
            {
                Settings.ShowMouse = true;
                // User Input
                oldMouseState = newMouseState;
                newMouseState = Mouse.GetState();
                oldKeyState = newKeyState;
                newKeyState = Keyboard.GetState();
                HandleMouseInput(gameTime);
                HandleKeyboardInput(gameTime);
                return;
            }
            // User Input
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();
            HandleMouseInput(gameTime);
            HandleKeyboardInput(gameTime);
            // End of user input
            ElapsedTime++;

            ProjectileManager.Update(gameTime);
            NPCManager.Update(gameTime);
            MyPlayer.Update(gameTime);
            UpdateCollision(gameTime);
        }


        /// <summary>
        /// This will update the collision, and check if anything is intersecting.
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateCollision(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            ScreenWidth = graphicsDevice.Viewport.Width;
            ScreenHeight = graphicsDevice.Viewport.Height;
            spriteBatch.Draw(StarFields[randomStarField], new Vector2(0, 0), Color.White);
            ProjectileManager.DrawProjectiles(spriteBatch);
            NPCManager.Draw(spriteBatch);
            MyPlayer.Render(PlayerShips[0], spriteBatch);
            if (!Settings.IsGamePaused) 
                spriteBatch.Draw(Cursors[0], new Vector2((newMouseState.X - Cursors[0].Width / 2), (newMouseState.Y - Cursors[0].Height / 2)), Color.White);
            if (Settings.Debugging)
            {
                float distance = Vector2.Distance(MyPlayer.Position, new Vector2(newMouseState.X, newMouseState.Y));
                spriteBatch.DrawString(Debug, "Active Projectiles: " + ProjectileManager.EntityProjectileList.Count, new Vector2(30, 30), Color.White);
                spriteBatch.DrawString(Debug, "Active Entities: " + EntityManager.EntityList.Count, new Vector2(30, 45), Color.White);
                spriteBatch.DrawString(Debug, "Mouse Distance: " + distance, new Vector2(30, 75), Color.White);
                foreach (Entity e in EntityManager.EntityList)
                {
                    if (e != null)
                    {
                        spriteBatch.DrawString(Debug, "HP: " + e.CurrentHealth + " / " + e.MaxHealth, new Vector2(e.Position.X, e.Position.Y), Color.White);
                        spriteBatch.DrawString(Debug, "SD: " + e.CurrentShields + " / " + e.MaxShields, new Vector2(e.Position.X, e.Position.Y+15), Color.White);
                    }
                }
            } 
            if (Settings.IsGamePaused)
            {
                DrawPauseMenu(spriteBatch);
            }
        }

        private void DrawPauseMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Fade, new Vector2(0,0), Color.White);
            spriteBatch.Draw(PauseMenu, new Vector2((Settings.ScreenResolutionWidth / 2 - PauseMenu.Width / 2), (Settings.ScreenResolutionHeight / 2 - PauseMenu.Height / 2)), Color.White); 
            if (DrawResumeHover)
                spriteBatch.Draw(HoverSprites[0], new Vector2(562, 254), Color.White);
            if (DrawOptionsHover)
                spriteBatch.Draw(HoverSprites[1], new Vector2(561, 326), Color.White);
            if (DrawExitHover)
                spriteBatch.Draw(HoverSprites[2], new Vector2(561, 401), Color.White);
        }

        private void HandleMouseInput(GameTime gameTime)
        {
            int x = newMouseState.X;
            int y = newMouseState.Y;
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
            if (Settings.IsGamePaused)
            {
                // Resume Hover
                if (x >= 564 && x <= 715 && y >= 225 && y <= 296)
                {
                    DrawResumeHover = true;
                }
                else
                {
                    DrawResumeHover = false;
                }

                // Options Hover
                if (x >= 564 && x <= 715 && y >= 329 && y <= 370)
                {
                    DrawOptionsHover = true;
                }
                else
                {
                    DrawOptionsHover = false;
                }

                // Exit Hover
                if (x >= 564 && x <= 715 && y >= 404 && y <= 497)
                {
                    DrawExitHover = true;
                }
                else
                {
                    DrawExitHover = false;
                }

                // Resume the game
                if (PressedX >= 564 && PressedX <= 715 && PressedY >= 225 && PressedY <= 296)
                {
                    Settings.IsGamePaused = false;
                }

                // Options
                if (PressedX >= 564 && PressedX <= 715 && PressedY >= 329 && PressedY <= 370)
                {

                }

                // Exit to main menu
                if (PressedX >= 564 && PressedX <= 715 && PressedY >= 404 && PressedY <= 497)
                {
                    MyPlayer.SaveProfile();
                    StateManager.EnterState((int)StateManager.GameStates.MENU_SCREEN);
                }
            }
            // Mouse left button has been pressed
            if (newMouseState.LeftButton == ButtonState.Pressed)
            {
                if (ElapsedTime >= Settings.ShotThrottleLimit)
                {
                    if (!Boosts[Settings.DAMAGE_BOOST])
                    {
                        ProjectileManager.AddProjectile(MyPlayer, ProjectileManager.projectileSprites[0], new Vector2(newMouseState.X, newMouseState.Y)); 
                    }
                    else if (Boosts[Settings.DAMAGE_BOOST])
                    {
                        ProjectileManager.AddProjectile(MyPlayer, ProjectileManager.projectileSprites[1], new Vector2(newMouseState.X, newMouseState.Y));
                    }
                    ElapsedTime = 0f;
                }

            }
        }

        private void HandleKeyboardInput(GameTime gameTime)
        {
            if (newKeyState.IsKeyDown(Keys.D))
            {
                if ((MyPlayer.Position.X + MyPlayer.Speed) < 1275)
                {
                    MyPlayer.Position.X += MyPlayer.Speed;
                }
            }
            if (newKeyState.IsKeyDown(Keys.A))
            {
                if ((MyPlayer.Position.X - MyPlayer.Speed) > 0)
                {
                    MyPlayer.Position.X -= MyPlayer.Speed;
                }
            }
            if (newKeyState.IsKeyDown(Keys.W))
            {
                if ((MyPlayer.Position.Y - MyPlayer.Speed) > 0)
                {
                    MyPlayer.Position.Y -= MyPlayer.Speed;
                }
            }
            if (newKeyState.IsKeyDown(Keys.S))
            {
                if ((MyPlayer.Position.Y + MyPlayer.Speed) < 715)
                {
                    MyPlayer.Position.Y += MyPlayer.Speed;
                }
            }
            if (newKeyState.IsKeyDown(Keys.K))
            {
                if (Boosts[Settings.ACCELERATION_BOOST])
                {
                    Boosts[Settings.ACCELERATION_BOOST] = false;
                }
                else
                {
                    Boosts[Settings.ACCELERATION_BOOST] = true;
                }
            }
            if (newKeyState.IsKeyDown(Keys.M))
            {
                if (Boosts[Settings.DAMAGE_BOOST])
                {
                    Boosts[Settings.DAMAGE_BOOST] = false;
                }
                else
                {
                    Boosts[Settings.DAMAGE_BOOST] = true;
                }
            }
            if (newKeyState.IsKeyDown(Keys.Escape))
            {
                if (!Settings.IsGamePaused)
                {
                    Settings.IsGamePaused = true;
                }
            }
        }

        public void StartMusic()
        {
            if (Settings.IsMusicEnabled)
            {
                CurrentSong = SoundBanks[0].GetCue("MusicCue");
                CurrentSong.Play();
            }
            else
            {
                StopMusic();
            }
        }

        public void StopMusic()
        {
            CurrentSong.Stop(AudioStopOptions.Immediate);
        }
    }
}
