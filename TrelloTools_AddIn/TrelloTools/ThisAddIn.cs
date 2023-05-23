using System;
using System.Collections.Generic;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using System.Windows.Forms;
using Microsoft.Office.Tools;
using System.Drawing;
using TrelloToolsLogic;
using Exception = System.Exception;
using TextBox = System.Windows.Forms.TextBox;
using System.Diagnostics;
using System.Configuration;
using TrelloToolsBean;

namespace TrelloTools
{
    public partial class ThisAddIn
    {
        private ExceptionBL exception = new ExceptionBL();
        private Dictionary<Outlook.Inspector, InspectorWrapper> inspectorWrappersValue = new Dictionary<Outlook.Inspector, InspectorWrapper>();
        private Outlook.Inspectors inspectors;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            try
            {
                inspectors = this.Application.Inspectors;
                inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);

                foreach (Outlook.Inspector inspector in inspectors)
                {
                    Inspectors_NewInspector(inspector);
                }
            } catch (Exception ex)
            {
                MessageBox.Show(exception.GetMessage("01"));
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
        }

        void Inspectors_NewInspector(Outlook.Inspector Inspector)
        {
            if (Inspector.CurrentItem is Outlook.MailItem)
            {
                inspectorWrappersValue.Add(Inspector, new InspectorWrapper(Inspector));
            }
        }

        public Dictionary<Outlook.Inspector, InspectorWrapper> InspectorWrappers
        {
            get
            {
                return inspectorWrappersValue;
            }
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }

    public class InspectorWrapper
    {
        private ExceptionBL exception = new ExceptionBL();
        private Utilities utilities = new Utilities();
        private bool isDarkTheme = false;
        private static ConfigurationBL configurationBL = new ConfigurationBL();
        private static FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL();
        private Outlook.Inspector inspector;
        public CustomTaskPane taskPane;
        private UserControl userControl;
        private string Key = null;
        private string Token = null;
        
        public InspectorWrapper(Outlook.Inspector Inspector)
        {
            inspector = Inspector;
            ((Outlook.InspectorEvents_Event)inspector).Close += new Outlook.InspectorEvents_CloseEventHandler(InspectorWrapper_Close);
            dynamic mail = utilities.GetOrCreateEmailSingletonInstance();
            mail.Details = inspector.CurrentItem;
            userControl = new UserControl();
            BuildLayoutPage();
            taskPane = Globals.ThisAddIn.CustomTaskPanes.Add(userControl, Constants.TASK_PANE_HEADER, inspector);
            taskPane.Control.AutoScroll = false;
            taskPane.Control.HorizontalScroll.Enabled = false;
            taskPane.Control.HorizontalScroll.Visible = false;
            taskPane.Control.HorizontalScroll.Maximum = 0;
            taskPane.Control.AutoScroll = true;
            taskPane.DockPositionRestrict = Office.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
        }

        void InspectorWrapper_Close()
        {
            if (taskPane != null)
            {
                Globals.ThisAddIn.CustomTaskPanes.Remove(taskPane);
            }
            taskPane = null;
            Globals.ThisAddIn.InspectorWrappers.Remove(inspector);
            ((Outlook.InspectorEvents_Event)inspector).Close -= new Outlook.InspectorEvents_CloseEventHandler(InspectorWrapper_Close);
            inspector = null;
        }

