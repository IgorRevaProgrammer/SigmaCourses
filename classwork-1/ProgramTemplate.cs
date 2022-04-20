using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork
{
    class ProgramTemplate
    {
        static void Main(string[] args)
        {
            int[] arr1 = new int[] { 0, 1, 2 };
            int[] arr2 = new int[] { 0, 1, 2 };
            string[] arr3 = (arr1.SelectMany(i => arr2, (k, j) => k + "," + j)).ToArray();
            foreach (int i in Enumerable.Range(0,9))
                Console.WriteLine(arr3[i]);
        }
    }

    class Car
    {
        private Owner owner;
        public Car()
        {
            AllOwners = new List<Owner>();
        }
        public string VIN { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public double StickerPrice { get; set; }

        public Owner Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
                AllOwners.Add(value);
            }
        }

        public List<Owner> AllOwners { get; set; }
    }

    class Owner
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}