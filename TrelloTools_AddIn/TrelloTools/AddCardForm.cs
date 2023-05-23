using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.PropertyGridInternal;
using TrelloToolsLogic;
using TrelloToolsBean;
using View = TrelloToolsBean.View;
using TrelloToolsLogicInterfaces;

namespace TrelloTools { 
    public class AddCardForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label titleAddCardForm;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblBody;
        private System.Windows.Forms.TextBox txtBody;
        private System.Windows.Forms.Label lblAttachments;
        private System.Windows.Forms.Button btnAddCard;
        private System.Windows.Forms.ComboBox comboBoard;
        private System.Windows.Forms.Label lblComboBoard;
        private System.Windows.Forms.Label lblComboView;
        private System.Windows.Forms.ComboBox comboView;
        private System.Windows.Forms.Button btnCancel;
        private ExceptionBL exception = new ExceptionBL();
        private FunctionalitiesBL functionalitiesBL = new FunctionalitiesBL();
        private IUtilities utilities = new Utilities();
        private CheckBox cbAttachEmail;
        private List<string> boards = null;
        private CheckBox cbTitleEmail;
        private List<string> views = null;

        public AddCardForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.titleAddCardForm = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblBody = new System.Windows.Forms.Label();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.lblAttachments = new System.Windows.Forms.Label();
            this.btnAddCard = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboBoard = new System.Windows.Forms.ComboBox();
            this.lblComboBoard = new System.Windows.Forms.Label();
            this.lblComboView = new System.Windows.Forms.Label();
            this.comboView = new System.Windows.Forms.ComboBox();
            this.cbAttachEmail = new System.Windows.Forms.CheckBox();
            this.cbTitleEmail = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // titleAddCardForm
            // 
            this.titleAddCardForm.AutoSize = true;
            this.titleAddCardForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleAddCardForm.Location = new System.Drawing.Point(302, 49);
            this.titleAddCardForm.Name = "titleAddCardForm";
            this.titleAddCardForm.Size = new System.Drawing.Size(447, 32);
            this.titleAddCardForm.TabIndex = 0;
            this.titleAddCardForm.Text = "Add Card into Your Trello Board";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(66, 250);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(49, 25);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(171, 250);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(758, 26);
            this.txtTitle.TabIndex = 2;
            // 
            // lblBody
            // 
            this.lblBody.AutoSize = true;
            this.lblBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBody.Location = new System.Drawing.Point(66, 335);
            this.lblBody.Name = "lblBody";
            this.lblBody.Size = new System.Drawing.Size(57, 25);
            this.lblBody.TabIndex = 3;
            this.lblBody.Text = "Body";
            // 
            // txtBody
            // 
            this.txtBody.Location = new System.Drawing.Point(171, 335);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.Size = new System.Drawing.Size(758, 100);
            this.txtBody.TabIndex = 4;
            // 
            // lblAttachments
            // 
            this.lblAttachments.AutoSize = true;
            this.lblAttachments.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttachments.Location = new System.Drawing.Point(66, 478);
            this.lblAttachments.Name = "lblAttachments";
            this.lblAttachments.Size = new System.Drawing.Size(121, 25);
            this.lblAttachments.TabIndex = 5;
            this.lblAttachments.Text = "Attachments";
            // 
            // btnAddCard
            // 
            this.btnAddCard.Location = new System.Drawing.Point(327, 533);
            this.btnAddCard.Name = "btnAddCard";
            this.btnAddCard.Size = new System.Drawing.Size(146, 48);
            this.btnAddCard.TabIndex = 6;
            this.btnAddCard.Text = "ADD CARD";
            this.btnAddCard.UseVisualStyleBackColor = true;
            this.btnAddCard.Click += new System.EventHandler(this.BtnAddCard_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(510, 533);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(146, 48);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // comboBoard
            // 
            this.comboBoard.FormattingEnabled = true;
            this.comboBoard.Location = new System.Drawing.Point(171, 129);
            this.comboBoard.Name = "comboBoard";
            this.comboBoard.Size = new System.Drawing.Size(228, 28);
            this.comboBoard.TabIndex = 8;
            this.comboBoard.SelectedIndexChanged += new System.EventHandler(this.ComboBoard_SelectedIndexChanged);
            // 
            // lblComboBoard
            // 
            this.lblComboBoard.AutoSize = true;
            this.lblComboBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComboBoard.Location = new System.Drawing.Point(66, 132);
            this.lblComboBoard.Name = "lblComboBoard";
            this.lblComboBoard.Size = new System.Drawing.Size(64, 25);
            this.lblComboBoard.TabIndex = 9;
            this.lblComboBoard.Text = "Board";
            // 
            // lblComboView
            // 
            this.lblComboView.AutoSize = true;
            this.lblComboView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComboView.Location = new System.Drawing.Point(66, 190);
            this.lblComboView.Name = "lblComboView";
            this.lblComboView.Size = new System.Drawing.Size(55, 25);
            this.lblComboView.TabIndex = 10;
            this.lblComboView.Text = "View";
            // 
            // comboView
            // 
            this.comboView.FormattingEnabled = true;
            this.comboView.Location = new System.Drawing.Point(171, 190);
            this.comboView.Name = "comboView";
            this.comboView.Size = new System.Drawing.Size(228, 28);
            this.comboView.TabIndex = 11;
            // 
            // cbAttachEmail
            // 
            this.cbAttachEmail.AutoSize = true;
            this.cbAttachEmail.Location = new System.Drawing.Point(206, 481);
            this.cbAttachEmail.Name = "cbAttachEmail";
            this.cbAttachEmail.Size = new System.Drawing.Size(152, 24);
            this.cbAttachEmail.TabIndex = 12;
            this.cbAttachEmail.Text = "Attach this email";
            this.cbAttachEmail.UseVisualStyleBackColor = true;
            this.cbAttachEmail.CheckedChanged += new System.EventHandler(this.CbAttachEmail_CheckedChanged);
            // 
            // cbTitleEmail
            // 
            this.cbTitleEmail.AutoSize = true;
            this.cbTitleEmail.Location = new System.Drawing.Point(171, 282);
            this.cbTitleEmail.Name = "cbTitleEmail";
            this.cbTitleEmail.Size = new System.Drawing.Size(217, 24);
            this.cbTitleEmail.TabIndex = 13;
            this.cbTitleEmail.Text = "Set same subject of email";
            this.cbTitleEmail.UseVisualStyleBackColor = true;
            this.cbTitleEmail.CheckedChanged += new System.EventHandler(this.CbTitleEmail_CheckedChanged);
            // 
            // AddCardForm
            // 
            this.ClientSize = new System.Drawing.Size(1059, 621);
            this.Controls.Add(this.cbTitleEmail);
            this.Controls.Add(this.cbAttachEmail);
            this.Controls.Add(this.comboView);
            this.Controls.Add(this.lblComboView);
            this.Controls.Add(this.lblComboBoard);
            this.Controls.Add(this.comboBoard);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddCard);
            this.Controls.Add(this.lblAttachments);
            this.Controls.Add(this.txtBody);
            this.Controls.Add(this.lblBody);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.titleAddCardForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCardForm";
            this.ShowIcon = false;
            this.Text = "Add Trello Card";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                boards = functionalitiesBL.RetrieveBoards();
                boards.ForEach(boardName => this.comboBoard.Items.Add(boardName));
                this.comboView.Enabled = false;
            } catch(Exception ex)
            {
                MessageBox.Show(exception.GetMessage(ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ComboBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.comboView.Enabled = true;
                this.comboView.Items.Clear();
                string boardSelected = this.comboBoard.Text;
                views = functionalitiesBL.RetrieveView(boardSelected);
                views.ForEach(view => this.comboView.Items.Add(view));
            } catch(Exception ex)
            {
                MessageBox.Show(exception.GetMessage(ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private bool ControlsBeforeAddingCard()
        {
            bool isError = false;
            if (String.IsNullOrEmpty(this.comboBoard.Text))
            {
                MessageBox.Show(Constants.WARNING_COMBOBOARD_MESSAGE);
                isError = true;
            }
            else if (!isError && String.IsNullOrEmpty(this.comboView.Text))
            {
                MessageBox.Show(Constants.WARNING_COMBOVIEW_MESSAGE);
                isError = true;
            }
            else if (!isError && String.IsNullOrEmpty(this.txtTitle.Text))
            {
                MessageBox.Show(Constants.WARNING_TITLEMAIL_MESSAGE);
                isError = true;
            }
            return isError;
        }

        private void BtnAddCard_Click(object sender, EventArgs e)
        {
            if (ControlsBeforeAddingCard())
            {
                return;
            }
            try
            {
                string idList = this.comboView.Text;
                string name = this.txtTitle.Text;
                string body = this.txtBody.Text;
                bool isAttachEmail = this.cbAttachEmail.Checked;
                bool isCardAdded = functionalitiesBL.AddCardToBoard(idList, name, body, isAttachEmail);
                String messageOperation = (isCardAdded) ? Constants.SUCCESS_ADDING_CARD_PROCESS : Constants.WARNING_ADDING_CARD_PROCESS_MESSAGE;   
                MessageBox.Show(messageOperation);
            } catch(Exception ex)
            {
                MessageBox.Show(exception.GetMessage(ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
            finally
            {
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CbTitleEmail_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbTitleEmail = (CheckBox)sender;
            if (cbTitleEmail.Checked)
            {
                dynamic mail = utilities.GetOrCreateEmailSingletonInstance();
                this.txtTitle.Text = mail.Details.Subject;
            } else
            {
                this.txtTitle.Clear();
            }
        }

        private void CbAttachEmail_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbAttachEmail = (CheckBox)sender;
            try
            {
                if (cbAttachEmail.Checked)
                {
                    functionalitiesBL.SaveEmail();
                }
            } catch(Exception ex)
            {
                MessageBox.Show(exception.GetMessage(ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
