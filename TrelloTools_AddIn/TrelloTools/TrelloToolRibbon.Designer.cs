namespace TrelloTools
{
    partial class RibbonTrelloTools : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonTrelloTools()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RibbonTrelloTools));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.titleTrelloTools = this.Factory.CreateRibbonLabel();
            this.btnTrelloToolsStart = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.titleTrelloTools);
            this.group1.Items.Add(this.btnTrelloToolsStart);
            this.group1.Name = "group1";
            // 
            // titleTrelloTools
            // 
            this.titleTrelloTools.Label = "Trello Tools";
            this.titleTrelloTools.Name = "titleTrelloTools";
            this.titleTrelloTools.ShowLabel = false;
            // 
            // btnTrelloToolsStart
            // 
            this.btnTrelloToolsStart.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnTrelloToolsStart.Image = ((System.Drawing.Image)(resources.GetObject("btnTrelloToolsStart.Image")));
            this.btnTrelloToolsStart.Label = "Trello Tools";
            this.btnTrelloToolsStart.Name = "btnTrelloToolsStart";
            this.btnTrelloToolsStart.ShowImage = true;
            this.btnTrelloToolsStart.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnTrelloToolsStart_Click);
            // 
            // RibbonTrelloTools
            // 
            this.Name = "RibbonTrelloTools";
            this.RibbonType = "Microsoft.Outlook.Mail.Read";
            this.Tabs.Add(this.tab1);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel titleTrelloTools;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTrelloToolsStart;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonTrelloTools Ribbon1
        {
            get { return this.GetRibbon<RibbonTrelloTools>(); }
        }
    }
}
