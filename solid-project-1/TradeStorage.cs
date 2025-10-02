using System.Collections.Generic;
using System.Linq;

namespace solid_project_1
{
    public class TradeStorage
    {
        public string StoreTrades(IEnumerable<TradeRecord> trades)
        {
            var databaseRepository = new DatabaseRepository();

            // Clear existing trades before inserting new ones to prevent duplicate trade records
            // from accumulating when the application is run multiple times. This ensures the
            // database contains only the current set of trades from the input file, making the
            // behavior predictable for educational purposes.
            databaseRepository.ClearAllTrades();

            // Insert all trades using the repository
            databaseRepository.InsertTrades(trades);

            return $"INFO: {trades.Count()} trades processed";
        }
    }
}