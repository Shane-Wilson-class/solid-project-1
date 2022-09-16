using System.Collections.Generic;
using System.IO;

namespace solid_project_1
{
    public class TradeDataProvider : ITradeDataProvider
    {
        public List<string> GetTradeData(Stream stream)
        {
            var lines = new List<string>();
            using var reader = new StreamReader(stream);
            while (reader.ReadLine() is { } line)
            {
                lines.Add(line);
            }

            return lines;
        }
    }
}
