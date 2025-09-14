using System;
using System.IO;
using LiteDB;

namespace solid_project_1;

public static class Program
{
    private static void Main()
    {
        using var tradeStream = File.OpenRead("trades.txt");

        // Create the database repository (concrete implementation of interface)
        IDatabaseRepository databaseRepository = new DatabaseRepository();

        // Create TradeStorage with IDatabaseRepository dependency (interface-based dependency injection)
        var tradeStorage = new TradeStorage(databaseRepository);

        var tradeProcessor = new TradeProcessor(new TradeParser(), tradeStorage, new TradeDataProvider());
        tradeProcessor.ProcessTrades(tradeStream);

        // Display all trades from the database
        databaseRepository.GetAllTrades().ForEach(Console.WriteLine);
    }
}