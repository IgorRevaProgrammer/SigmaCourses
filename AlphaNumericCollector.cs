using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask_3
{
    internal class AlphaNumericCollector
    {
        private MyOwnList<string> stringsWithNums;
        public AlphaNumericCollector()
        {
            stringsWithNums=new MyOwnList<string>();
        }
        public void AddStringWithNums(string str)
        {
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9')
                {
                    stringsWithNums.AddToTail(str);
                    break;
                }
            }
        }
        public MyOwnList<string> GetStringsWithNums() => stringsWithNums;
    }
}
