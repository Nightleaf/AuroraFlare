using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuroraFlare.State;
using AuroraFlare.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AuroraFlare.Model.State.impl
{
    class OptionsState : GameState
    {

        public override void Enter()
        {
            Settings.ShowMouse = true;
        }

        public override void Leave()
        {
            throw new NotImplementedException();
        }

        public override void Initialize(ContentManager content)
        {
            Initialized = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            throw new NotImplementedException();
        }
    }
}
