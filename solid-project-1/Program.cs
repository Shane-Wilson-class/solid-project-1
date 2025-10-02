using System;
using System.IO;
using LiteDB;

namespace solid_project_1;

internal static class Program
{
    private static void Main()
    {
        using var tradeStream = File.OpenRead("trades.txt");
        IDatabaseRepository databaseRepository = new DatabaseRepository();

        var tradeStorage = new TradeStorage(databaseRepository);

        var tradeProcessor = new TradeProcessor(new TradeParser(), tradeStorage, new TradeDataProvider());
        TradeProcessor.ProcessTrades(tradeStream);

        // Use DatabaseRepository to display results (demonstrates the concrete dependency)
        
        databaseRepository.GetAllTrades().ForEach(Console.WriteLine);
    }
}