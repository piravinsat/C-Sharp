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
            spawnerTile
        }
        public TileType type;
        public BaseItem.BaseItemTypes baseItemType;
        public int x, y;
        Rectangle destinationRectangle;
        public bool isSelected = false, isEmpty = true;
        public Tile(int t, int x, int y)
        {
            if (t == 0) type = TileType.tile1;
            else if (t == 1) type = TileType.tile2;
            else if (t == 2) type = TileType.tile3;
            if (t == 3) type = TileType.spawnerTile;
            if (t == 4) type = TileType.baseTile;
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
            else if (type == TileType.spawnerTile) sb.Draw(ResourceManager.spawnerTile, destinationRectangle, isSelected ? Color.Green : Color.White);
        }
    }
}
