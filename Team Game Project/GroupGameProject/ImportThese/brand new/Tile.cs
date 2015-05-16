using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    //Each spuare that is on the map.
    public class Tile : IGameObject
    {
        public enum TileType
        {
            tile1,
            tile2,
            tile3,
            baseTile,
            emptyTile,
        }
        public TileType type;
        public BaseItem.BaseItemTypes baseItemType;
        public int x, y;
        Rectangle destinationRectangle;
        public bool isSelected = false;
        public Tile(int t, int x, int y)
        {
            if (t == 0) type = TileType.tile1;
            if (t == 1) type = TileType.tile2;
            if (t == 2) type = TileType.tile3;
            if (y > 15 && (x > 5 && x < 15)) type = TileType.emptyTile;
            this.x = x;
            this.y = y;
            destinationRectangle = new Rectangle(x * 32, y * 32, 32, 32);
        }
        public void Update(Game1 game)
        {
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if(type == TileType.tile1) sb.Draw(ResourceManager.tile1, destinationRectangle, isSelected? Color.Green : Color.White);
            else if (type == TileType.tile2) sb.Draw(ResourceManager.tile2, destinationRectangle, isSelected ? Color.Green : Color.White);
            else if (type == TileType.tile3) sb.Draw(ResourceManager.tile3, destinationRectangle, isSelected ? Color.Green : Color.White);
            else if (type == TileType.baseTile) sb.Draw(ResourceManager.exampleTower, destinationRectangle, isSelected ? Color.Green : Color.White);
        }
    }
}
