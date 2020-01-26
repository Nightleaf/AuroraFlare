using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuroraFlare.Model.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AuroraFlare.Model
{
    class ProjectileManager
    {
        /// <summary>
        /// The Entity projectile list.
        /// </summary>
        public static List<Projectile> EntityProjectileList = new List<Projectile>();

        /// <summary>
        /// Projectile Sprites
        /// </summary>
        public static Texture2D[] projectileSprites;

        public static void LoadProjectileContent(ContentManager content)
        {
            projectileSprites = new Texture2D[5];
            projectileSprites[0] = content.Load<Texture2D>("Game/Projectiles/BEAM_BLUE_SPRITE");
            projectileSprites[1] = content.Load<Texture2D>("Game/Projectiles/BEAM_RED_SPRITE");
            projectileSprites[2] = content.Load<Texture2D>("Game/Projectiles/LaserBall");
            projectileSprites[3] = content.Load<Texture2D>("Game/Projectiles/LaserSmall");
            projectileSprites[4] = content.Load<Texture2D>("Game/Projectiles/LaserBallPurple");
        }

        /// <summary>
        /// Updates all projectiles.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            for (int index = 0; index < EntityProjectileList.Count; index++)
            {
                Projectile projectile = EntityProjectileList[index];
                if (projectile != null)
                {
                    projectile.Motion = (projectile.Target - projectile.InitialPosition);
                    if (projectile.Motion != Vector2.Zero)
                        projectile.Motion.Normalize();
                    projectile.Position.X += projectile.Motion.X * projectile.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    projectile.Position.Y += projectile.Motion.Y * projectile.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (projectile.Position.X + projectile.Texture.Width / 2 > Main.viewport.Width)
                    {
                        RemoveProjectile(projectile, index);
                    }
                    if (projectile.Position.Y + projectile.Texture.Height / 2 > Main.viewport.Height)
                    {
                        RemoveProjectile(projectile, index);
                    }
                    if (projectile.Position.X < 0)
                    {
                        RemoveProjectile(projectile, index);
                    }
                    if (projectile.Position.Y < 0)
                    {
                        RemoveProjectile(projectile, index);
                    }
                    projectile.MyRectangle = new Rectangle((int)projectile.Position.X, (int)projectile.Position.Y, projectile.Texture.Width, projectile.Texture.Height);
                }
            }
        }

        /// <summary>
        /// Draws all projectiles.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in EntityProjectileList)
            {
                if (projectile != null)
                {
                    Vector2 projorigin = new Vector2((projectile.Texture.Width / 2), (projectile.Texture.Height / 2));
                    spriteBatch.Draw(projectile.Texture, projectile.Position, null, Color.White, projectile.ProjectileAngle, projorigin, 1.0f, SpriteEffects.None, 0f);
                }
            }
        }

        /// <summary>
        /// Adds a projectile to the EntityProjectileList.
        /// </summary>
        /// <param name="source">The entity which fired the projectile.</param>
        /// <param name="targetPosition">The position of the reticule when fired.</param>
        public static void AddProjectile(Entity source, Texture2D texture, Vector2 targetPosition)
        {
            Projectile projectile = new Projectile(source, texture, source.Position, targetPosition, 650f, source.Angle);
            EntityProjectileList.Add(projectile);
        }

        /// <summary>
        /// Removes a projectile from the EntityProjectileList.
        /// </summary>
        /// <param name="projectile">The projectile we are removing.</param>
        /// <param name="index">The index from the list.</param>
        public static void RemoveProjectile(Projectile projectile, int index) 
        {
            if (EntityProjectileList.Count > 0)
            {
                if (EntityProjectileList[index] != null)
                {
                    EntityProjectileList.RemoveAt(index);
                }
            }
        }
    }
}
