using System.IO;

namespace solid_project_1
{
    public class TradeProcessor
    {
        private readonly ITradeParser _tradeParser;
        private readonly ITradeStorage _tradeStorage;
        private readonly ITradeDataProvider _tradeDataProvider;

        public TradeProcessor(ITradeParser tradeParser, ITradeStorage tradeStorage, ITradeDataProvider tradeDataProvider)
        {
            _tradeParser = tradeParser;
            _tradeStorage = tradeStorage;
            _tradeDataProvider = tradeDataProvider;
        }

        public void ProcessTrades(Stream stream)
        {
            var lines = _tradeDataProvider.GetTradeData(stream);

            var trades = _tradeParser.Parse(lines);

            _tradeStorage.Persist(trades);
        }
    }
}