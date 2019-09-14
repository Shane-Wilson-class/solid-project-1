using System;
using System.Collections.Generic;
using LiteDB;

namespace solid_project_1
{
    public class TradeStorage : ITradeStorage
    {
        public void Persist(List<TradeRecord> trades)
        {
            using (var db = new LiteRepository(@"trades.db"))
            {
                foreach (var tradeRecord in trades)
                {
                    db.Insert(tradeRecord);
                }
            }

            Console.WriteLine($"INFO: {trades.Count} trades processed");
        }
    }
}