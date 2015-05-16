using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonDefense
{
    //The super class of all towers, needs sub classes.
    class Tower : IGameObject
    {
        public void Update(Game1 game)
        {
            throw new NotImplementedException();
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            throw new NotImplementedException();
        }
    }
}
