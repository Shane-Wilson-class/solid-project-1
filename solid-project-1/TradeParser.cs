using System.Collections.Generic;
using System.IO;

namespace solid_project_1
{
    public class TradeParser
    {
        private const float LotSize = 100;
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

                var trade = MapTradeDataToTradeRecord(fields);

                trades.Add(trade);

                lineCount++;
            }

            return trades;
        }
        public TradeRecord MapTradeDataToTradeRecord(IReadOnlyList<string> fields)
    {
        var sourceCurrencyCode = fields[0].Substring(0, 3);
        var destinationCurrencyCode = fields[0].Substring(3, 3);
        var tradeAmount = int.Parse(fields[1]);
        var tradePrice = decimal.Parse(fields[2]);

        var tradeRecord = new TradeRecord
        {
            SourceCurrency = sourceCurrencyCode,
            DestinationCurrency = destinationCurrencyCode,
            Lots = tradeAmount / LotSize,
            Price = tradePrice
        };

        return tradeRecord;
    }
    }
}
