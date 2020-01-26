using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuroraFlare.State;
using AuroraFlare.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.RegularExpressions;
using System.IO;
using AuroraFlare.Model.Entities;

namespace AuroraFlare.Model.State.impl
{
    class ProfileState : GameState
    {
        private static KeyboardState oldKeyState;
        private static KeyboardState newKeyState = Keyboard.GetState();
        private static MouseState oldMouseState;
        private static MouseState newMouseState = Mouse.GetState();

        List<Player> ProfileList;

        SpriteFont[] ProfileFonts;

        Texture2D[] Backgrounds;
        Texture2D[] Buttons;
        Texture2D[] Headers;

        Player MyProfile;

        bool CreatingProfile;
        bool DrawConfirmationBox;
        bool[] ProfileSelected;

        String keysTyped;
        String profileToCreate;

        Keys[] MyKeys = new Keys[] {
            Keys.A, Keys.B, Keys.C, Keys.D,
            Keys.E, Keys.F, Keys.G, Keys.H, 
            Keys.I, Keys.J, Keys.K, Keys.L, 
            Keys.M, Keys.N, Keys.O, Keys.P, 
            Keys.Q, Keys.R, Keys.S, Keys.T,
            Keys.U, Keys.V, Keys.W, Keys.X,
            Keys.Y, Keys.Z, Keys.Back, Keys.Space
        };


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
            ProfileSelected = new bool[3];
            CreateProfileDirectory();
            LoadAllProfiles();
            Backgrounds = new Texture2D[7];
            Backgrounds[0] = content.Load<Texture2D>("MainBG");
            Backgrounds[1] = content.Load<Texture2D>("Profile/AtlarProfileSelect");
            Backgrounds[2] = content.Load<Texture2D>("Profile/NPBackground");
            Backgrounds[3] = content.Load<Texture2D>("Profile/NPInput");
            Backgrounds[4] = content.Load<Texture2D>("Profile/ProfileAvBG");
            Backgrounds[5] = content.Load<Texture2D>("Profile/ProfileAvBGActive");
            Backgrounds[6] = content.Load<Texture2D>("DialogueBox");
            Buttons = new Texture2D[10];
            Buttons[0] = content.Load<Texture2D>("Profile/PlayButtonActive");
            Buttons[1] = content.Load<Texture2D>("Profile/PlayButtonBlacked");
            Buttons[2] = content.Load<Texture2D>("Profile/CreateNewButton");
            Buttons[3] = content.Load<Texture2D>("Profile/CreateNewActive");
            Buttons[4] = content.Load<Texture2D>("Profile/LeftArrowButton");
            Buttons[5] = content.Load<Texture2D>("Profile/RightArrowButton");
            Buttons[6] = content.Load<Texture2D>("Profile/NPAccept");
            Buttons[7] = content.Load<Texture2D>("Profile/NPCancel");
            Buttons[8] = content.Load<Texture2D>("Profile/DeleteButton");
            Buttons[9] = content.Load<Texture2D>("Profile/DeleteButtonActive");
            Headers = new Texture2D[2];
            Headers[0] = content.Load<Texture2D>("Profile/ProfilesHeader");
            Headers[1] = content.Load<Texture2D>("Profile/OverviewHeader");
            ProfileFonts = new SpriteFont[4];
            ProfileFonts[0] = content.Load<SpriteFont>("Fonts/ProfileFont");
            ProfileFonts[1] = content.Load<SpriteFont>("Fonts/SmallProfileFont");
            ProfileFonts[2] = content.Load<SpriteFont>("Fonts/InputFont");
            ProfileFonts[3] = content.Load<SpriteFont>("Fonts/DialogueFont");
            keysTyped = "";
            profileToCreate = "";
            DrawConfirmationBox = false;
            CreatingProfile = false;
            Initialized = true;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMouseInput(gameTime);
            UpdateKeyboardInput(gameTime);
        }

