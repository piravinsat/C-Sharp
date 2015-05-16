using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MoonDefense
{
    interface IGameObject
    {
        void Update(Game1 game);
        void Draw(SpriteBatch sb);
    }
}
