using System.Collections.Generic;

namespace solid_project_1
{
    public interface ITradeStorage
    {
        string Persist(List<TradeRecord> trades);
    }
}