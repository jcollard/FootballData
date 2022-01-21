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
            for (int year = 2021; year >= (2021-50); year--)
            {
                string data = stats.GetYear(year);
                Console.WriteLine(data);
            }
        }

    }
}
