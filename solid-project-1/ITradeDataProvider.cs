using System.Collections.Generic;
using System.IO;

namespace solid_project_1
{
    public interface ITradeDataProvider
    {
        IEnumerable<string> GetTradeData(Stream stream);
    }
}