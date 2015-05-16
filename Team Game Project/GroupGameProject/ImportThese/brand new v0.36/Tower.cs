using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    //The super class of all towers, needs sub classes.
    public class Tower : IGameObject
    {
        public enum TowerType
        {
            electricTower,
            basicTower,
            waterTower,
            turretTower,
            sniperTower
        }

        public TowerType towerType;
        public int towerCost, fireRadius;
        Vector2 center;
        Rectangle destinationRectangle;
        int Width = 32, Height = 32;
        TimeSpan fireDelay = new TimeSpan(0, 0, 1);
        DateTime lastFireTime = DateTime.Now;

        public Tower(BaseItem.BaseItemTypes itemType, Vector2 c)
        {
            center = c;
            if (itemType == BaseItem.BaseItemTypes.basicTower) { towerType = TowerType.basicTower; towerCost = 100; fireRadius = 300; }
            else if (itemType == BaseItem.BaseItemTypes.electricTower) { towerType = TowerType.electricTower; towerCost = 500; fireRadius = 150; }
            else if (itemType == BaseItem.BaseItemTypes.waterTower) { towerType = TowerType.waterTower; towerCost = 200; fireRadius = 200; }
            else if (itemType == BaseItem.BaseItemTypes.turrentTower) { towerType = TowerType.turretTower; towerCost = 300; fireRadius = 500; }
            else if (itemType == BaseItem.BaseItemTypes.sniperTower) { towerType = TowerType.sniperTower; towerCost = 200; fireRadius = 1000; }
            destinationRectangle = new Rectangle((int) center.X - Width / 2,(int) center.Y - Height / 2, Width, Height);
        }

        public void MoveTo(int x, int y)
        {
            center.X = x;
            center.Y = y;
            destinationRectangle = new Rectangle((int)center.X - Width / 2, (int)center.Y - Height / 2, Width, Height);
        }

        public void Update(Game1 game)
        {
            foreach (Mob m in game.mobs)
            {
                if (m.isAlive && DateTime.Now - lastFireTime >= fireDelay && Math.Abs((m.center - center).Length()) <= fireRadius)
                {
                    lastFireTime = DateTime.Now;
                    TowerBullet bullet;

                    if (towerType == TowerType.turretTower)
                    {
                        bullet = new LaserBullet(center, m, 5);
                    }
                    else if (towerType == TowerType.electricTower)
                    {
                        bullet = new ElectricBullet(center, new Vector2(2, 2), m, 30);
                    }
                    else
                    {
                        bullet = new TowerBullet(center, new Vector2(2), m, 25);
                    }
                    game.bullets.Add(bullet);
                }
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (towerType == TowerType.basicTower) sb.Draw(ResourceManager.exampleTower, destinationRectangle, Color.White);
            else if (towerType == TowerType.electricTower) sb.Draw(ResourceManager.electricTower, destinationRectangle, Color.White);
            else if (towerType == TowerType.waterTower) sb.Draw(ResourceManager.exampleTower, destinationRectangle, Color.White);
            else if (towerType == TowerType.turretTower) sb.Draw(ResourceManager.turretTower, destinationRectangle, Color.White);
            else if (towerType == TowerType.sniperTower) sb.Draw(ResourceManager.sniperTower, destinationRectangle, Color.White);
        }
    }
}
