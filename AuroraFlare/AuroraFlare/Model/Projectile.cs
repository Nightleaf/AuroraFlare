using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AuroraFlare.Model.Entities;

namespace AuroraFlare.Model
{
    class Projectile
    {
        // The source of the projectile.
        public Entity source;

        // Image representing the Projectile
        public Texture2D Texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        public Vector2 InitialPosition;

        // The outline of the rectangle
        public Rectangle MyRectangle;

        // Where the Projectile needs to go, relative to the target.
        public Vector2 Target;

        public Vector2 Motion;

        // The speed of the projectile.
        public float Speed;

        // The angle of the projectile.
        public float ProjectileAngle;

        /// <summary>
        /// Projectile Constructor
        /// </summary>
        /// <param name="projectile">The 'projectile' we fired.</param>
        /// <param name="position">The position where it was fired.</param>
        /// <param name="angle">The angle of the ship when it was fired.</param>
        public Projectile(Entity source, Texture2D projectile, Vector2 position, Vector2 target, float speed, float angle)
        {
            this.source = source;
            this.Texture = projectile;
            this.Position = position;
            this.InitialPosition = source.Position;
            this.Target = target;
            this.Speed = speed;
            this.ProjectileAngle = angle;
        }

        public void UpdateAngle()
        {
            this.Motion = -(this.Target - this.InitialPosition);
            if (this.Motion != Vector2.Zero)
                this.Motion.Normalize();
        }
    }
}
