using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrelloToolsLogicInterfaces
{
    public interface IFunctionalities
    {
        Task<bool> AttachEmail(string cardId);

        void SaveEmail();

        bool AddCardToBoard(string idList, string name, string body, bool isAttachMail);

        List<string> RetrieveView(string boardName);

        List<string> RetrieveBoards();
    }
}
