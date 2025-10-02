using System.Collections.Generic;

namespace solid_project_1
{
    public interface ITradeDataProvider
    {
        List<string> GetTradeData(Stream stream);
    }
}