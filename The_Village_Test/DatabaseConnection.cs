using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Village_Test
{
    public class DatabaseConnection
    {
        Village currentVillage;
        public virtual bool Load()
        {
            //Connect to database
            //currentVillage contains all the variables, lists etc we'll get from database.
            return true;
        }
        public virtual bool Save(int[] villageData,bool hasCastle,List<Worker> workers, List<Building> buildings,Queue<Building> projects)
        {
            //Connect to database
            //currentVillage save all the incoming variables, lists etc to currentVillage.
            //... Would be nice with neat organized functions, I know!

            currentVillage.Food = villageData[0];
            currentVillage.Metal = villageData[1];
            currentVillage.Wood = villageData[2];
            currentVillage.Population = villageData[3];
            currentVillage.DayCount = villageData[4];
            currentVillage.DeathCount = villageData[5];
            currentVillage.HasCastle = hasCastle;

            //Get and set all worker to list
            if (workers.Count > 0)
            {
                foreach (Worker worker in workers)
                {
                    currentVillage.AddWorker(worker.Profession);
                    currentVillage.GetWorkerList().Last().Name = worker.Name;
                    currentVillage.GetWorkerList().Last().IsHungry = worker.IsHungry;
                    currentVillage.GetWorkerList().Last().DaysHungry = worker.DaysHungry;
                    currentVillage.GetWorkerList().Last().IsAlive = worker.IsAlive;

                    //Will this point towards the village job, or the delegate it self?
                    currentVillage.GetWorkerList().Last().work = worker.work;
                }
            }
            if (buildings.Count > 0)
            {
                //Get all buildnings
                foreach (Building building in buildings)
                {
                    currentVillage.AddBuildning(building.Type);
                }
            }
            if (projects.Count > 0)
            {
                //Get and set projects
                foreach (Building building in projects)
                {
                    currentVillage.AddProject(building.Type);
                    currentVillage.GetProjectQueue().First().DaysSpentBuilding = building.DaysSpentBuilding;
                }
            }
                
            return true;
        }

        //Load
        public virtual int GetFood() { return currentVillage.Food; }
        public virtual int GetWood() { return currentVillage.Wood; }
        public virtual int GetMetal() { return currentVillage.Metal; }
        public virtual int GetHousing() { return currentVillage.Housing; }
        public virtual int GetDayCount() { return currentVillage.DayCount; }
        public virtual int GetDeathCount() { return currentVillage.DeathCount; }
        public virtual int GetPopulation() { return currentVillage.Population; }
        public virtual bool GetHasCastle() { return currentVillage.HasCastle; }
        public virtual List<Worker> GetWorkers() 
        { 
            List<Worker> workers = new List<Worker>();

            foreach(Worker worker in currentVillage.GetWorkerList())
            {
                workers.Add(worker);
            }

            return workers;
        }
        public virtual List<Building> GetBuildings() 
        { 
            List<Building> buildings = new List<Building>();

            foreach(Building building in currentVillage.GetBuildingsList())
            {
                buildings.Add(building);
            }

            return buildings;
        }
        public virtual Queue<Building> GetProjects() 
        { 
            Queue<Building> queue = new Queue<Building>();

            foreach(Building building in currentVillage.GetProjectQueue())
            {
                queue.Enqueue(building);
            }
            return queue;
        }
    }
}
