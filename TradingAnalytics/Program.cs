using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingAnalytics.Services;

namespace TradingAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to close");
            var intradayPositionCalculator = new ValuedRTPositionService();
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
