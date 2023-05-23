using System.Collections.Generic;

namespace TrelloToolsLogicInterfaces
{
    public interface IUtilities
    {
        bool CheckConfFileExists();

        Dictionary<string, string> RetrieveCredentials();

        bool IsDarkTheme();

        string NormalizeString(string text);

        string ConcatenateCredentialsWithUrl(string uri, string key, string token);

        dynamic GetOrCreateEmailSingletonInstance();
    }
}
