using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day12
    {
        public class Day12_Input : List<string> //Define input type
        {
        }
        public static void Day12_Main()
        {
            var input = Day12_ReadInput();
            Console.WriteLine($"Day12 Part1: {Day12_Part1(input)}");
            Console.WriteLine($"Day12 Part2: {Day12_Part2(input)}");
        }

        public static Day12_Input Day12_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day12\\Day12_input.txt").ReadToEnd();
            }

            var result = new Day12_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day12_Part1(Day12_Input input)
        {

            return 0;
        }

        public static int Day12_Part2(Day12_Input input)
        {
            return 0;
        }


    }
    public class Day12_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day12Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day12.Day12_Part1(Day12.Day12_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day12Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day12.Day12_Part2(Day12.Day12_ReadInput(rawinput)));
        }
    }
}
