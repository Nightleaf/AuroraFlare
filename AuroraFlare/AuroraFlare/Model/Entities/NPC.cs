using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AuroraFlare.Model.Entities
{
    class NPC : Entity
    {

        public NPC()
        {
            this.Initialize();    
        }

        public void Initialize()
        {
            this.MaxHealth = 100;
            this.CurrentHealth = 100;
            this.MaxShields = 30;
            this.CurrentShields = 30;
            this.IsDead = false;
        }

        public override void Update(GameTime gameTime)
        {
            this.LastFired += (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.UpdateShields(gameTime);
            this.UpdateAngle(gameTime);
            this.fire(ProjectileManager.projectileSprites[4], getNearestPlayer());
        }

        public override void Render(Texture2D sprite, SpriteBatch spriteBatch)
        {
            if (!this.IsDead)
            {
                Vector2 Origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
                spriteBatch.Draw(sprite, this.Position, null, Color.White, this.Angle, Origin, 1.0f, SpriteEffects.None, 0f);
            }
        }

        public override void OnDeath()
        {
            this.shouldBeRemoved = true;
        }

        /// <summary>
        /// Fires a projectile at the target.
        /// </summary>
        public void fire(Texture2D sprite, Vector2 targetPosition)
        {
            if (this.LastFired >= 5.5f)
            {
                ProjectileManager.AddProjectile(this, sprite, targetPosition);
            }
        }

        /// <summary>
        /// Gets the nearest player.
        /// </summary>
        /// <returns></returns>
        public Vector2 getNearestPlayer()
        {
            Vector2 Target = new Vector2();
            Vector2 MyTarget = new Vector2();
            foreach (Entity entity in EntityManager.EntityList)
            {
                if (entity != null)
                {
                    if (entity is Player)
                    {
                        if (!entity.IsDead)
                        {
                            MyTarget = new Vector2(entity.Position.X, entity.Position.Y);
                            break;
                        }
                    }
                }
            }
            return Target = new Vector2(MyTarget.X, MyTarget.Y);
        }
    }
}
