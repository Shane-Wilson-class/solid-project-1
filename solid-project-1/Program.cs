using System;
using System.IO;
using LiteDB;

namespace solid_project_1
{
    public static class Program
    {
        private static void Main()
        {
            var tradeStream = File.OpenRead("trades.txt");
            var tradeProcessor = new TradeProcessor(new TradeParser(), new TradeStorage(), new TradeDataProvider());
            tradeProcessor.ProcessTrades(tradeStream);

            using var db = new LiteRepository(@"trades.db");

            db.Query<TradeRecord>().ToList().ForEach(Console.WriteLine);
        }
    }
}
