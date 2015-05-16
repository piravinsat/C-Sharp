using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    //The turrets or upgrades to buy from the base, needs sub classes
    public class BaseItem : Tile, IGameObject
    {
        //These need to be properly renamed
        public enum BaseItemTypes
        {
            electricTower,
            basicTower,
            waterTower,
            turrentTower,
            sniperTower,
            upgrade1,
            upgrade2,
        }
        Rectangle destinationRectangle;
        public BaseItem(int i, int x, int y) : base(3, x, y)
        {
            switch (i){
                case 0: baseItemType = BaseItemTypes.electricTower; destinationRectangle = new Rectangle(448, 512, 32, 32); break;
                case 1: baseItemType = BaseItemTypes.basicTower; destinationRectangle = new Rectangle(448, 544, 32, 32); break;
                case 2: baseItemType = BaseItemTypes.waterTower; destinationRectangle = new Rectangle(448, 576, 32, 32); break;
                case 3: baseItemType = BaseItemTypes.turrentTower; destinationRectangle = new Rectangle(416, 608, 32, 32); break;
                case 4: baseItemType = BaseItemTypes.sniperTower; destinationRectangle = new Rectangle(384, 608, 32, 32); break;
                case 5: baseItemType = BaseItemTypes.upgrade1; destinationRectangle = new Rectangle(352, 608, 32, 32); break;
                case 6: baseItemType = BaseItemTypes.upgrade2; destinationRectangle = new Rectangle(320, 608, 32, 32); break;
            }
            base.type = TileType.baseTile;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (baseItemType == BaseItemTypes.electricTower) sb.Draw(ResourceManager.electricTower, destinationRectangle, isSelected? Color.Green: Color.White);
            else if (baseItemType == BaseItemTypes.basicTower) sb.Draw(ResourceManager.exampleTower, destinationRectangle, isSelected? Color.Green: Color.White);
            else if (baseItemType == BaseItemTypes.waterTower) sb.Draw(ResourceManager.exampleTower, destinationRectangle, isSelected? Color.Green: Color.White);
            else if (baseItemType == BaseItemTypes.turrentTower) sb.Draw(ResourceManager.turretTower, destinationRectangle, isSelected? Color.Green: Color.White);
            else if (baseItemType == BaseItemTypes.sniperTower) sb.Draw(ResourceManager.sniperTower, destinationRectangle, isSelected? Color.Green: Color.White);
            else if (baseItemType == BaseItemTypes.upgrade1) sb.Draw(ResourceManager.upgradeTexture, destinationRectangle, isSelected? Color.Green: Color.White);
            else if (baseItemType == BaseItemTypes.upgrade2) sb.Draw(ResourceManager.upgradeTexture, destinationRectangle, isSelected? Color.Green: Color.White);
        }
    }
}
