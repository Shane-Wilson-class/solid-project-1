using System;
using System.IO;
using LiteDB;

namespace solid_project_1;

internal static class Program
{
    private static void Main()
    {
        using var tradeStream = File.OpenRead("trades.txt");
        TradeProcessor.ProcessTrades(tradeStream);

        // Use DatabaseRepository to display results (demonstrates the concrete dependency)
        var databaseRepository = new DatabaseRepository();
        databaseRepository.GetAllTrades().ForEach(Console.WriteLine);
    }
}