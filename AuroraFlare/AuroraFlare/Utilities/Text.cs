using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AuroraFlare.Utilities
{
    class Text
    {
        public static void DrawStringEffect(SpriteBatch spriteBatch, SpriteFont font, String text, Vector2 location, Color mainColor, Color backgroundColor)
        {
            spriteBatch.DrawString(font, text, location, backgroundColor);
            spriteBatch.DrawString(font, text, new Vector2(location.X - 1, location.Y), mainColor);
        }
    }
}
