using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FootballData
{
    class Program
    {
        static void Main(string[] args)
        {
            NFLStats stats = new NFLStats();
            
            Console.WriteLine("Enter a year between 1971 and 2021:");
            int year = int.Parse(Console.ReadLine());

            List<string> teams = stats.GetTeamNames(year);

            Console.Clear();
            Console.WriteLine($"Showing Teams from {year}");

            foreach (string team in teams)
            {
                Console.WriteLine(team);
            }

            Console.WriteLine("\nEnter a Team Name: ");
            string selectedTeam = Console.ReadLine();

            List<string> statNames = stats.GetOffenseStatNames(year);
            
            Console.WriteLine("Offensive Stats: ");
            foreach (string name in statNames)
            {
                Console.Write($"{name} ");
            }

            Console.WriteLine();
            Console.WriteLine("Select a stat: ");
            string selectedStat = Console.ReadLine();
            string stat = stats.GetOffenseStat(year, selectedTeam, selectedStat);
            Console.WriteLine($"In {year} the {selectedTeam} had {stat} in {selectedStat}!");
        }

    }
}
