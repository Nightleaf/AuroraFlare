using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using AuroraFlare.Utilities;

namespace AuroraFlare.Model.Entities
{
    class Player : Entity
    {

        public String PlayerName;

        public void Initialize(String name)
        {
            this.LoadProfile(name);
            this.Speed = 4.0f;
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateShields(gameTime);
            this.UpdateAngle(gameTime);
        }

        public override void Render(Texture2D sprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, this.Position, null, Color.White, this.Angle, new Vector2(sprite.Width / 2, sprite.Height / 2), 1.0f, SpriteEffects.None, 0f);
        }

        public override void OnDeath()
        {

        }

        /// <summary>
        /// Loads the players profile.
        /// </summary>
        /// <param name="ProfileName">The name of the profile.</param>
        public void LoadProfile(String ProfileName)
        {
            FileStream fileStream;
            BinaryReader reader;
            try
            {
                fileStream = new FileStream(Settings.ProfileDirectory + ProfileName + ".bin", FileMode.Open);
                reader = new BinaryReader(fileStream);
                this.PlayerName = reader.ReadString();
                this.MaxHealth = reader.ReadDouble();
                this.MaxShields = reader.ReadDouble();
                this.CurrentHealth = this.MaxHealth;
                this.CurrentShields = this.MaxShields;
                reader.Close();
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return;
            }
        }

        /// <summary>
        /// Saves the player's profile.
        /// </summary>
        public void SaveProfile()
        {
            FileStream fileStream;
            BinaryWriter writer;
            try
            {
                fileStream = new FileStream(Settings.ProfileDirectory + this.PlayerName + ".bin", FileMode.Create);
                writer = new BinaryWriter(fileStream);
                writer.Write((String)this.PlayerName);
                writer.Write((double)this.MaxHealth);
                writer.Write((double)this.MaxShields);
                writer.Close();
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return;
            }
        }
    }
}
