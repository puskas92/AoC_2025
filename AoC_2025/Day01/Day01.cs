using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
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
            var result = Day01_Part12(input);
            Console.WriteLine($"Day01 Part1: {result.Item1}");
            Console.WriteLine($"Day01 Part2: {result.Item2}");
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
                result.Add(line);
            }

            return result;
        }


        public static (int, int) Day01_Part12(Day01_Input input)
        {
            var result1 = 0;
            var result2 = 0;

            var lockstate = 50;
            var endedOnZero = false;
            foreach (var line in input)
            {
                var turn = line[0];
                var dist = int.Parse(line[1..]);
                if (turn == 'L')
                {
                    lockstate +=dist;
                }
                else if (turn == 'R')
                {
                    lockstate -= dist;
                }
                int div100 = Math.Abs(lockstate / 100);
                if (endedOnZero)
                {

                    result2 += (lockstate < 0 ? div100 : div100);
                }
                else
                {
                    result2 += (lockstate < 0 ? div100 + 1 : div100);
                }

                var newlockstate = ((lockstate % 100 ) + 100 ) % 100;
                if ((newlockstate == lockstate) & (lockstate == 0))
                {
                    result2 += 1;
                }

                lockstate = newlockstate;
                if (lockstate == 0)
                {
                    endedOnZero = true;
                    result1++;
                }
                else
                {
                    endedOnZero = false;
                }
            }
            
            return (result1,result2); 
        }


    }
    public class Day01_Test
    {
        [Theory]
        [InlineData("L68\r\nL30\r\nR48\r\nL5\r\nR60\r\nL55\r\nL1\r\nL99\r\nR14\r\nL82", 3)]
        public static void Day01Part1Test(string rawinput, int expectedValue)
        {
            var result = Day01.Day01_Part12(Day01.Day01_ReadInput(rawinput));
            Assert.Equal(expectedValue, result.Item1);
        }

        [Theory]
        [InlineData("L68\r\nL30\r\nR48\r\nL5\r\nR60\r\nL55\r\nL1\r\nL99\r\nR14\r\nL82", 6)]
        [InlineData("R1000", 10)]
        [InlineData("L50\r\nR1\r\nL1\r\nR1\r\nL1\r\nR1\r\nL1", 4)]
        [InlineData("L50\r\nR1\r\nL101\r\nR1\r\nL201", 6)]
        [InlineData("L50\r\nL1\r\nR101\r\nL1\r\nR201", 6)]
        [InlineData("L50\r\nL100\r\nR100\r\nR200\r\nL200", 7)]
        [InlineData("R50\r\nR100\r\nL100\r\nL200\r\nR200", 7)]
        [InlineData("L1\r\nR1\r\nL50\r\nL1\r\nR2", 2)]
        [InlineData("R1\r\nL1\r\nR50\r\nR1\r\nL2", 2)]
        [InlineData("L50\r\nL101\r\nR102", 4)]  //<this is it
        [InlineData("R50\r\nR101\r\nL102", 4)]
        public static void Day01Part2Test(string rawinput, int expectedValue)
        {
            var result = Day01.Day01_Part12(Day01.Day01_ReadInput(rawinput));
            Assert.Equal(expectedValue, result.Item2);
        }
    }
}
