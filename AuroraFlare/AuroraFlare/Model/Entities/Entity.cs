using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AuroraFlare.Model.Entities
{
    abstract class Entity
    {
        /// <summary>
        /// The entities position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Used for locking on to a Player.
        /// </summary>
        Vector2 target;

        /// <summary>
        /// The entities current health.
        /// </summary>
        public double CurrentHealth;

        /// <summary>
        /// The entities max health.
        /// </summary>
        public double MaxHealth;

        /// <summary>
        /// The entities current shields.
        /// </summary>
        public double CurrentShields;

        /// <summary>
        /// The entities max shields.
        /// </summary>
        public double MaxShields;

        /// <summary>
        /// The delay for the shield regeneration.
        /// </summary>
        public float ShieldRegen;

        /// <summary>
        /// Whether or not the entity is dead.
        /// </summary>
        public bool IsDead;

        /// <summary>
        /// For NPC's to be removed after they die.
        /// </summary>
        public bool shouldBeRemoved;

        /// <summary>
        /// The entities angle.
        /// </summary>
        public float Angle;

        /// <summary>
        /// The entities speed.
        /// </summary>
        public float Speed;

        /// <summary>
        /// The last time the entity got hit.
        /// </summary>
        public float LastHit;

        /// <summary>
        /// The last time the entity fired.
        /// </summary>
        public float LastFired;

        /// <summary>
        /// The dimensions for the clipping system.
        /// </summary>
        public Rectangle MyRectangle;

        public Entity()
        {
            EntityManager.EntityList = new List<Entity>();
            EntityManager.AddEntity(this);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Render(Texture2D sprite, SpriteBatch spriteBatch);

        /// <summary>
        /// Updates the entities shields.
        /// </summary>
        /// <param name="gameTime"></param>
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
                    double result = this.CurrentShields + 5;
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
                    double result = this.CurrentShields + 10;
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

        /// <summary>
        /// Appends a certain amount of damage to the entity.
        /// </summary>
        /// <param name="damage"></param>
        public void AppendDamage(float damage)
        {
            bool shieldUsed = false;
            double remainder = 0;
            if (this.IsDead)
                return;
            this.LastHit = 0f;
            if (this.CurrentShields > 0)
            {
                shieldUsed = true;
                double res = this.CurrentShields -= damage;
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
                    // The remainder will always be negative so you want to add it to the health, rather than subtract.
                    this.CurrentHealth += remainder;
                    remainder = 0;
                    shieldUsed = false;
                }
                else
                {
                    double res = this.CurrentHealth -= damage;
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
                this.OnDeath();
            }
        }

        public void UpdateAngle(GameTime gameTime)
        {
            if (this.IsDead)
            {
                return;
            }
            if (this is Player) {
                float XDistance = this.Position.X - Main.newMouseState.X;
                float YDistance = this.Position.Y - Main.newMouseState.Y;
                this.Angle = (float)Math.Atan2(YDistance, XDistance);
            }
            if (this is NPC)
            {
                foreach (Entity entity in EntityManager.EntityList) {
                    if (entity != null) {
                        
                        if (entity is Player)
                        {
                            target = entity.Position;
                        }
                    }
                }
                float XDistance = this.Position.X - target.X;
                float YDistance = this.Position.Y - target.Y;
                this.Angle = -(float)Math.Atan2(XDistance, YDistance);
            }
        }

        public abstract void OnDeath();
    }
}
