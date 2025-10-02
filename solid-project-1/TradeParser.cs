using System.Collections.Generic;
using System.IO;

namespace solid_project_1
{
    public class TradeParser
    {
        public IEnumerable<TradeRecord> Parse(List<string> lines)
        {
            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split([',']).ToList();

                if (!TradeProcessor.ValidateTradeData(fields, lineCount))
                {
                    continue;
                }

                var trade = TradeProcessor.MapTradeDataToTradeRecord(fields);

                trades.Add(trade);

                lineCount++;
            }

            return trades;
        }
    }
}
