namespace The_Village_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameHandler gameHandler = new GameHandler();
            Village village = gameHandler.Start();
            bool notOver = true;

            while (notOver)
            {
                notOver = MainMenu();
            }

            //Code below handles Menu, Options and User input.
            bool MainMenu()
            {
                Console.Clear();

                ShowVillageData();

                Console.WriteLine("");
                Console.WriteLine("********************** Make your choice *********************");
                Console.WriteLine("*************************************************************");

                OptionMenu();

                //OptionMenu input handler
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        OptionMenuWorker();
                        OptionMenuWorkerInput();
                        return true;
                    case "2":
                        OptionMenuProject();
                        OptionMenuProjectInput();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        return true;
                    case "3":
                        gameHandler.NextDay(village);
                        Console.WriteLine("Press Enter to continue.");
                        Console.Read();
                        return true;
                    case "9":
                        Console.Clear();
                        village.BuryDead();
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        return true;
                    case "e":
                        return false;
                    default:
                        return true;
                }
            }

            void ShowVillageData()
            {
                Console.Clear();
                Console.WriteLine("******************** The Testing Village ********************");
                Console.WriteLine("Food: " + village.Food);
                Console.WriteLine("Metal: " + village.Metal);
                Console.WriteLine("Wood: " + village.Wood);
                Console.WriteLine("");
                Console.WriteLine("Lumberjacks: " + village.GetLumberjackCount());
                Console.WriteLine("Miners: " + village.GetMinerCount());
                Console.WriteLine("Farmers: " + village.GetFarmerCount());
                Console.WriteLine("Builders: " + village.GetBuilderCount());
                Console.WriteLine("");
                Console.WriteLine("Population: " + village.Population + "/" + village.Housing);
                Console.WriteLine("Day: " + village.DayCount);
                Console.WriteLine("");
                Console.WriteLine("Per Turn: Wood: " 
                    + (village.GetLumberjackCount() + village.GetBuildingsList().Count(x => x.Name == "Woodmill") * 2)
                    + " Metal: " 
                    + (village.GetMinerCount() + (village.GetBuildingsList().Count(x => x.Name == "Quarry") * 2)
                    + " Food: " 
                    + ((village.GetFarmerCount()*5 -village.Population) + (village.GetBuildingsList().Count(x => x.Name == "Farm") * 10)
                    )));
                village.WriteFirstProjectInQueue();
            }
            void OptionMenu()
            {
                Console.WriteLine("Options: ");
                Console.WriteLine("1: Add Worker.");
                Console.WriteLine("2: Add Project.");
                Console.WriteLine("3: Next Turn.");
                //Console.WriteLine("7: Load");
                //Console.WriteLine("8: Save");
                Console.WriteLine("9: Bury the dead.");
                Console.WriteLine("e: Exit Game.");
            }
            void OptionMenuWorker()
            {
                Console.WriteLine("What type of worker do you want?");
                Console.WriteLine("1. Lumberjack.");
                Console.WriteLine("2. Miner.");
                Console.WriteLine("3. Farmer.");
                Console.WriteLine("4. Builder.");
                Console.WriteLine("5. Random Worker.");
                Console.WriteLine("6. Cancel.");
            }
            void OptionMenuProject()
            {
                Console.WriteLine("What type of building do you want?");
                Console.WriteLine("1. House.");
                Console.WriteLine("2. Woodmill.");
                Console.WriteLine("3. Quarry.");
                Console.WriteLine("4. Farm.");
                Console.WriteLine("5. Castle.");
                Console.WriteLine("6. Cancel.");
            }
            void OptionMenuWorkerInput()
            {
                try
                {
                    int input = Convert.ToInt32(Console.ReadLine());
                    input--;
                    if (input < 0 || input > 4)
                    {
                        Console.WriteLine("Invalid input!");
                    }
                    else
                    {
                        if (village.Population >= village.Housing)
                        {
                            Console.Clear();
                            Console.WriteLine("Build more houses!");
                            Console.WriteLine("Press Enter to continue");
                            Console.ReadLine();
                        }
                        else if(input == 4)
                        {
                            village.AddRandomWorker();
                        }
                        else
                        {
                            village.AddWorker(input);
                        }
                    }
                }
                catch 
                {
                    Console.WriteLine("Invalid input!");
                }
            }
            void OptionMenuProjectInput()
            {
                try
                {
                    int input = Convert.ToInt32(Console.ReadLine());
                    input--;
                    village.AddProject(input);
                }
                catch
                {
                    Console.WriteLine("Invalid input or insufficient funds!");
                }
            }
        }
    }
}