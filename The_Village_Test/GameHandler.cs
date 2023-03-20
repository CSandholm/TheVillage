using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Village_Test
{
    public class GameHandler
    {
        public Village Start()
        {
            //Set start values for the village.

            Village village = new Village();

            //Start with 10 food, 0 metal and 0 wood. 
            village.Food = 10;
            village.Metal = 0;
            village.Wood = 0;

            //Start with 3 houses.
            for (int i = 0; i < 3; i++)
            {
                village.AddBuildning(0);
            }

            return village;
        }

        public Village NextDay(Village village)
        {
            village.Day();

            //If the castle is built then the player won and the game will end.
            //If there's no food and no workers then the player failed and game over.
            if (village.HasCastle)
            {
                End(village);
            }
            if(village.Food <= 0 && village.Population <= 0)
            {
                GameOver();
            }

            return village;
        }
        public void GameOver()
        {
            Console.WriteLine("************************** Game Over **************************");
            Console.WriteLine("****************** Your village was destroyed *****************");
            Console.WriteLine("***************************************************************");
            Console.WriteLine("Score: 0");
            Console.WriteLine("Press Enter to exit.");
            System.Environment.Exit(1);
        }
        public void End(Village village)
        {
            Console.WriteLine("************************** VICTORY **************************");
            Console.WriteLine("***************** Your village build a castle ***************");
            Console.WriteLine("*************************************************************");
            Console.WriteLine("Days: " + village.DayCount);
            Console.WriteLine("Score: " + Score(village));
            Console.WriteLine("Press Enter to exit.");
            System.Environment.Exit(1);
        }
        public int Score(Village village)
        {
            int deathPenalty = village.DeathCount * 25;
            int buildingScore = village.GetBuildingCount() * 10;
            int workerScore = village.GetWorkerCount() * 10;
            int score = buildingScore + workerScore - deathPenalty;

            return score;
        }
    }
}