        void BuildLayoutPage()
        {
            #region Retrieve Theme
            try
            {
                isDarkTheme = utilities.IsDarkTheme();
            }
            catch(Exception ex)
            {
                MessageBox.Show(exception.GetMessage(ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            #region Header
            Label lblImage = new Label();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream iconTool = assembly.GetManifestResourceStream(ConfigurationManager.AppSettings["BRAND_IMAGE_PATH_ASSEMBLY"]);
            Image ttbImage = new Bitmap(Image.FromStream(iconTool), new Size(200, 200));
            lblImage.Location = new System.Drawing.Point(50, 20);
            lblImage.Size = new System.Drawing.Size(ttbImage.Width, ttbImage.Height);
            lblImage.Image = ttbImage;
            userControl.Controls.Add(lblImage);

            Label credits = new Label();
            credits.Size = new System.Drawing.Size(250, 20);
            credits.Location = new System.Drawing.Point(65, 225);
            credits.Text = ConfigurationManager.AppSettings["CREDITS"];
            credits.ForeColor = (isDarkTheme) ? Color.White : Color.Black;
            userControl.Controls.Add(credits);
            #endregion

            #region Configuration Group Box
            bool existConfFile = !utilities.CheckConfFileExists();
            GroupBox customGbConfTrello = new GroupBox();
            customGbConfTrello.Size = new System.Drawing.Size(260, 350);
            customGbConfTrello.Location = new System.Drawing.Point(15, 300);
            customGbConfTrello.ForeColor = (isDarkTheme) ? Color.White : Color.Black;
            customGbConfTrello.Text = Constants.CONFIGURATION_GROUP_BOX_HEADER;

            Label lblKey = new Label();
            lblKey.Location = new System.Drawing.Point(30, 30);
            lblKey.Text = Constants.KEY_LABEL_VALUE;
            customGbConfTrello.Controls.Add(lblKey);

            TextBox txtBoxKey = new TextBox();
            txtBoxKey.Location = new System.Drawing.Point(30, 60);
            txtBoxKey.Width = 200;
            txtBoxKey.Height = 20;
            txtBoxKey.Enabled = existConfFile;
            Key = txtBoxKey.Text;
            txtBoxKey.TextChanged += TbKey_OnTextChanged;
            customGbConfTrello.Controls.Add(txtBoxKey);

            Label lblToken = new Label();
            lblToken.Location = new System.Drawing.Point(30, 100);
            lblToken.Text = Constants.TOKEN_LABEL_VALUE;
            lblToken.ForeColor = (isDarkTheme) ? Color.White : Color.Black;
            customGbConfTrello.Controls.Add(lblToken);

            TextBox txtBoxToken = new TextBox();
            txtBoxToken.Location = new System.Drawing.Point(30, 125);
            txtBoxToken.Width = 200;
            txtBoxToken.Height = 20;
            txtBoxToken.Enabled = existConfFile;
            Token= txtBoxToken.Text;
            txtBoxToken.TextChanged += TbToken_OnTextChanged;
            customGbConfTrello.Controls.Add(txtBoxToken);

            Button btnConnect = new Button();
            btnConnect.Location = new System.Drawing.Point(30, 175);
            btnConnect.Width = 200;
            btnConnect.Height = 50;
            btnConnect.Text = Constants.BUTTON_CONNECT_TEXT;
            btnConnect.Enabled = existConfFile;
            btnConnect.Click += ConnectTrelloAccount;
            customGbConfTrello.Controls.Add(btnConnect);

            Button btnDisconnect = new Button();
            btnDisconnect.Location = new System.Drawing.Point(30, 250);
            btnDisconnect.Width = 200;
            btnDisconnect.Height = 50;
            btnDisconnect.Text = Constants.BUTTON_DISCONNECT_TEXT;
            btnDisconnect.Enabled = !existConfFile;
            btnDisconnect.Click += DisconnectTrelloAccount;
            customGbConfTrello.Controls.Add(btnDisconnect);

            userControl.Controls.Add(customGbConfTrello);
            #endregion

            #region Functionalities Group Box
            GroupBox customGbTrelloFunctionalities = new GroupBox();
            customGbTrelloFunctionalities.Size = new System.Drawing.Size(260, 250);
            customGbTrelloFunctionalities.Location = new System.Drawing.Point(15, 675);
            customGbTrelloFunctionalities.Text = Constants.FUNCTIONALITIES_GROUP_BOX_HEADER;
            customGbTrelloFunctionalities.ForeColor = (isDarkTheme) ? Color.White : Color.Black;

            Button btnAddCard = new Button();
            btnAddCard.Location = new System.Drawing.Point(30, 50);
            btnAddCard.Width = 200;
            btnAddCard.Height = 50;
            btnAddCard.Text = Constants.BUTTON_ADD_CARD_TEXT;
            btnAddCard.Enabled = !existConfFile;
            btnAddCard.Click += AddCard_ClickEvent;
            customGbTrelloFunctionalities.Controls.Add(btnAddCard);

            userControl.Controls.Add(customGbTrelloFunctionalities);
            #endregion

            #region Footer
            Label lblGitHubIcon = new Label();
            System.IO.Stream iconGitHub = assembly.GetManifestResourceStream(ConfigurationManager.AppSettings["GITHUB_ICON_PATH_ASSEMBLY"]);
            Image ttbGitHubIcon = new Bitmap(Image.FromStream(iconGitHub), new Size(100, 100));
            lblGitHubIcon.Location = new System.Drawing.Point(100, 1000);
            lblGitHubIcon.Size = new System.Drawing.Size(ttbGitHubIcon.Width, ttbGitHubIcon.Height);
            lblGitHubIcon.Image = ttbGitHubIcon;
            lblGitHubIcon.Click += OpenGithubProfile_ClickEvent;
            lblGitHubIcon.Cursor = Cursors.Hand;
            lblGitHubIcon.ForeColor = (isDarkTheme) ? Color.White : Color.Black;
            userControl.Controls.Add(lblGitHubIcon);
            #endregion
        }

        void OpenGithubProfile_ClickEvent(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(ConfigurationManager.AppSettings["GITHUB_PROFILE"]) { UseShellExecute = true });
        }

        void AddCard_ClickEvent(object sender, EventArgs e)
        {
            AddCardForm form = new AddCardForm();
            form.Show();
        }
        
        void TbKey_OnTextChanged(object sender, EventArgs e)
        {
            TextBox tbKey = (TextBox)sender;
            Key = tbKey.Text;
        }

        void TbToken_OnTextChanged(object sender, EventArgs e)
        {
            TextBox tbToken = (TextBox)sender;
            Token = tbToken.Text;
        }

        void ConnectTrelloAccount(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(Key) || String.IsNullOrEmpty(Token))
            {
                MessageBox.Show(Constants.WARNING_KEY_TOKEN_EMPTY);
                return;
            }
            try
            {
                configurationBL.ConnectTrelloAccount(Key, Token);
                MessageBox.Show(Constants.SUCCESS_CONNECTED_TRELLO_ACCOUNT);
            } catch(CustomException customEx)
            {
                MessageBox.Show(exception.GetMessage(customEx.Code));
            }
        }

        void DisconnectTrelloAccount(object sender, EventArgs e)
        {
            try
            {
                configurationBL.DisconnectTrelloAccount();
                MessageBox.Show(Constants.SUCCESS_DISCONNECTED_TRELLO_ACCOUNT);
            } catch(CustomException customEx)
            {
                MessageBox.Show(exception.GetMessage(customEx.Code));
            }
        }
    }
}