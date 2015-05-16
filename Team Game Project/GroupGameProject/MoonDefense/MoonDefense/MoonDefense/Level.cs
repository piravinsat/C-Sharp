using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonDefense
{
    //Holds the level
    class Level : IGameObject
    {
        Tile[][] tiles;
        int LevelWidth = 20, LevelHeight = 20;
        public Level()
        {
            tiles = new Tile[LevelWidth][];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile[LevelHeight];
            }
        }

        public void Update(Game1 game)
        {

        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            for (int y = 0; y < tiles.Length; y++)
            {
                for (int x = 0; x < tiles[y].Length; x++)
                {
                    tiles[x][y].Draw(sb);
                }
            }
        }
    }
}
