using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TrelloToolsBean;
using TrelloToolsLogicInterfaces;

namespace TrelloToolsLogic
{
    public class Utilities : IUtilities
    {
        private IFileWrapper fileWrapper;
        private IRegistryKey registryKeyWrapper;

        public Utilities() {
            this.fileWrapper = new FileWrapper();
            this.registryKeyWrapper = new RegistryKeyWrapper(Registry.CurrentUser);
        }

        public Utilities(IFileWrapper fileWrapper, IRegistryKey registryKey) 
        {
            this.fileWrapper = fileWrapper;
            this.registryKeyWrapper = registryKey;
        }

        public bool CheckConfFileExists()
        {
            if (fileWrapper.Exists(Constants.PATH_CONFIGURATION_FILE)) {
                return fileWrapper.ReadAllText(Constants.PATH_CONFIGURATION_FILE).Contains("key: ") &&
                    fileWrapper.ReadAllText(Constants.PATH_CONFIGURATION_FILE).Contains("token: ");
            } else {
                return false;
            }
        }

        public Dictionary<string, string> RetrieveCredentials()
        {
            Dictionary<string, string> keyAndApiDictionary = new Dictionary<string, string>();
            if (CheckConfFileExists())
            {
                string confFileText = fileWrapper.ReadAllText(Constants.PATH_CONFIGURATION_FILE);
                string[] stringSeparators = new string[] { "\r\n" };
                string[] confFileTextSplitted = confFileText.Split(stringSeparators, StringSplitOptions.None);
                keyAndApiDictionary.Add("key", confFileTextSplitted[0].Replace("key:", "").Trim());
                keyAndApiDictionary.Add("token", confFileTextSplitted[1].Replace("token:", "").Trim());
            } else
            {
                throw new System.Exception();
            }
            return keyAndApiDictionary;
        }

        public bool IsDarkTheme()
        {
            int theme = 0;
            try
            {
                IRegistryKey registryKey = registryKeyWrapper.OpenSubKey(@"Software\Microsoft\Office");
                var officeVersions = registryKey.GetSubKeyNames().Where(keyName => keyName.Any(char.IsDigit));
                string officeVersion = officeVersions.OrderByDescending(officeKey => Int32.Parse(officeKey.Split('.')[0])).First();
                string officeCommonKey = @"Software\Microsoft\Office\" + officeVersion + @"\Common";
                string keyOfficeTheme = "UI Theme";
                using (IRegistryKey key = registryKeyWrapper.OpenSubKey(officeCommonKey))
                {
                    theme = (int)key.GetValue(keyOfficeTheme);
                    return (theme == 4);
                }
            }catch(Exception ex) {
                throw new CustomException("10");
            }
        }

        public string NormalizeString(string text)
        {
            return Regex.Replace(text, "[^a-zA-Z0-9_ ]", "", RegexOptions.Compiled);
        }

        public string ConcatenateCredentialsWithUrl(string uri, string key, string token)
        {
            return String.Concat(uri, "?" + "key=", key, "&token=", token);
        }

        public dynamic GetOrCreateEmailSingletonInstance()
        {
            return SingletonMail.Mail;
        }
    }
}
