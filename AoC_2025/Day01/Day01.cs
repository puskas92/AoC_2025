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
    public static class Day01
    {
        public class Day01_Input : List<string> //Define input type
        {
        }
        public static void Day01_Main()
        {
            var input = Day01_ReadInput();
            Console.WriteLine($"Day01 Part1: {Day01_Part1(input)}");
            Console.WriteLine($"Day01 Part2: {Day01_Part2(input)}");
        }

        public static Day01_Input Day01_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day01\\Day01_input.txt").ReadToEnd();
            }

            var result = new Day01_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day01_Part1(Day01_Input input)
        {

            return 0;
        }

        public static int Day01_Part2(Day01_Input input)
        {
            return 0;
        }


    }
    public class Day01_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day01Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day01.Day01_Part1(Day01.Day01_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day01Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day01.Day01_Part2(Day01.Day01_ReadInput(rawinput)));
        }
    }
}
