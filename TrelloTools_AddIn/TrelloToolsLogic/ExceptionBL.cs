using System.Collections.Generic;

namespace TrelloToolsLogic
{
    public class ExceptionBL
    {
        private static Dictionary<string, string> exceptions = FillExceptions();

        public string GetMessage(string key)
        {
            return exceptions[key];
        }

        private static Dictionary<string, string> FillExceptions()
        {
            exceptions = new Dictionary<string, string>
            {
                { "01", "Error into startup operation about add-in." },
                { "02", "Error to connect your Trello account with this tool." },
                { "03", "Error to disconnect your Trello account with this tool." },
                { "04", "Error to retrieve boards from your Trello account.\r\nPlease try again or check your API credentials." },
                { "05", "Error to retrieve Views from your Trello account.\r\nPlease try again or check your API credentials." },
                { "06", "Error to save this email." },
                { "07", "Error into attachment this email to Trello card.\r\nPlease try again." },
                { "08", "Error into add card inside board." },
                { "09", "Error into attach email into card.\r\nAs the card was created correctly, you can attach it manually from Trello application." },
                { "10", "Error to retrieve theme of your Outlook application." }
            };
            return exceptions;
        }
    }
}
