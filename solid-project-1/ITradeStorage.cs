using System.Collections.Generic;

namespace solid_project_1;

public interface ITradeStorage
{
    void Persist(List<TradeRecord> trades);
}