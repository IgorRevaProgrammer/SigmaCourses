using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Here I make some objects
            Human human1 = new Human("Tom", "Holland", 25);
            Human human2 = new Human("Leonardo", "DiCaprio", 47);
            Human human3 = new Human("Emma", "Watson", 31);
            //create my list and addition obj. to list
            MyOwnList<Human> humans = new MyOwnList<Human>();
            humans.AddToTail(human1);
            humans.AddToTail(human2);
            humans.AddToTail(human3);
            //output first names to console
            foreach(Human human in humans)
            {
                Console.WriteLine(human.First_name);
            }
            //remove 1st element
            humans.Remove(1);
            //and output elements again
            foreach (Human human in humans)
            {
                Console.WriteLine(human.First_name);
            }
            //You can also test other methods...
            Console.ReadLine();
        }
    }
}
