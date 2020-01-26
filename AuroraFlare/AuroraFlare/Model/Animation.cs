using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AuroraFlare.Model
{
    class Animation
    {
        // The sprite sheet for the animation.
        public Texture2D SpriteSheet;

        // The width of each frame.
        public int frameSizeX;

        // The height of each frame.
        public int frameSizeY;

        // The total amount of frames in the animation.
        public int animationLength;

        // The current frame the animation is on.
        public int currentFrame;

        // Should the animation be looping.
        public Boolean isLooping;

        public Animation(Texture2D spritesheet, int frameWidth, int frameHeight, int frameCount)
        {
            this.SpriteSheet = spritesheet;
            this.frameSizeX = frameWidth;
            this.frameSizeY = frameHeight;
            this.animationLength = frameCount;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Update(GameTime gameTime)
        {
            if (this.isLooping)
            {
                this.currentFrame++;
                if (this.currentFrame >= this.animationLength)
                {
                    this.currentFrame = 0;
                }
            }
        }
    }
}
