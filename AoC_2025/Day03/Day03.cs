using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;
using Xunit.Sdk;

namespace AoC_2025
{
    public static class Day03
    {
        public class Day03_Input : List<Day03_BatteryBank> //Define input type
        { 
        }

        public class Day03_BatteryBank : List<byte> //Define custom data structure
        {
            public Day03_BatteryBank(string raw)
            { 
                foreach (char c in raw)
                {
                    this.Add(byte.Parse(c.ToString()));
                }
            }

            //public int GetMaxJoltageFrom2()
            //{
            //    for(byte i = 9; i >= 1; i--)
            //    {
            //        var currentPos = this.IndexOf(i);

            //        if (currentPos == -1) continue;
            //        if (currentPos + 1 >= this.Count) continue;
                   
            //        for(byte j = 9; j >= 1; j--)
            //        {
            //            var nextPos = this.IndexOf(j, currentPos + 1);
            //            if (nextPos == -1) continue;
            //            return (i*10 ) + j;
            //        }

            //    }
            //    return 0;
            //}

            public string GetMaxJoltage(int level = 12, int pos = 0)
            {
                if (level == 0) return "";
                

                for (byte i = 9; i >= 1; i--)
                {
                    var currentPos = this.IndexOf(i,pos);

                    if (currentPos == -1) continue;
                    if (currentPos + (level-1) >= this.Count) continue;

                    return i.ToString() + GetMaxJoltage(level - 1, currentPos + 1);

                }
                return "e";
            }
        }
        public static void Day03_Main()
        {
            var input = Day03_ReadInput();
            Console.WriteLine($"Day03 Part1: {Day03_Part1(input)}");
            Console.WriteLine($"Day03 Part2: {Day03_Part2(input)}");
        }

        public static Day03_Input Day03_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day03\\Day03_input.txt").ReadToEnd();
            }

            var result = new Day03_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(new Day03_BatteryBank(line));
            }

            return result;
        }


        public static int Day03_Part1(Day03_Input input)
        {

            //return input.Sum(f=> f.GetMaxJoltageFrom2());
            return input.Sum(f => int.Parse(f.GetMaxJoltage(2)));
        }

        public static long Day03_Part2(Day03_Input input)
        {
            return input.Sum(f => Int64.Parse(f.GetMaxJoltage(12)));
        }


    }
    public class Day03_Test
    {
        [Theory]
        [InlineData("987654321111111\r\n811111111111119\r\n234234234234278\r\n818181911112111", 357)]
        public static void Day03Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day03.Day03_Part1(Day03.Day03_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("987654321111111\r\n811111111111119\r\n234234234234278\r\n818181911112111", 3121910778619)]
        public static void Day03Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day03.Day03_Part2(Day03.Day03_ReadInput(rawinput)));
        }
    }
}
