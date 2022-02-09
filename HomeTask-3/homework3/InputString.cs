using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework3
{
    public delegate void AddStringDelegate(string str);
    internal class InputString
    {
        public AddStringDelegate Addition;

        public void Input()
        {
            Console.Write("Input string here: ");
            Addition(Console.ReadLine());
        }
    }
}
