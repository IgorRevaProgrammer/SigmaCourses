using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Human//test class
    {
        #region Properties
        public string First_name { get; }
        public string Last_name { get; }
        public int Old { get; }
        #endregion
        #region constructor
        public Human(string fname, string lname, int old)
        {
            First_name = fname;
            Last_name = lname;
            Old = old;            
        }
        #endregion 
    }
}
