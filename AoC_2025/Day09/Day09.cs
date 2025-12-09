using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day09
    {
        public class Day09_Input : List<Point> //Define input type
        {
        }
        public static void Day09_Main()
        {
            var input = Day09_ReadInput();
            Console.WriteLine($"Day09 Part1: {Day09_Part1(input)}");
            Console.WriteLine($"Day09 Part2: {Day09_Part2(input)}");
        }

        public static Day09_Input Day09_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day09\\Day09_input.txt").ReadToEnd();
            }

            var result = new Day09_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var split = line.Split(',');
                result.Add(new Point(int.Parse(split[0]), int.Parse(split[1])));
            }

            return result;
        }


        public static long Day09_Part1(Day09_Input input)
        {
            long maxArea = 0;
            for(var i = 0; i < input.Count-1; i++)
            {
                for(var j = i+1; j < input.Count; j++)
                {
                    long area = (long)Math.Abs((long)input[i].X - input[j].X +1) * (long)Math.Abs((long)input[i].Y - input[j].Y +1);
                    maxArea = Math.Max(maxArea, area);
                }
            }
            return maxArea;
        }

        public static int Day09_Part2(Day09_Input input)
        {
            return 0;
        }


    }
    public class Day09_Test
    {
        [Theory]
        [InlineData("7,1\r\n11,1\r\n11,7\r\n9,7\r\n9,5\r\n2,5\r\n2,3\r\n7,3", 50)]
        public static void Day09Part1Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part1(Day09.Day09_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day09Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part2(Day09.Day09_ReadInput(rawinput)));
        }
    }
}
