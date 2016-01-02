using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolskaBot.Core.Darkorbit
{
    public class Building
    {
        public int BuildingID { get; set; }
        public string Name { get; set; }
        public Point Position { get; set; }
        public int AssetType { get; set; }

        public Building(int buildingID, string name, int x, int y, int assetType)
        {
            BuildingID = buildingID;
            Name = name;
            Position = new Point(x, y);
            AssetType = assetType;
        }
    }
}
