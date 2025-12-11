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
    public static class Day11
    {
        public class Day11_Input : List<string> //Define input type
        {
        }
        public static void Day11_Main()
        {
            var input = Day11_ReadInput();
            Console.WriteLine($"Day11 Part1: {Day11_Part1(input)}");
            Console.WriteLine($"Day11 Part2: {Day11_Part2(input)}");
        }

        public static Day11_Input Day11_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day11\\Day11_input.txt").ReadToEnd();
            }

            var result = new Day11_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day11_Part1(Day11_Input input)
        {

            return 0;
        }

        public static int Day11_Part2(Day11_Input input)
        {
            return 0;
        }


    }
    public class Day11_Test
    {
        [Theory]
        [InlineData("aaa: you hhh\r\nyou: bbb ccc\r\nbbb: ddd eee\r\nccc: ddd eee fff\r\nddd: ggg\r\neee: out\r\nfff: out\r\nggg: out\r\nhhh: ccc fff iii\r\niii: out", 5)]
        public static void Day11Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day11.Day11_Part1(Day11.Day11_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("aaa: you hhh\r\nyou: bbb ccc\r\nbbb: ddd eee\r\nccc: ddd eee fff\r\nddd: ggg\r\neee: out\r\nfff: out\r\nggg: out\r\nhhh: ccc fff iii\r\niii: out", 0)]
        public static void Day11Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day11.Day11_Part2(Day11.Day11_ReadInput(rawinput)));
        }
    }
}
