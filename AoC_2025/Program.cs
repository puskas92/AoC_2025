global using Xunit;
using AoC_2025;
using System.Diagnostics;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("AoC 2025");

var sw = new Stopwatch();
sw.Start();
//Day01.Day01_Main();
Day02.Day02_Main();



sw.Stop();

Console.WriteLine($"Code run under {sw.ElapsedMilliseconds}ms");
Console.ReadLine();