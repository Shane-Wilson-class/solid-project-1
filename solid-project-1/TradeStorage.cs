using System.Collections.Generic;
using System.Linq;

namespace solid_project_1
{
    public class TradeStorage : ITradeStorage
    {
        private readonly IDatabaseRepository _databaseRepository;
        public TradeStorage(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }
        public string Persist(List<TradeRecord> trades)
        {
            _databaseRepository.ClearAllTrades();
            _databaseRepository.InsertTrades(trades);
            return $"INFO {trades.Count} trades processed";
        }
    }
}