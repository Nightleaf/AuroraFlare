using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AuroraFlare.Model.Entities
{
    class Mob
    {
        Random rand = new Random();

        public float[] FireSpeeds = { 550, 600, 650 };

        /// <summary>
        /// The sprite of the mob.
        /// </summary>
        public Texture2D Sprite;

        /// <summary>
        /// The mob's current health.
        /// </summary>
        public float CurrentHealth;

        /// <summary>
        /// The mob's max health.
        /// </summary>
        public float MaxHealth;

        /// <summary>
        /// The mob's current shields.
        /// </summary>
        public float CurrentShields;

        /// <summary>
        /// The mob's max shields.
        /// </summary>
        public float MaxShields;

        /// <summary>
        /// The last time the mob got hit.
        /// </summary>
        public float LastHit;

        /// <summary>
        /// The last time the mob fired at the player.
        /// </summary>
        public float LastFired;

        /// <summary>
        /// The delay for the shield regeneration.
        /// </summary>
        public float ShieldRegen;

        /// <summary>
        /// Whether or not the mob is dead.
        /// </summary>
        public bool IsDead;

        /// <summary>
        /// Whethor or not the mob needs to be removed.
        /// </summary>
        public bool HasToBeRemoved;

        /// <summary>
        /// Whethor or not the mob's point value has been displayed after dying.
        /// </summary>
        public bool HasScoreBeenDrawn;

        /// <summary>
        /// The mob's position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The outline of the mob.
        /// </summary>
        public Rectangle MyRectangle;

        /// <summary>
        /// The mob's angle.
        /// </summary>
        public float Angle;

        /// <summary>
        /// The mob's speed.
        /// </summary>
        public float Speed;

        /// <summary>
        /// The mob's firing speed.
        /// </summary>
        public float FireSpeed;
        

        /// <summary>
        /// The mob's bounty.
        /// </summary>
        public int Bounty;

        /// <summary>
        /// Mob Constructor
        /// </summary>
        /// <param name="sprite">The mob's image.</param>
        /// <param name="health">The setting for the mob's health.</param>
        /// <param name="shield">The setting for the mob's shields.</param>
        /// <param name="pos">The setting for the mob's position.</param>
        public Mob(Texture2D sprite, float health, float shield, Vector2 pos)
        {
            this.Sprite = sprite;
            this.CurrentHealth = health;
            this.MaxHealth = health;
            this.CurrentShields = shield;
            this.MaxShields = shield;
            this.Position = pos;
            this.IsDead = false;
            this.HasToBeRemoved = false;
            this.HasScoreBeenDrawn = false;
            this.Speed = 4.2f;
            this.LastHit = 10f;
            this.LastFired = 0f;
            this.Bounty = (int)(this.MaxHealth + this.MaxShields);
            int choice = rand.Next(0, 2);
            this.FireSpeed = FireSpeeds[choice];
        }

        public Mob(Texture2D sprite, Vector2 pos)
        {
            this.Sprite = sprite;
            this.CurrentHealth = 100;
            this.MaxHealth = 100;
            this.CurrentShields = 30;
            this.MaxShields = 30;
            this.Position = pos;
            this.IsDead = false;
            this.HasToBeRemoved = false;
            this.HasScoreBeenDrawn = false;
            this.Speed = 4.2f;
            this.LastHit = 10f;
            this.LastFired = 0f;
            this.Bounty = 130;
            int choice = rand.Next(0, 2);
            this.FireSpeed = FireSpeeds[choice];
        }

        public void Update(GameTime gameTime)
        {
            if (!this.IsDead)
            {
                this.LastHit += (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.ShieldRegen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.LastFired += (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateShields(gameTime);
                UpdateMovement(gameTime);
                this.MyRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Sprite.Width, this.Sprite.Height);
            }
        }

        public void UpdateShields(GameTime gameTime)
        {
            if (this.CurrentShields == this.MaxShields)
            {
                return;
            }
            if (this.LastHit >= 2.5f && this.LastHit <= 10f)
            {
                if (this.ShieldRegen >= 1f)
                {
                    float result = this.CurrentShields + 5;
                    if (result > this.MaxShields)
                    {
                        this.CurrentShields = this.MaxShields;
                    }
                    else
                    {
                        this.CurrentShields += 5;
                    }
                    this.ShieldRegen = 0f;
                }
            }
            else if (this.LastHit >= 10f)
            {
                if (this.ShieldRegen >= 1f)
                {
                    float result = this.CurrentShields + 10;
                    if (result > this.MaxShields)
                    {
                        this.CurrentShields = this.MaxShields;
                    }
                    else
                    {
                        this.CurrentShields += 10;
                    }
                    this.ShieldRegen = 0f;
                }
            }
        }

        public void UpdateMovement(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (this != null && !this.IsDead)
            {
                Vector2 Origin = new Vector2(this.Sprite.Width / 2, this.Sprite.Height / 2);
                spriteBatch.Draw(this.Sprite, this.Position, null, Color.White, this.Angle, Origin, 1.0f, SpriteEffects.None, 0f);
            }
        }

        public void OnDeath()
        {
            // On random chance, maybe the enemy can drop some powerups, or boosters.
            this.HasToBeRemoved = true;
        }

        public void AppendDamage(float damage, Player player)
        {
            bool shieldUsed = false;
            float remainder = 0;
            if (this.IsDead)
                return;
            this.LastHit = 0f;
            if (this.CurrentShields > 0)
            {
                shieldUsed = true;
                float res = this.CurrentShields -= damage;
                if (res < 0)
                {
                    this.CurrentShields = 0;
                    remainder = res;
                }
            }
            if (this.CurrentHealth > 0)
            {
                if (shieldUsed)
                {
                    this.CurrentHealth += remainder; // The remainder will always be negative so you want to add it to the health, rather than subtract.
                    remainder = 0;
                    shieldUsed = false;
                }
                else
                {
                    float res = this.CurrentHealth -= damage;
                    if (res <= 0)
                    {
                        this.CurrentHealth = 0;
                        this.IsDead = true;
                    }
                }
            }
            if (this.CurrentHealth <= 0)
            {
                this.IsDead = true;
            }
            if (this.IsDead)
            {
                //player.Wealth += this.Bounty;
                OnDeath();
            }
        }
    }
}
