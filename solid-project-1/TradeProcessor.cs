using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;

namespace solid_project_1;

/// <summary>
///     The responsibilities of the TradeProcessor are reading streams, parsing strings, validating fields, logging, and
///     database insertion. The single responsibility principle states that this class, like all others, should have only a
///     single reason to change. However, the reality of the TradeProcessor is that it will change under the following
///     circumstances:
///     When you decide not to use a Stream for input but instead read the trades from a remote call to a web service.
///     When the format of the input data changes, perhaps with the addition of an extra field indicating the broker for
///     the transaction.
///     When the validation rules of the input data change.
///     When the way in which you log warnings, errors, and information changes.
///     When the database changes in any way.
///     Example from Adaptive Code: Agile coding with design patterns and SOLID principles 2nd Edition, by McLean Hall, Gary
/// </summary>
public static class TradeProcessor
{
    private const float LotSize = 100;

    public static void ProcessTrades(Stream stream)
    {
        var dataProvider = new TradeDataProvider();
        var lines = dataProvider.GetTradeData(stream);

        var parser = new TradeParser();
        var trades = parser.Parse(lines);

        var storage = new TradeStorage();
        var statusMessage = storage.StoreTrades(trades);
        Console.WriteLine(statusMessage);
    }

    

    

    private static bool ValidateTradeData(IReadOnlyList<string> fields, int currentLine)
    {
        if (fields.Count != 3)
        {
            Console.WriteLine($"WARN: Line {currentLine} malformed. Only {fields.Count} field(s) found.");
            return false;
        }

        if (fields[0].Length != 6)
        {
            Console.WriteLine($"WARN: Trade currencies on line {currentLine} malformed: '{fields[0]}'");
            return false;
        }

        if (!int.TryParse(fields[1], out _))
        {
            Console.WriteLine($"WARN: Trade amount on line {currentLine} not a valid integer: '{fields[1]}'");
            return false;
        }


        if (!decimal.TryParse(fields[2], out _))
        {
            Console.WriteLine($"WARN: Trade price on line {currentLine} not a valid decimal: '{fields[2]}'");
            return false;
        }

        return true;
    }

    

    
}