        public void UpdateMouseInput(GameTime gameTime)
        {
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
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

            // Play button
            if (PressedX >= 532 && PressedX <= 745 && PressedY >= 652 && PressedY <= 704)
            {
                if (IsProfileSelected())
                {
                    if (!DrawConfirmationBox)
                    {
                        SetProfileForPlay();
                        StateManager.EnterState((int)StateManager.GameStates.GAME_SCREEN);
                    }
                }
            }

            // Create new profile button
            if (PressedX >= 1006 && PressedX <= 1124 && PressedY >= 96 && PressedY <= 116)
            {
                if (CreatingProfile)
                {
                    keysTyped = "";
                    CreatingProfile = false;
                }
                else
                {
                    keysTyped = "";
                    CreatingProfile = true;
                }
            }

            // Accept the profile.
            if (PressedX >= 1007 && PressedX <= 1032 && PressedY >= 201 && PressedY <= 220)
            {
                String NameChosen = keysTyped;
                keysTyped = "";
                CreateProfile(NameChosen);
                ProfileSelected[0] = false;
                ProfileSelected[1] = false;
                ProfileSelected[2] = false;
                CreatingProfile = false;
                LoadAllProfiles();
            }

            // Cancel/decline the new profile.
            if (PressedX >= 1041 && PressedX <= 1069 && PressedY >= 202 && PressedY <= 219)
            {
                keysTyped = "";
                CreatingProfile = false;
            }

            // Delete the profile
            if (PressedX >= 872 && PressedX <= 990 && PressedY >= 96 && PressedY <= 116)
            {
                DrawConfirmationBox = false;
            }
            if (!DrawConfirmationBox)
            {
                // Selecting the profiles
                if (PressedX >= 298 && PressedX <= 443 && PressedY >= 151 && PressedY <= 293)
                {
                    if (!ProfileSelected[0])
                    {
                        ProfileSelected[0] = true;
                    }
                    else
                    {
                        ProfileSelected[0] = false;
                    }
                    ProfileSelected[1] = false;
                    ProfileSelected[2] = false;
                }
                // Selecting the profiles
                if (PressedX >= 568 && PressedX <= 714 && PressedY >= 150 && PressedY <= 294)
                {
                    if (!ProfileSelected[1])
                    {
                        ProfileSelected[1] = true;
                    }
                    else
                    {
                        ProfileSelected[0] = false;
                        ProfileSelected[1] = false;
                        ProfileSelected[2] = false;
                    }
                    ProfileSelected[0] = false;
                    ProfileSelected[2] = false;
                }
                // Selecting the profiles
                if (PressedX >= 837 && PressedX <= 985 && PressedY >= 150 && PressedY <= 293)
                {
                    if (!ProfileSelected[2])
                    {
                        ProfileSelected[2] = true;
                    }
                    else
                    {
                        ProfileSelected[0] = false;
                        ProfileSelected[1] = false;
                        ProfileSelected[2] = false;
                    }
                    ProfileSelected[0] = false;
                    ProfileSelected[1] = false;
                }
            }
            if (DrawConfirmationBox)
            {
                // Accept the deletion
                if (PressedX >= 607 && PressedX <= 632 && PressedY >= 527 && PressedY <= 547)
                {
                    DrawConfirmationBox = false;
                    LoadAllProfiles();
                }
                // Decline the deletion
                if (PressedX >= 646 && PressedX <= 674 && PressedY >= 526 && PressedY <= 547)
                {
                    DrawConfirmationBox = false;
                }
            }
        }

