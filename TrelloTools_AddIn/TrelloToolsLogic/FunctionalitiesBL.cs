using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloToolsAccess;
using TrelloToolsAccessInterfaces;
using TrelloToolsBean;
using TrelloToolsBeanInterface;
using TrelloToolsLogicInterfaces;
using Exception = System.Exception;
using View = TrelloToolsBean.View;

namespace TrelloToolsLogic
{
    public class FunctionalitiesBL : IFunctionalities
    {
        private IWebRequestAC webRequest;
        private ExceptionBL exception = new ExceptionBL();
        private List<IBoard> boards = new List<IBoard>();
        private List<IView> views = new List<IView>();
        private IUtilities utilities;
        private IFileWrapper file;

        public FunctionalitiesBL() 
        {
            file = new FileWrapper();
            utilities = new Utilities();
            webRequest = new WebRequestAC();
        }

        public FunctionalitiesBL(IWebRequestAC webRequestAC, IFileWrapper file, IUtilities utilities, List<IView> views,
            List<IBoard> boards)
        {
            this.webRequest = webRequestAC;
            this.file = file;
            this.utilities = utilities;
            this.views = views;
            this.boards = boards;
        }

        public async Task<bool> AttachEmail(string cardId)
        {
            FileResponse fileResponse = null;
            string fullPathMail = null;
            try
            {   
                dynamic mail = utilities.GetOrCreateEmailSingletonInstance();
                string pathFile = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\");
                string fileName = String.Concat(utilities.NormalizeString(mail.Details.ConversationTopic), ".msg");
                fullPathMail = pathFile + fileName;
                byte[] fileContent = file.ReadAllBytes(fullPathMail);
                string URI = new Uri(String.Concat(Constants.URL_ADD_CARD, "//", cardId, Constants.PREFIX_ADD_ATTACHMENT)).ToString();
                Dictionary<string, string> parameters = utilities.RetrieveCredentials();
                string response = await webRequest.SendRequestWithFile(URI, fileContent, fileName, parameters);
                fileResponse = JsonConvert.DeserializeObject<FileResponse>(response);
            } catch(Exception ex)
            {
                throw new CustomException("09");
            }
            finally
            {
                file.Delete(fullPathMail);
            }
            return (fileResponse.Id != null && fileResponse.Bytes > 0);
        }

        public void SaveEmail()
        {
            try
            {
                dynamic mail = utilities.GetOrCreateEmailSingletonInstance();
                mail.Details.SaveAs(String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\", utilities.NormalizeString(mail.Details.ConversationTopic),".msg"), Microsoft.Office.Interop.Outlook.OlSaveAsType.olMSG);
            } catch(Exception ex)
            {
                throw new CustomException("06");
            }
        }

        public bool AddCardToBoard(string idList, string name, string body, bool isAttachMail)
        {
            ICard card = null;
            bool isAddedCard = false;
            try
            {
                string idListRecovered = views.Find(view => view.Name.Equals(idList)).Id;
                string URI = Constants.URL_ADD_CARD;
                Dictionary<string, string> parameters = utilities.RetrieveCredentials();
                parameters.Add("idList", idListRecovered);
                parameters.Add("name", name);
                parameters.Add("desc", body);
                string response = webRequest.SendRequest(URI, parameters).GetAwaiter().GetResult();
                card = JsonConvert.DeserializeObject<Card>(response);
                isAddedCard = card.Id != null;
                if (isAttachMail)
                {
                    isAddedCard = AttachEmail(card.Id).GetAwaiter().GetResult();
                }
            } catch(Exception ex)
            {
                throw new CustomException("08");
            }
            return isAddedCard;
        }

        public List<string> RetrieveView(string boardName)
        {
            List<View> viewsResponse = new List<View>();
            List<string> viewsName = new List<string>();
            try
            {
                string idBoard = boards.Find(board => board.Name.Equals(boardName)).Id;
                string URI = String.Concat(Constants.URL_VIEWS, idBoard, "/lists");
                Dictionary<string, string> parameters = utilities.RetrieveCredentials();
                string response = webRequest.GetRequest(URI, parameters);
                viewsResponse = JsonConvert.DeserializeObject<List<View>>(response);
                views = viewsResponse.ConvertAll(v => (IView) v);
                views.ForEach(view => viewsName.Add(view.Name));
            } catch(Exception ex)
            {
                throw new CustomException("05");
            }
            return viewsName;
        }

        public List<string> RetrieveBoards()
        {
            List<Board> boardsResponse = new List<Board>();
            List<string> boardsName = new List<string>();
            try
            {
                string URI = Constants.URL_BOARDS;
                Dictionary<string, string> parameters = utilities.RetrieveCredentials();
                string response = webRequest.GetRequest(URI, parameters);
                boardsResponse = JsonConvert.DeserializeObject<List<Board>>(response);
                boards = boardsResponse.ConvertAll(b => (IBoard)b);
                boards.ForEach(board => boardsName.Add(board.Name));
            } catch(Exception ex)
            {
                throw new CustomException("04");
            }
            return boardsName;
        }
    }
}
