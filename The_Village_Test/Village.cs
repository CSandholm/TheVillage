using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Village_Test
{
    public class Village
    {
        int food;
        int wood;
        int metal;
        int housing;
        int dayCount;
        int deathCount;
        int population;
        bool hasCastle;
        List<Worker> workers;
        List<Building> buildings;
        Queue<Building> projects;

        public Village()
        {
            food = 0;
            wood = 0;
            metal = 0;
            dayCount = 0;
            housing = 0;
            deathCount = 0;
            population = 0;
            hasCastle = false;
            workers = new List<Worker>();
            buildings = new List<Building>();
            projects = new Queue<Building>();
            DatabaseConnection = new DatabaseConnection();
        }

        //Properties
        public int Food { get { return food; } set { food = value; } }
        public int Wood { get { return wood; } set { wood = value; } }
        public int Metal { get { return metal; } set { metal = value; } }
        public int DayCount { get { return dayCount; } set { dayCount = value; } }
        public int DeathCount { get { return deathCount; } set { deathCount = value; } }
        public int Population { get { return population; } set { population = value; } }
        public bool HasCastle { get { return hasCastle; } set { hasCastle = value; } }
        public DatabaseConnection DatabaseConnection { get; set; }

        public int GetBuildingCount()
        {
            return buildings.Count;
        }
        public int GetWorkerCount()
        {
            return workers.Count;
        }
        public int GetProjectCount()
        {
            return projects.Count;
        }
        public int GetFarmerCount()
        {
            return workers.Count(x => x.Profession == 2);
        }
        public int GetMinerCount()
        {
            return workers.Count(x => x.Profession == 1);
        }
        public int GetLumberjackCount()
        {
            return workers.Count(x => x.Profession == 0);
        }
        public int GetBuilderCount()
        {
            return workers.Count(x => x.Profession == 3);
        }

        public List<Worker> GetWorkerList() { return workers; }
        public List<Building> GetBuildingsList() { return buildings; }
        public Queue<Building> GetProjectQueue() { return projects; }
        public void WriteFirstProjectInQueue()
        {
            if(projects.Count > 0) 
            {
                Console.WriteLine("");
                Console.WriteLine("Project: " + projects.Peek().Name+": " + projects.Peek().DaysSpentBuilding 
                    +"/"+ projects.Peek().DaysToBuild);
            }
        }

        public int Housing 
        { 
            get 
            {
                //Each house in the buildings list provides two housing.
                //Housing depends on houses. 2 housing per house.
                //This was implemented early and never changed though it should've! This is bad practise.
                //This way we can never really set housing in testing, instead houses needs to be added in tests
                //if workers are being added.
                return buildings.Count(x=>x.Name == "House") *2; 
            } 
            set 
            { 
                housing = value; 
            } 
        }

        //Methods
        public bool AddWorker(int profession)
        {
            //Add a worker if housing is available.
            if(population >= Housing)
            {
                return false;
            }
            else
            {
                Worker worker = new Worker(profession);
                switch (profession)
                {
                    case 0:
                        //Lumberjack
                        worker.work = () => AddWood();
                        worker.WorkString = " is gathering wood.";
                        break;
                    case 1:
                        //Miner
                        worker.work = () => AddMetal();
						worker.WorkString = " is gathering metal.";
						break;
                    case 2:
                        //Farmer
                        worker.work = () => AddFood();
						worker.WorkString = " is gathering food.";
						break;
                    case 3:
                        //Builder
                        worker.work = () => Build();
						worker.WorkString = " is building, if able.";
						break;

                }
                workers.Add(worker);
                population++;
            }
            return true;
        }
        public bool AddRandomWorker()
        {
            bool added;
            added = AddWorker(RandomInt());
            return added;
        }
        public virtual int RandomInt()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, 4);
            return num;
        }
        public void AddBuildning(int type)
        {
            //Before buildings are going into the buildings list, they're projects. Once they are actually built
            //that building type is added to the building list and removed from the project queue.

            //Add a building to the building list.
            buildings.Add(new Building(type));
            Console.WriteLine(buildings.Last().Name + " constructed.");

            //If a castle was added hasCastle is true, which will end the game.
            if(type == 4)
            {
                hasCastle = true;
            }
        }

        //Add resources to the village
        void AddWood()
        {
            wood++;
        }
        void AddMetal()
        {
            metal++;
        }
        void AddFood()
        {
            food += 5;
        }
        public void Build()
        {
            //Add one builders day of work to the first project in queue. 
            //If build reach days to build then add building to building list and dequeue project from project queue.
            if (projects.Count > 0)
            {
                projects.Peek().DaysSpentBuilding += 1;
                if (projects.Peek().DaysSpentBuilding >= projects.Peek().DaysToBuild)
                {
                    AddBuildning(projects.Peek().Type);
                    projects.Dequeue();
                }
            }
        }

        public bool AddProject(int type)
        {
            //Add a project to the queue, if there are enough resources.
            Building building = new Building(type);

            if (building.WoodCost > Wood || building.MetalCost > Metal)
            {
                Console.WriteLine("Insufficient funds!");
                return false;
            }
            else
            {
                wood -= building.WoodCost;
                metal -= building.MetalCost;
                projects.Enqueue(building);

                return true;
            }
        }
        public void FeedWorkers()
        {
            //Fun idea: A hierarchy in the village could decide whom gets to eat first. Peasants would run a higher risk
            //of dying of starvation than nobels, which could lead to village collapse, or rebellion.

            //To feed the most hungry workers first we sort the worker list.
            //Note: If food is 0 there might be farmers that arn't hungry and they'll gather food for other workers so that a few might work.
            //Some might be able to work even though food is 0 before and after the turn.
            List<Worker> sortedList = workers.OrderBy(x => x.DaysHungry).ToList();

            //Loop through workers and subtract food. If hungry adjust hunger variables. In worst case, death.
            //If worker is not alive, then don't feed that worker. 
            for (int i = sortedList.Count-1; i >= 0; i--)
            {
                if (!sortedList[i].IsAlive)
                {
                    continue;
                }
                else if (food <= 0)
                {
                    sortedList[i].IsHungry = true;
                    sortedList[i].DaysHungry++;
                    if(sortedList[i].DaysHungry >= 10)
                    {
                        //Worker death.
                        sortedList[i].IsAlive = false;
                        deathCount++;
                        population--;
                    }
                }
                else if (!sortedList[i].IsHungry && food > 0)
                {
                    food--;
                }
                else if(food > 0)
                {
                    food--;
                    sortedList[i].IsHungry = false;
                    sortedList[i].DaysHungry = 0;
                }
            }
            workers = sortedList;
        }
        public void GatherResourcesAndBuild()
        {
            //Loop through workers and buildingtypes and gain resources through workers work.

            //A worker might be working a building. Whom doesn't matter only that someone is working it.
            //Therefore we save the total amount of each buildingtype that gives a resource bonus. 

            int lumbermills = GetBuildingsList().Count(x => x.Name == "Woodmill");
            int quarries = GetBuildingsList().Count(x => x.Name == "Quarry");
            int farms = GetBuildingsList().Count(x => x.Name == "Farm");

            //For each worker. If they arn't hungry they work.
            //If A building is available to work, then the current worker work it and the total amount of available
            //buildings of that type is reduced by one, since a building shouldn't be worked more than once.
            //Only living workers work.
            foreach (Worker worker in workers)
            {
                if (!worker.IsHungry && worker.IsAlive)
                {
                    switch (worker.Profession)
                    {
                        case 0:
                            //Lumberjack
                            worker.work();
                            Console.WriteLine(worker.Name + worker.WorkString);
                            if(lumbermills > 0)
                            {
                                Console.WriteLine(" And working a woodmill.");
                                lumbermills--;
                                wood += 2;
                            }
                            break;
                        case 1:
                            //Miner
                            worker.work();
							Console.WriteLine(worker.Name + worker.WorkString);
							if (quarries > 0)
                            {
								Console.WriteLine(" And working a quarry.");
								quarries--;
                                metal += 2;
                            }
                            break;
                        case 2:
                            //Farmer
                            worker.work();
							Console.WriteLine(worker.Name + worker.WorkString);
							if (farms > 0)
                            {
								Console.WriteLine(" And working a farm.");
								farms--;
                                food += 10;
                            }
                            break;
                        case 3:
							//Builder
							Console.WriteLine(worker.Name + worker.WorkString);
							worker.work();
							break;
                    }
                }
            }
        }  

        public void BuryDead()
        {
            //Bury each dead worker.

            for(int i = workers.Count-1; i >= 0; i--)
            {
                if (!workers[i].IsAlive)
                {
                    Console.WriteLine("RIP: " + workers[i].Name);
                    workers.Remove(workers[i]);
                }
            }
        }
        public void Day()
        {
            //After each turn workers will work, if able. The village will feed the workers, if able.
            //The amount of days spent in game increase by 1
            GatherResourcesAndBuild();
            FeedWorkers();
            DayCount++;
        }
        public bool SaveProgress()
        {
            int[] villageData = { Food, Metal, Wood, Population, DayCount, DeathCount};

            //Save village variables, lists etc to database.
            bool success = DatabaseConnection.Save(villageData, hasCastle, workers, buildings, projects);

            if (success)
            {
                return true;
            }

            return false;
        }
        public string LoadProgress()
        {
            //Load village variables, lists etc from database.
            bool success = DatabaseConnection.Load();
            if (success)
            {
                Wood = DatabaseConnection.GetWood();
                Metal = DatabaseConnection.GetMetal();
                Food = DatabaseConnection.GetFood();
                DayCount = DatabaseConnection.GetDayCount();
                Housing= DatabaseConnection.GetHousing();
                DeathCount = DatabaseConnection.GetDeathCount();
                Population = DatabaseConnection.GetPopulation();
                HasCastle = DatabaseConnection.GetHasCastle();
                workers.Clear();
                foreach(Worker worker in DatabaseConnection.GetWorkers())
                {
                    workers.Add(worker);
                }
                buildings.Clear();
                foreach (Building building in DatabaseConnection.GetBuildings())
                {
                    buildings.Add(building);
                }
                projects.Clear();
                foreach(Building buildning in DatabaseConnection.GetProjects())
                {
                    projects.Enqueue(buildning);
                }
                return VillageToString();
            }
            else
                return "Failed Load!";
        }

        public string VillageToString()
        {
            return "Workers " + Population + ". "
                  + "On day: " + DayCount + ". "
                  + "Food: " + Food + " ."
                  + "Wood: " + Wood + " ."
                  + "Metal: " + Metal + " ."
                  + "Housing: " + Housing + " .";
        }
    }
}