        public void UpdateKeyboardInput(GameTime gameTime)
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();
            foreach (Keys key in MyKeys)
            {
                if (CheckKeys(key))
                {
                    AddKeyToText(key);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(Backgrounds[0], new Vector2(0, 0), Color.White);
            spriteBatch.Draw(Backgrounds[1], new Vector2((Settings.ScreenResolutionWidth / 2 - Backgrounds[1].Width / 2), (Settings.ScreenResolutionHeight / 2 - Backgrounds[1].Height / 2)), Color.White);
            if (!IsProfileSelected())
            {
                spriteBatch.Draw(Buttons[1], new Vector2((Settings.ScreenResolutionWidth / 2 - Buttons[0].Width / 2), (Settings.ScreenResolutionHeight / 2 - Buttons[0].Height / 2) + 320), Color.White);
            }
            else if (IsProfileSelected())
            {
                spriteBatch.Draw(Buttons[0], new Vector2((Settings.ScreenResolutionWidth / 2 - Buttons[0].Width / 2), (Settings.ScreenResolutionHeight / 2 - Buttons[0].Height / 2) + 320), Color.White);
            }
            spriteBatch.Draw(Headers[0], new Vector2(155, 95), Color.White);
            spriteBatch.Draw(Headers[1], new Vector2(155, 370), Color.White);
            if (GetProfileCount() <= 2)
            {
                spriteBatch.Draw(Buttons[2], new Vector2(1005, 95), Color.White);
            }
            DrawProfiles(gameTime, spriteBatch, graphicsDevice);
            /*if (IsProfileSelected())
            {
                spriteBatch.Draw(Buttons[8], new Vector2(870, 95), Color.White);
            }*/
            if (CreatingProfile)
            {
                spriteBatch.Draw(Backgrounds[2], new Vector2(805, 125), Color.White);
                spriteBatch.Draw(Buttons[6], new Vector2(1005, 200), Color.White);
                spriteBatch.Draw(Buttons[7], new Vector2(1040, 200), Color.White);
                spriteBatch.Draw(Backgrounds[3], new Vector2(811, 175), Color.White);
                spriteBatch.DrawString(ProfileFonts[1], "Creating Profile", new Vector2(819, 149), new Color(0, 216, 255));
                spriteBatch.DrawString(ProfileFonts[2], keysTyped + "*", new Vector2(817, 177), Color.White);
            }
            if (DrawConfirmationBox)
            {
                spriteBatch.Draw(Backgrounds[6], new Vector2((Settings.ScreenResolutionWidth / 2 - Backgrounds[6].Width / 2), (Settings.ScreenResolutionHeight / 2 - Backgrounds[6].Height / 2) + 150), Color.White);
                spriteBatch.DrawString(ProfileFonts[3], "Are you sure you wish to delete this profile?", new Vector2(525, 488), Color.Red);
                spriteBatch.Draw(Buttons[6], new Vector2(605, 525), Color.White);
                spriteBatch.Draw(Buttons[7], new Vector2(645, 525), Color.White);
            }
        }

        public void DrawProfiles(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (GetProfileCount() > 0)
            {
                for (int i = 0; i < GetProfileCount(); i++)
                {
                    Player prof = ProfileList[i];
                    if (prof != null)
                    {
                        String[] files = Directory.GetFiles(Settings.ProfileDirectory);
                        prof.PlayerName = files[i];
                        prof.PlayerName = Regex.Replace(prof.PlayerName, "./Profiles/", "");
                        prof.PlayerName = Regex.Replace(prof.PlayerName, ".bin", "");
                        Boolean[] HasBeenDrawn = new Boolean[4];
                        if (i == 0)
                        {
                            if (!HasBeenDrawn[0])
                            {
                                if (ProfileSelected[0])
                                {
                                    spriteBatch.Draw(Backgrounds[5], new Vector2(285, 140), Color.White);
                                }
                                if (!ProfileSelected[0])
                                {
                                    spriteBatch.Draw(Backgrounds[4], new Vector2(285, 140), Color.White);
                                }
                                spriteBatch.DrawString(ProfileFonts[1], prof.PlayerName, new Vector2(348, 325), new Color(0, 216, 255));
                                HasBeenDrawn[0] = true;
                            }
                        }
                        if (i == 1)
                        {
                            prof.PlayerName = Regex.Replace(prof.PlayerName, ".bin", "");
                            if (!HasBeenDrawn[1])
                            {
                                if (ProfileSelected[1])
                                {
                                    spriteBatch.Draw(Backgrounds[5], new Vector2(555, 140), Color.White);
                                }
                                if (!ProfileSelected[1])
                                {
                                    spriteBatch.Draw(Backgrounds[4], new Vector2(555, 140), Color.White);
                                }
                                spriteBatch.DrawString(ProfileFonts[1], prof.PlayerName, new Vector2(620, 325), new Color(0, 216, 255));
                                HasBeenDrawn[1] = true;
                            }
                        }
                        if (i == 2)
                        {
                            prof.PlayerName = Regex.Replace(prof.PlayerName, ".bin", "");
                            if (!HasBeenDrawn[2])
                            {
                                if (ProfileSelected[2])
                                {
                                    spriteBatch.Draw(Backgrounds[5], new Vector2(825, 140), Color.White);
                                }
                                if (!ProfileSelected[2])
                                {
                                    spriteBatch.Draw(Backgrounds[4], new Vector2(825, 140), Color.White);
                                }
                                spriteBatch.DrawString(ProfileFonts[1], prof.PlayerName, new Vector2(885, 325), new Color(0, 216, 255));
                                HasBeenDrawn[2] = true;
                            }
                        }
                    }
                }
            }
        }

        private bool CheckKeys(Keys key)
        {
            return oldKeyState.IsKeyDown(key) && newKeyState.IsKeyUp(key);
        }

        private void AddKeyToText(Keys key)
        {
            String newChar = "";
            if (keysTyped.Length >= 20)
                return;

            switch (key)
            {
                case Keys.A:
                    newChar += "a";
                    break;
                case Keys.B:
                    newChar += "b";
                    break;
                case Keys.C:
                    newChar += "c";
                    break;
                case Keys.D:
                    newChar += "d";
                    break;
                case Keys.E:
                    newChar += "e";
                    break;
                case Keys.F:
                    newChar += "f";
                    break;
                case Keys.G:
                    newChar += "g";
                    break;
                case Keys.H:
                    newChar += "h";
                    break;
                case Keys.I:
                    newChar += "i";
                    break;
                case Keys.J:
                    newChar += "j";
                    break;
                case Keys.K:
                    newChar += "k";
                    break;
                case Keys.L:
                    newChar += "l";
                    break;
                case Keys.M:
                    newChar += "m";
                    break;
                case Keys.N:
                    newChar += "n";
                    break;
                case Keys.O:
                    newChar += "o";
                    break;
                case Keys.P:
                    newChar += "p";
                    break;
                case Keys.Q:
                    newChar += "q";
                    break;
                case Keys.R:
                    newChar += "r";
                    break;
                case Keys.S:
                    newChar += "s";
                    break;
                case Keys.T:
                    newChar += "t";
                    break;
                case Keys.U:
                    newChar += "u";
                    break;
                case Keys.V:
                    newChar += "v";
                    break;
                case Keys.W:
                    newChar += "w";
                    break;
                case Keys.X:
                    newChar += "x";
                    break;
                case Keys.Y:
                    newChar += "y";
                    break;
                case Keys.Z:
                    newChar += "z";
                    break;
                case Keys.Space:
                    newChar += "_";
                    break;
                case Keys.Back:
                    if (keysTyped.Length != 0)
                        keysTyped = keysTyped.Remove(keysTyped.Length - 1);
                    return;
            }
            if (newKeyState.IsKeyDown(Keys.RightShift) || newKeyState.IsKeyDown(Keys.LeftShift))
            {
                newChar = newChar.ToUpper();
            }
            keysTyped += newChar;
        }

        /// <summary>
        /// This method will create the profile directory if it doesn't exist.
        /// </summary>
        public void CreateProfileDirectory()
        {
            String dir = Settings.ProfileDirectory;
            if (!Directory.Exists(dir))
            {
                DirectoryInfo di = Directory.CreateDirectory(dir);
            }

        }

        /// <summary>
        /// How many profiles that are currently saved.
        /// </summary>
        /// <returns></returns>
        public int GetProfileCount()
        {
            String[] profiles;
            profiles = Directory.GetFiles(Settings.ProfileDirectory);
            return profiles.Length;
        }

        /// <summary>
        /// Creates a new profile.
        /// </summary>
        public void CreateProfile(String name)
        {
            if (File.Exists(Settings.ProfileDirectory + "" + name))
            {
                return;
            }
            FileStream fileStream;
            BinaryWriter writer;
            try
            {
                fileStream = new FileStream(Settings.ProfileDirectory + name + ".bin", FileMode.Create);
                writer = new BinaryWriter(fileStream);
                writer.Write((String)name);
                writer.Write((double)250);
                writer.Write((double)75);
                writer.Write((int)0);
                writer.Write((int)0);
                writer.Write((int)0);
                writer.Close();
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return;
            }
        }

        public void LoadAllProfiles()
        {
            ProfileList = new List<Player>(GetProfileCount());
            String[] profileNames = Directory.GetFiles(Settings.ProfileDirectory);
            for (int i2 = 0; i2 < GetProfileCount(); i2++)
            {
                String fixedName = profileNames[i2].Replace("./Profiles/", "");
                Player p = new Player();
                p.PlayerName = fixedName;
                AddProfile(p);
            }
        }

        /// <summary>
        /// This will add a profile to the list so it can be managed.
        /// </summary>
        /// <param name="profile"></param>
        public void AddProfile(Player profile)
        {
            if (!ProfileList.Contains(profile))
            {
                ProfileList.Add(profile);
            }
        }

        public Boolean IsProfileSelected()
        {
            for (int i = 0; i < ProfileSelected.Length; i++)
            {
                if (ProfileSelected[i])
                {
                    return true;
                }
            }
            return false;
        }

        public void SetProfileForPlay()
        {
            if (ProfileSelected[0])
            {
                Settings.PROFILE_TO_USE = ProfileList[0].PlayerName;
            }
            else if (ProfileSelected[1])
            {
                Settings.PROFILE_TO_USE = ProfileList[1].PlayerName;
            }
            else if (ProfileSelected[2])
            {
                Settings.PROFILE_TO_USE = ProfileList[2].PlayerName;
            }
            Console.WriteLine("PROFILE SELECTED: " + Settings.PROFILE_TO_USE);
        }
    }
}
