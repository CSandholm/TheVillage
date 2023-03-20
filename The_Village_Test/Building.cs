using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Village_Test
{
    public class Building
    {
        string name;
        int metalCost;
        int woodCost;
        int daysToBuild;
        int daysSpentBuilding;
        bool isBuilt;
        int type;

        public Building(int type)
        {
            daysSpentBuilding = 0;
            isBuilt = false;
            this.type = type;

            //Instead of having a class for each building the type is decided in the constructor. Bad practice
            //but good enough for this project.
            switch (type)
            {
                case 0:
                    name = "House";
                    metalCost = 1;
                    woodCost = 5;
                    daysToBuild = 3;
                    break;
                case 1:
                    name = "Woodmill";
                    metalCost = 1;
                    woodCost = 5;
                    daysToBuild = 3;
                    break;
                case 2:
                    name = "Quarry";
                    metalCost = 5;
                    woodCost = 3;
                    daysToBuild = 7;
                    break;
                case 3:
                    name = "Farm";
                    metalCost = 2;
                    woodCost = 5;
                    daysToBuild = 5;
                    break;
                case 4:
                    name = "Castle";
                    metalCost = 50;
                    woodCost = 50;
                    daysToBuild = 50;
                    break;
            }
        }
        //Properties
        public string Name { get { return name; } }
        public int MetalCost { get { return metalCost; } }
        public int WoodCost { get { return woodCost; } }
        public int DaysToBuild { get { return daysToBuild; } }  
        public bool IsBuilt 
        { 
            get { return isBuilt; }
            set { isBuilt = value; }
        }
        public int DaysSpentBuilding 
        { 
            get { return daysSpentBuilding; }
            set { daysSpentBuilding = value; }
        }
        public int Type { get { return type; } }
    }
}