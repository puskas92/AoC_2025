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
            //return Part1_BF_Regex(input);
            return Optimized(input, false);


        }

        private static long Optimized(Day02_Input input,  bool isPart2)
        {
            // Optimized version without regex
            long sum = 0;

            // Make new range list with only ranges that are same digit long
            var filteredRanges = new Day02_Input();
            foreach (var (start, end) in input)
            {
                int startLen = start.ToString().Length;
                int endLen = end.ToString().Length;
                if (startLen == endLen)
                {
                    filteredRanges.Add((start, end));
                }
                else
                {
                    long rangeStart = start;
                    for (int len = startLen; len < endLen; len++)
                    {
                        long rangeEnd = (long)(Math.Pow(10, len) - 1);
                        filteredRanges.Add((rangeStart, rangeEnd));
                        rangeStart = rangeEnd + 1;
                    }
                    filteredRanges.Add((rangeStart, end));
                }
            }

            //iterate over filtered ranges
            foreach (var (start, end) in filteredRanges)
            {
                var numberSet = new HashSet<long>();
                //iterate through possible digit repeat patterns
                int digitCount = start.ToString().Length; 

                int upperLimit = isPart2 ? digitCount : 2;// for part 1 we only need to check pairs

                for (int i = 2; i <= upperLimit; i++)
                {
                    if (digitCount % i != 0) continue; //skip if digit count not divisible by i

                    //get fix start for the pattern
                    long patternStart = 0;
                    long patternEnd = 0;
                    string patternStartString = "";
                    string patternEndString = "";
                    for (int j = 0; j <= (digitCount / i)-1; j++)
                    {
  
                        if (start.ToString()[j] == end.ToString()[j])
                        {
                            patternStartString += start.ToString()[j];
                            patternEndString += start.ToString()[j];
                        }
                        else
                        {
                            patternStartString += '0';
                            patternEndString += '9';
                        }                       
                    }
                    patternStart = long.Parse(patternStartString);
                    patternEnd = long.Parse(patternEndString);

                    for (long k = patternStart; k <= patternEnd; k++)
                    {
                        long num = 0;
                        //construct candidate number by repeating i times the digit k
                        string numString = string.Concat(Enumerable.Repeat(k.ToString(), i));
                        num = long.Parse(numString);

                        if (num >= start && num <= end)
                        {
                            numberSet.Add(num);
                            //Console.WriteLine(num);
                        }
                    }
                }

                sum += numberSet.Sum();
            }

            return sum;
        }

        private static long Part1_BF_Regex(Day02_Input input)
        {
            long sum = 0;
            foreach (var (start, end) in input)
            {
                for (long i = start; i <= end; i++)
                {
                    if (Regex.IsMatch(i.ToString(), @"^(\d+)\1$")) sum += i;
                }
            }

            return sum;
        }

        public static long Day02_Part2(Day02_Input input)
        {
            //return Part2_BF_Regex(input);
            return Optimized(input, true);
        }

        private static long Part2_BF_Regex(Day02_Input input)
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
