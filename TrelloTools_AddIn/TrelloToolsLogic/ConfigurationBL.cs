using System;
using System.IO;
using System.Text;
using TrelloToolsBean;
using TrelloToolsLogicInterfaces;

namespace TrelloToolsLogic
{
    public class ConfigurationBL
    {
        private IFileWrapper fileWrapper;

        public ConfigurationBL() 
        {
            this.fileWrapper = new FileWrapper();
        }

        public ConfigurationBL(IFileWrapper fileWrapper)
        {
            this.fileWrapper = fileWrapper;
        }

        public void ConnectTrelloAccount(string key, string token)
        {
            try
            {
                fileWrapper.Create(Constants.PATH_CONFIGURATION_FILE).Close();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("key: " + key + "\r\n");
                stringBuilder.Append("token: " + token + "\r\n");
                stringBuilder.Append("dataCreationFile: " + DateTime.Now);
                fileWrapper.WriteAllText(Constants.PATH_CONFIGURATION_FILE, stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw new CustomException("02");
            }
        }

        public void DisconnectTrelloAccount()
        {
            try
            {
                fileWrapper.Delete(Constants.PATH_CONFIGURATION_FILE);
            } catch(Exception ex) 
            {
                throw new CustomException("03");
            }
        }
    }
}
