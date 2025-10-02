using System.Collections.Generic;
using System.IO;

namespace solid_project_1
{
    public interface ITradeDataProvider
    {
        List<string> GetTradeData(Stream stream);
    }
}

