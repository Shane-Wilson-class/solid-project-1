using System;
using System.IO;
using LiteDB;

namespace solid_project_1
{
    internal static class Program
    {
        private static void Main()
        {
            var tradeStream = File.OpenRead("trades.txt");
            TradeProcessor.ProcessTrades(tradeStream);

            using var db = new LiteRepository(@"trades.db");

            db.Query<TradeRecord>().ToList().ForEach(Console.WriteLine);
        }
    }
}
