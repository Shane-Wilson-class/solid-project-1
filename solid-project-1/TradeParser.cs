using System;
using System.Collections.Generic;
using System.Linq;

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

                if (ValidateTradeData(fields, lineCount))
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
}
