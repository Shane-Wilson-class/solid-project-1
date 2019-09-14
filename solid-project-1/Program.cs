using System;
using System.IO;
using LiteDB;
using Unity;


namespace solid_project_1
{
    internal static class Program
    {
        //private static readonly UnityContainer Container = new UnityContainer(); 

        private static void Main()
        {
            var tradeStream = File.OpenRead("trades.txt");
            var tradeProcessor = new TradeProcessor();
            tradeProcessor.ProcessTrades(tradeStream);

            using (var db = new LiteRepository(@"trades.db"))
            {
                db.Query<TradeRecord>().ToList().ForEach(Console.WriteLine);
            }
        }
    }
}