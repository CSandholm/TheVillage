using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Village_Test
{
    public class Worker
    {
        string name;
        string workString;
        int profession;
        bool isHungry;
        int daysHungry;
        bool isAlive;
        public delegate void Work();
        public Work work;

        public Worker(int profession)
        {
            work = () => { };
            isAlive = true;
            name = NameGenerator();
            this.profession = profession;
            isHungry = false;
            daysHungry = 0;
            workString = "";
		}

        //Properties
        public string Name { get { return name; } set { name = value; } }
        public bool IsHungry 
        { 
            get { return isHungry; }
            set { isHungry = value; }
        }
        public int DaysHungry 
        {
            get { return daysHungry; }
            set { daysHungry = value; }
        }
        public bool IsAlive 
        { 
            get { return isAlive; } 
            set { isAlive = value; } 
        }
        public int Profession { get { return profession; } }

        public string WorkString { get; set; }

        string NameGenerator()
        {
            //Returns a full name randomized from the string arrays.
            string[] names = { "Karl", "Franz", "Ikit", "Nasty", "Bobby", "Glen", "Olaf", "Paul", "Gurdge", "Giggidy" };
            string[] surNames = { "Franzson", "Franzdottir", "Paulson", "Olofson", "Hermanson","Tingelin", "Rudolfson", 
                    "Goransson", "Nastyson", "Glendottir"};

            Random rnd = new Random();
            Random rnd2 = new Random();

            return names[rnd.Next(names.Length)] +" "+ surNames[rnd2.Next(surNames.Length)];
        }
    }
}
