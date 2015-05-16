using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATD.InGameObjects
{
    public interface IInGameObject
    {
        void Update(Game game, GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        bool ShouldBeRemoved();
    }
}
