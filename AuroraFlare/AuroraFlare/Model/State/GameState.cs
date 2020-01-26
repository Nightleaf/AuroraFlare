using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AuroraFlare.State
{
    abstract class GameState
    {
        public abstract void Enter();
        public abstract void Leave();
        public abstract void Initialize(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);

        public bool Initialized;
    }
}
