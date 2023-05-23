using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrelloToolsAccessInterfaces
{
    public interface IWebRequestAC
    {
        string GetRequest(string URI, Dictionary<string, string> parameters);

        Task<string> SendRequest(string URI, Dictionary<string, string> parameters);

        Task<string> SendRequestWithFile(string URI, byte[] fileContent, string fileName, Dictionary<string, string> parameters);
    }
}
