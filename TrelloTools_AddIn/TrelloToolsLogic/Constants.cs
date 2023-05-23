using System;

namespace TrelloToolsLogic
{
    public static class Constants
    {
        #region LAYOUT
        public const string BUTTON_ADD_CARD_TEXT = "Add card";
        public const string BUTTON_CONNECT_TEXT = "Connect your Trello account";
        public const string BUTTON_DISCONNECT_TEXT = "Disconnect";
        public const string CONFIGURATION_GROUP_BOX_HEADER = "Config Trello Account";
        public const string FUNCTIONALITIES_GROUP_BOX_HEADER = "Trello Tools Functionalities";
        public const string KEY_LABEL_VALUE = "Key";
        public const string TASK_PANE_HEADER = "Trello Tools";
        public const string TOKEN_LABEL_VALUE = "Token";
        #endregion

        #region CONFIGURATION
        private static readonly string CONFIGURATION_FILE_NAME = "TrelloToolsConfig";
        public static readonly string PATH_CONFIGURATION_FILE = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\", CONFIGURATION_FILE_NAME);
        #endregion

        #region ENDPOINT
        public const string PREFIX_ADD_ATTACHMENT = "/attachments";
        public const string URL_ADD_CARD = "https://api.trello.com/1/cards";
        public const string URL_BOARDS = "https://api.trello.com/1/members/me/boards";
        public const string URL_VIEWS = "https://api.trello.com/1/boards/";
        #endregion

        #region WARNING MESSAGE
        public const string WARNING_ADDING_CARD_PROCESS_MESSAGE = "Error to adding card!\r\nTry again...";
        public const string WARNING_COMBOBOARD_MESSAGE = "Please, select a value into Board dropdown!";
        public const string WARNING_COMBOVIEW_MESSAGE = "Please, select a value into View dropdown!";
        public const string WARNING_KEY_TOKEN_EMPTY = "Please, enter valid Key and/or Token!";
        public const string WARNING_TITLEMAIL_MESSAGE = "Please, fill card title field!";
        #endregion

        #region SUCCESS MESSAGE
        public const string SUCCESS_ADDING_CARD_PROCESS = "Card added successfully!";
        public const string SUCCESS_CONNECTED_TRELLO_ACCOUNT = "Connection successfully to your Trello account.\r\nPlease close and reopen Outlook";
        public const string SUCCESS_DISCONNECTED_TRELLO_ACCOUNT = "Disconnect operation completed successfully!\r\nClose and re-open Outlook";
        #endregion
    }
}
