using System;
using System.Collections.Generic;
using LiteDB;

namespace solid_project_1;

public class TradeStorage : ITradeStorage
{
    public void Persist(List<TradeRecord> trades)
    {
        using (var db = new LiteRepository(@"trades.db"))
        {
            // Clear existing trades before inserting new ones to prevent duplicate trade records
            // from accumulating when the application is run multiple times. This ensures the
            // database contains only the current set of trades from the input file, making the
            // behavior predictable for educational purposes.
            db.DeleteMany<TradeRecord>(_ => true);

            foreach (var tradeRecord in trades)
            {
                db.Insert(tradeRecord);
            }
        }

        Console.WriteLine($"INFO: {trades.Count} trades processed");
    }
}