using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4
{
    internal class Player
    {
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public int Age()
        {
            int age = DateTime.Now.Year - Birth.Year;
            if (Birth.Month > DateTime.Now.Month) age--;
            else if (Birth.Month == DateTime.Now.Month
                && Birth.Day > DateTime.Now.Day) age--;
            return age;
        }
    }
}
