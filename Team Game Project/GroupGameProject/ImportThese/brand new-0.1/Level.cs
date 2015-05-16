using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    //Holds the level
    public class Level : IGameObject
    {
        Tile[][] tiles;
        public Tile selectedTile;
        int LevelWidth = 20, LevelHeight = 20;
        Base moonBase;
        public Level()
        {
            //Added support for loading files
            Color[] pixels = new Color[ResourceManager.levelTexture.Width * ResourceManager.levelTexture.Height];
            ResourceManager.levelTexture.GetData<Color>(pixels);
            Random rnd = new Random();
            //Set up array
            tiles = new Tile[LevelWidth][];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile[LevelHeight];
            }

            //Initialise array
            for (int y = 0; y < tiles.Length; y++)
            {
                for (int x = 0; x < tiles[y].Length; x++)
                {
                    Color pixel = pixels[(20 * y) + x];
                    if (pixel == Color.Gray) tiles[x][y] = new Tile(rnd.Next() % 3, x, y);
                    else if (pixel == Color.Red) tiles[x][y] = new Tile(3, x, y);
                    else if (pixel == Color.Yellow) tiles[x][y] = new Tile(4, x, y);
                }
            }
            moonBase = new Base();
            selectedTile = moonBase.items[0];
        }

        public void Update(Game1 game)
        {

        }

        public void UpdateTile(int x, int y)
        {
            if (x < 20 && y < 20 && x >= 0 && y >= 0)
            {
                selectedTile.isSelected = false;
                if (x == 14 && y == 16) selectedTile = moonBase.items[0];
                else if ((x == 14 || x == 13) && y == 17) selectedTile = moonBase.items[1];
                else if ((x == 14 || x == 13) && y == 18) selectedTile = moonBase.items[2];
                else if (x == 13 && y == 19) selectedTile = moonBase.items[3];
                else if (x == 12 && (y == 19 || y == 18)) selectedTile = moonBase.items[4];
                else if (x == 11 && (y == 19 || y == 18)) selectedTile = moonBase.items[5];
                else if (x == 10 && (y == 19 || y == 18)) selectedTile = moonBase.items[6];
                else selectedTile = tiles[x][y];
                selectedTile.isSelected = true;
            }
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
            moonBase.Draw(sb);
        }
    }
}
