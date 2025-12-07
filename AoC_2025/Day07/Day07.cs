using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day07
    {
        public class Day07_Input 
        {
            public Point start;
            public List<Point> splitters = new List<Point>();
        }
        public static void Day07_Main()
        {
            var input = Day07_ReadInput();
            var result = Day07_Part12(input);
            Console.WriteLine($"Day07 Part1: {result.Item1}");
            Console.WriteLine($"Day07 Part2: {result.Item2}");
        }

        public static Day07_Input Day07_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day07\\Day07_input.txt").ReadToEnd();
            }

            var result = new Day07_Input();

            var lines = rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim());
            for(int y = 0; y < lines.Count(); y++)
            {
                var line = lines.ElementAt(y);
                for(int x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    if(c == 'S')
                    {
                        result.start = new Point(x, y);
                    }
                    else if(c == '^')
                    {
                        result.splitters.Add( new Point(x,  y));
                    }
                }
            }

            return result;
        }


        public static int Day07_Part1(Day07_Input input)
        {
            return Day07_Part12(input).Item1;
        }

        private static (int,long) Day07_Part12(Day07_Input input)
        {
            var visited = new HashSet<Point>();
            long timelineNum = 1;
            var toCheckDictionary = new Dictionary<Point, long>();
          
            var firstSplitter = input.splitters.FindAll(p => p.X == input.start.X && p.Y > input.start.Y)
                .OrderBy(p => p.Y)
                .First();

            toCheckDictionary.Add(firstSplitter, 1);
            visited.Add(firstSplitter);

            while (toCheckDictionary.Count > 0)
            {

                var currentSplitter = toCheckDictionary.OrderBy(f => f.Key.Y).First();
                toCheckDictionary.Remove(currentSplitter.Key);
                timelineNum += currentSplitter.Value;

                for (int i = -1; i <=1; i+=2) // -1 left, +1 right
                {
                    if (input.splitters.Any(p => p.X == currentSplitter.Key.X + i && p.Y > currentSplitter.Key.Y))
                    {
                        var Splitter = input.splitters.FindAll(p => p.X == currentSplitter.Key.X + i && p.Y > currentSplitter.Key.Y)
                        .OrderBy(p => p.Y)
                        .First();

                        visited.Add(Splitter);

                        if (toCheckDictionary.ContainsKey(Splitter))
                        {
                            toCheckDictionary[Splitter] += currentSplitter.Value;
                        }
                        else
                        {
                            toCheckDictionary.Add(Splitter, currentSplitter.Value);
                        }
                    }
                }
               
            }


            return (visited.Count,timelineNum);  //63494160241 too low
        }

        public static long Day07_Part2(Day07_Input input)
        {
            return Day07_Part12(input).Item2;
        }


    }
    public class Day07_Test
    {
        [Theory]
        [InlineData(".......S.......\r\n...............\r\n.......^.......\r\n...............\r\n......^.^......\r\n...............\r\n.....^.^.^.....\r\n...............\r\n....^.^...^....\r\n...............\r\n...^.^...^.^...\r\n...............\r\n..^...^.....^..\r\n...............\r\n.^.^.^.^.^...^.\r\n...............", 21)]
        public static void Day07Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day07.Day07_Part1(Day07.Day07_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData(".......S.......\r\n...............\r\n.......^.......\r\n...............\r\n......^.^......\r\n...............\r\n.....^.^.^.....\r\n...............\r\n....^.^...^....\r\n...............\r\n...^.^...^.^...\r\n...............\r\n..^...^.....^..\r\n...............\r\n.^.^.^.^.^...^.\r\n...............", 40)]
        public static void Day07Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day07.Day07_Part2(Day07.Day07_ReadInput(rawinput)));
        }
    }
}
