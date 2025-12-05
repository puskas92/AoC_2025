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
    public static class Day05
    {
        public class Day05_Input //Define input type
        {
            public List<(long, long)> freshList;
            public List<long> ingredients;

            public Day05_Input(string rawinput)
            {
                freshList = new List<(long, long)>();
                ingredients = new List<long>();

                foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
                {
                    if (line == "") continue;
                    if (line.Contains("-"))
                        freshList.Add((Int64.Parse(line.Split("-")[0]), Int64.Parse(line.Split("-")[1])));
                    else
                        ingredients.Add(Int64.Parse(line));
                }
            }
        }
        public static void Day05_Main()
        {
            var input = Day05_ReadInput();
            Console.WriteLine($"Day05 Part1: {Day05_Part1(input)}");
            Console.WriteLine($"Day05 Part2: {Day05_Part2(input)}");
        }

        public static Day05_Input Day05_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day05\\Day05_input.txt").ReadToEnd();
            }

            var result = new Day05_Input(rawinput);

            return result;
        }


        public static int Day05_Part1(Day05_Input input)
        {
            var freshIngredients = 0;
            foreach (var ingredient in input.ingredients)
            {
                for(int i =0; i < input.freshList.Count; i++)
                {
                    var (start, end) = input.freshList[i];
                    if(ingredient >= start && ingredient <= end)
                    {
                        freshIngredients += 1;
                        break;
                    }
                }
            }
            return freshIngredients;

        }

        public static long Day05_Part2(Day05_Input input)
        {
            var reducedList = new List<(long, long)>();

            foreach(var (start, end) in input.freshList.OrderBy(t => t.Item1))
            {
                if(reducedList.Count == 0)
                {
                    reducedList.Add((start, end));
                }
                else
                {
                    var (lastStart, lastEnd) = reducedList[reducedList.Count - 1];
                    if(start <= lastEnd + 1)
                    {
                        reducedList[reducedList.Count - 1] = (lastStart, Math.Max(lastEnd, end));
                    }
                    else
                    {
                        reducedList.Add((start, end));
                    }
                }
            }

            return reducedList.Sum(t=> t.Item2 - t.Item1 + 1);

        }


    }
    public class Day05_Test
    {
        [Theory]
        [InlineData("3-5\r\n10-14\r\n16-20\r\n12-18\r\n\r\n1\r\n5\r\n8\r\n11\r\n17\r\n32", 3)]
        public static void Day05Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day05.Day05_Part1(Day05.Day05_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("3-5\r\n10-14\r\n16-20\r\n12-18\r\n\r\n1\r\n5\r\n8\r\n11\r\n17\r\n32", 14)]
        public static void Day05Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day05.Day05_Part2(Day05.Day05_ReadInput(rawinput)));
        }
    }
}
