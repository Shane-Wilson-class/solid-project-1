using System.Collections.Generic;

namespace solid_project_1
{
    public interface IDatabaseRepository
    {
        void ClearAllTrades();
        void InsertTrade(TradeRecord trade);
        void InsertTrades(IEnumerable<TradeRecord> trades);
        List<TradeRecord> GetAllTrades();
        int GetTradeCount();
    }
}