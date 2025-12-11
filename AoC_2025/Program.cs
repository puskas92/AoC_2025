global using Xunit;
using AoC_2025;
using System.Diagnostics;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("AoC 2025");

var sw = new Stopwatch();
sw.Start();
//Day01.Day01_Main();
//Day02.Day02_Main();
//Day03.Day03_Main();
//Day04.Day04_Main();
//Day05.Day05_Main();
//Day06.Day06_Main();
//Day07.Day07_Main();
//Day08.Day08_Main();
//Day09.Day09_Main();
Day10.Day10_Main();
Day11.Day11_Main();
sw.Stop();

Console.WriteLine($"Code run under {sw.ElapsedMilliseconds}ms");
Console.ReadLine();