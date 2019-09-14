using System.Collections.Generic;

namespace solid_project_1
{
    public interface ITradeParser
    {
        List<TradeRecord> Parse(List<string> lines);
    }
}