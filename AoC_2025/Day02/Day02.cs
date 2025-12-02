using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AoC_2025
{
    public static class Day02
    {
        public class Day02_Input : List<(long, long)> //Define input type
        {
        }
        public static void Day02_Main()
        {
            var input = Day02_ReadInput();
            Console.WriteLine($"Day02 Part1: {Day02_Part1(input)}");
            Console.WriteLine($"Day02 Part2: {Day02_Part2(input)}");
        }

        public static Day02_Input Day02_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day02\\Day02_input.txt").ReadToEnd();
            }

            var result = new Day02_Input();

            rawinput.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(line =>
            {
                var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
                result.Add((long.Parse(parts[0]), long.Parse(parts[1])));
            });

            return result;
        }


        public static long Day02_Part1(Day02_Input input)
        {
            long sum = 0;
            foreach (var (start, end) in input)
            {
                for(long i = start; i <= end; i++)
                {
                    if (Regex.IsMatch(i.ToString(), @"^(\d+)\1$")) sum += i;
                }
            }

            return sum;
        }
        
        public static long Day02_Part2(Day02_Input input)
        {
            long sum = 0;
            foreach (var (start, end) in input)
            {
                for (long i = start; i <= end; i++)
                {
                    if (Regex.IsMatch(i.ToString(), @"^(\d+)\1+$")) sum += i;
                }
            }

            return sum;
        }


    }
    public class Day02_Test
    {
        [Theory]
        [InlineData("11 -22,95-115,998-1012,1188511880-1188511890,222220-222224,\r\n1698522-1698528,446443-446449,38593856-38593862,565653-565659,\r\n824824821-824824827,2121212118-2121212124", 1227775554)]
        public static void Day02Part1Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day02.Day02_Part1(Day02.Day02_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("11 -22,95-115,998-1012,1188511880-1188511890,222220-222224,\r\n1698522-1698528,446443-446449,38593856-38593862,565653-565659,\r\n824824821-824824827,2121212118-2121212124", 4174379265)]
        public static void Day02Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day02.Day02_Part2(Day02.Day02_ReadInput(rawinput)));
        }
    }
}
