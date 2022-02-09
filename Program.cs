using System;

namespace HomeTask_3
{
    internal class Program
    {
        static void OutPut(Func<MyOwnList<string>> PrintFunc)
        {
            foreach (string str in PrintFunc())
            {
                Console.WriteLine(str);
            }
        }
        static void Main(string[] args)
        {
            AlphaNumericCollector alphaNumColl = new AlphaNumericCollector();
            StringCollector strColl = new StringCollector();
            InputString inputString = new InputString();
            inputString.Addition += alphaNumColl.AddStringWithNums;
            inputString.Addition += strColl.AddString;
            Console.Write("Do you want to input string(yes/no): ");
            string yes_no = (string)Console.ReadLine();
            if (yes_no != "yes" && yes_no != "no")
                Console.WriteLine("Error");
            while (yes_no == "yes")
            {
                inputString.Input();
                Console.Write("Do you want to input new string: ");
                yes_no = (string)Console.ReadLine();
                if (yes_no != "yes" && yes_no != "no")
                    Console.WriteLine("Error");
            }
            Console.WriteLine("Strings without numbers:");
            OutPut(strColl.GetStrings);
            Console.WriteLine("Strings with numbers:");
            OutPut(alphaNumColl.GetStringsWithNums);
        }
    }
}
