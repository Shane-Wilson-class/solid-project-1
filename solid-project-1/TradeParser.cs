using System;
using System.Collections.Generic;

namespace solid_project_1
{
    public class TradeParser : ITradeParser
    {
        private const float LotSize = 100;

        public List<TradeRecord> Parse(IEnumerable<string> lines)
        {
            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split(new[] { ',' });

                if (!ValidateTradeData(fields, lineCount))
                {
                    continue;
                }

                var trade = MapTradeDataToTradeRecord(fields);

                trades.Add(trade);

                lineCount++;
            }

            return trades;
        }

        private TradeRecord MapTradeDataToTradeRecord(string[] fields)
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

        private bool ValidateTradeData(string[] fields, int currentLine)
        {
            if (fields.Length != 3)
            {
                Console.WriteLine($"WARN: Line {currentLine} malformed. Only {fields.Length} field(s) found.");
                return false;
            }

            if (fields[0].Length != 6)
            {
                Console.WriteLine($"WARN: Trade currencies on line {currentLine} malformed: '{fields[0]}'");
                return false;
            }

            if (!int.TryParse(fields[1], out var tradeAmount))
            {
                Console.WriteLine($"WARN: Trade amount on line {currentLine} not a valid integer: '{fields[1]}'");
                return false;
            }


            if (!decimal.TryParse(fields[2], out var tradePrice))
            {
                Console.WriteLine($"WARN: Trade price on line {currentLine} not a valid decimal: '{fields[2]}'");
                return false;
            }

            return true;
        }
    }
}