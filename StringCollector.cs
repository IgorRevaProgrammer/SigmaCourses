using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask_3
{
    internal class StringCollector
    {
        private MyOwnList<string> strings;
        public StringCollector()
        {
            strings=new MyOwnList<string>();
        }
        public void AddString(string str)
        {
            bool flag = true;
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9')
                {
                    flag = false;
                    break;
                }
            }
            if (flag) strings.AddToTail(str);
        }
        public MyOwnList<string> GetStrings() => strings;
    }
}
