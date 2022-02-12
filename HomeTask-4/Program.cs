using System;
using System.Collections.Generic;
using System.Linq;

namespace Homework4
{
    internal class Program
    {
        private static string FirstEx(string str)
        {
            string[] SplitedStr = str.Split(", ");
            var ZippedStrings = Enumerable.Range(1, SplitedStr.Length + 1).Zip(SplitedStr, (a, b) => a + ". " + b);
            return string.Join(", ", ZippedStrings);
        }
        private static List<Player> SecondEx(string str)
        {
                var players = str.Split("; ").
                Select(s => s.Split(", ")).
                Select(s => new Player
                {
                    Name = s[0],
                    Birth = DateTime.Parse(s[1])
                }).OrderBy(s => s.Birth);
            
            return players.ToList<Player>();
        }
        private static string ThirdEx(string thirdString)
        {
            string[] TimeStrings = thirdString.Split(",");
            float[] Times = new float[TimeStrings.Length];
            for (int i = 0; i < TimeStrings.Length; i++)
            {
                TimeStrings[i] = TimeStrings[i].Replace(':', ',');
                Times[i] = float.Parse(TimeStrings[i]);
            }
            float sum = Times.Aggregate(0, (float t, float i) => t + i);
            return sum.ToString().Replace(',', ':');
        }
        static void Main(string[] args)
        {
            //First Exercise
            string FirstString = "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis, Schneiderlin, Cork, Lallana, Rodriguez, Lambert";
            Console.WriteLine("-----------1-----------");
            Console.WriteLine("String from first ex: " + FirstString);
            Console.WriteLine("Converted string: " + FirstEx(FirstString));
            Console.WriteLine("-----------2-----------");
            //Second Exercise
            string SecondString = $"Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988";
            Console.WriteLine("String from second ex: " + SecondString);
            foreach (Player player in SecondEx(SecondString))
                Console.WriteLine("Name: " + player.Name + "\tBirthday: " + player.Birth.ToShortDateString() + "\tAge: " + player.Age());
            Console.WriteLine("-----------3-----------");
            //Third Exercise
            string ThirdString = "4:12,2:43,3:51,4:29,3:24,3:14,4:46,3:25,4:52,3:27";
            Console.WriteLine("String from third ex: " + ThirdString);
            Console.WriteLine("Total time: " + ThirdEx(ThirdString));
            Console.WriteLine("----------end----------");
        }
    }
}
