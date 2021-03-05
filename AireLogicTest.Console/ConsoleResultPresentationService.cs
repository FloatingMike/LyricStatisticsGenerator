using System;
using AireLogicTest.LyricStatistics.Dtos;

namespace AireLogicTest
{
    public class ConsoleResultPresentationService : IResultPresentationService
    {
        public void OutputStatus(string status)
        {
            Console.WriteLine(status);
        }

        public void PresentResults(LyricStatisticsDto statistics)
        {
            Console.WriteLine(statistics.ToString());
        }
    }
}