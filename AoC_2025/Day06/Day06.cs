using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2025
{
    public static class Day06
    {
        public class Day06_Input //Define input type
        {
            public List<Day06_Column> part1Input = new List<Day06_Column>();
            public List<Day06_Column> part2Input = new List<Day06_Column>();
        }

        public class Day06_Column : List<int>
        {
            public string operationSymbol = "";
        }
        public static void Day06_Main()
        {
            var input = Day06_ReadInput();
            Console.WriteLine($"Day06 Part1: {Day06_Part1(input)}");
            Console.WriteLine($"Day06 Part2: {Day06_Part2(input)}");
        }

        public static Day06_Input Day06_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day06\\Day06_input.txt").ReadToEnd();
            }

            var result = new Day06_Input();

            var rawrow = rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in rawrow.Select(s => s.Trim()))
            {
                var parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < parts.Length; i++)
                {
                    if (result.part1Input.Count <= i)
                    {
                        result.part1Input.Add(new Day06_Column());
                    }

                    var part = parts[i];
                    
                    if (part == "*" || part == "+")
                    {
                        result.part1Input[i].operationSymbol = part ;
                    }
                    else
                    {
                     
                        result.part1Input[i].Add(int.Parse(part));
                    }
                }
            }

            var col = new Day06_Column();
            for (var columnIndex = 0; columnIndex < rawrow[0].Length; columnIndex++)
            {
                var columnString = rawrow.Aggregate("", (current, row) => current + " " + row[columnIndex]);

                columnString = columnString.Replace(" ", "");

                if (columnString == "")
                {
                    result.part2Input.Add(col);
                    col = new Day06_Column();
                }
                else
                {
                    if (columnString.Contains('*'))
                    {
                        col.operationSymbol = "*";
                        columnString = columnString.TrimEnd('*');
                    }
                    if (columnString.Contains('+'))
                    {
                        col.operationSymbol = "+";
                        columnString = columnString.TrimEnd('+');
                    }
                   
                    col.Add(int.Parse(columnString));
                }
                
            }
            result.part2Input.Add(col);


            return result;
        }


        public static ulong Day06_Part1(Day06_Input input)
        {
            return Calculate(input.part1Input);

        }

        private static ulong Calculate(List<Day06_Column> input)
        {
            ulong total = 0;

            foreach (var column in input)
            {
                ulong columnResult = (ulong)(column[0]);
                for (int i = 1; i < column.Count; i++)
                {
                    if (column.operationSymbol == "*")
                    {
                        columnResult *= (ulong)column[i];
                    }
                    else if (column.operationSymbol == "+")
                    {
                        columnResult += (ulong)column[i];
                    }
                }
                total += columnResult;
            }


            return total;
        }

        public static ulong Day06_Part2(Day06_Input input)
        {
            return Calculate(input.part2Input);
        }


    }
    public class Day06_Test
    {
        [Theory]
        [InlineData("123 328  51 64 \r\n 45 64  387 23 \r\n  6 98  215 314\r\n*   +   *   +  ", 4277556)]
        public static void Day06Part1Test(string rawinput, ulong expectedValue)
        {
            Assert.Equal(expectedValue, Day06.Day06_Part1(Day06.Day06_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("123 328  51 64 \r\n 45 64  387 23 \r\n  6 98  215 314\r\n*   +   *   +  ", 3263827)]
        public static void Day06Part2Test(string rawinput, ulong expectedValue)
        {
            Assert.Equal(expectedValue, Day06.Day06_Part2(Day06.Day06_ReadInput(rawinput)));
        }
    }
}
