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
    public static class Day04
    {
        public class Day04_Input : Dictionary<int, Dictionary<int,bool>> //Define input type
        {
        }
        public static void Day04_Main()
        {
            var input = Day04_ReadInput();
            Console.WriteLine($"Day04 Part1: {Day04_Part1(input)}");
            Console.WriteLine($"Day04 Part2: {Day04_Part2(input)}");
        }

        public static Day04_Input Day04_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day04\\Day04_input.txt").ReadToEnd();
            }

            var result = new Day04_Input();

            var i = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(i, new Dictionary<int,bool>());
                foreach (var j in Enumerable.Range(0, line.Length))
                {
                    result[i].Add(j, line[j] == '@');
                }
                i += 1;
            }
           

            return result;
        }


        public static int Day04_Part1(Day04_Input input)
        {
            return CalculateRemovableRolls(input).Count;

        }

        private static List<(int,int)> CalculateRemovableRolls(Day04_Input input)
        {
            var result = new List<(int,int)>();

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[i].Count; j++)
                {
                    if (input[i][j])
                    {
                        var neightborsOn = 0;
                        for (int di = -1; di <= 1; di++)
                        {
                            for (int dj = -1; dj <= 1; dj++)
                            {
                                if (di == 0 && dj == 0)
                                {
                                    continue;
                                }
                                var ni = i + di;
                                var nj = j + dj;
                                if (ni >= 0 && ni < input.Count && nj >= 0 && nj < input[i].Count && input[ni][nj])
                                {
                                    neightborsOn += 1;
                                }
                            }
                        }

                        if (neightborsOn < 4) result.Add((i,j));
                    }
                }
            }

            return result;
        }

        public static int Day04_Part2(Day04_Input input)
        {
            int totalRemoved = 0;
            int removeNum;
            do
            {
                var removableRolls = CalculateRemovableRolls(input);
                removeNum = removableRolls.Count;
                foreach (var (i, j) in removableRolls)
                {
                    input[i][j] = false;
                }
                totalRemoved += removeNum;
            } while (removeNum > 0);

            return totalRemoved;
        }


    }
    public class Day04_Test
    {
        [Theory]
        [InlineData("..@@.@@@@.\r\n@@@.@.@.@@\r\n@@@@@.@.@@\r\n@.@@@@..@.\r\n@@.@@@@.@@\r\n.@@@@@@@.@\r\n.@.@.@.@@@\r\n@.@@@.@@@@\r\n.@@@@@@@@.\r\n@.@.@@@.@.", 13)]
        public static void Day04Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day04.Day04_Part1(Day04.Day04_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("..@@.@@@@.\r\n@@@.@.@.@@\r\n@@@@@.@.@@\r\n@.@@@@..@.\r\n@@.@@@@.@@\r\n.@@@@@@@.@\r\n.@.@.@.@@@\r\n@.@@@.@@@@\r\n.@@@@@@@@.\r\n@.@.@@@.@.", 43)]
        public static void Day04Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day04.Day04_Part2(Day04.Day04_ReadInput(rawinput)));
        }
    }
}
