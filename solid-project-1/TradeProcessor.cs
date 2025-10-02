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
public class TradeProcessor
{
    public void ProcessTrades(Stream stream)
    {
        var _tradeDataProvider = new TradeDataProvider();
        var lines = _tradeDataProvider.GetTradeData(stream);

        var _tradeParser = new TradeParser();
        var trades = _tradeParser.Parse(lines);

        var _tradeStorage = new TradeStorage();
        var statusMessage = _tradeStorage.Persist(trades);
        Console.WriteLine(statusMessage);
    }
}