using System;
using System.IO;
using LiteDB;
using Unity;

namespace solid_project_1
{
    public static class Program
    {
        private static readonly UnityContainer Container = new UnityContainer();

        private static void Main()
        {
            ConfigureServices();
            var tradeStream = File.OpenRead("trades.txt");
            var tradeProcessor = Container.Resolve<TradeProcessor>();
            tradeProcessor.ProcessTrades(tradeStream);

            using (var db = new LiteRepository(@"trades.db"))
            {
                db.Query<TradeRecord>().ToList().ForEach(Console.WriteLine);
            }
        }

        private static void ConfigureServices()
        {
            Container.RegisterSingleton<ITradeParser, TradeParser>();
            Container.RegisterSingleton<ITradeStorage, TradeStorage>();
            Container.RegisterSingleton<ITradeDataProvider, TradeDataProvider>();
        }
    }
}