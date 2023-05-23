using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Windows.Forms;
using TrelloToolsLogic;

namespace TrelloTools
{
    public partial class RibbonTrelloTools
    {
        private bool isCheckedBtnConfig = false;
        private ExceptionBL exception = new ExceptionBL();

        private void btnTrelloToolsStart_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                bool isCheckBtnConfigTemp = !isCheckedBtnConfig;
                Microsoft.Office.Interop.Outlook.Inspector inspector = (Microsoft.Office.Interop.Outlook.Inspector)e.Control.Context;
                InspectorWrapper inspectorWrapper = Globals.ThisAddIn.InspectorWrappers[inspector];
                CustomTaskPane taskPane = inspectorWrapper.taskPane;
                if (taskPane != null)
                {
                    taskPane.Visible = isCheckBtnConfigTemp;
                }
                isCheckedBtnConfig = isCheckBtnConfigTemp;
            } catch (Exception ex) {
                MessageBox.Show(exception.GetMessage(ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
