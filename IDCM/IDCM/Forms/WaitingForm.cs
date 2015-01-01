using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.ViewLL.Manager;
using IDCM.ServiceBL;
using IDCM.SimpleDAL.POO;
using IDCM.SimpleDAL.DAM;
using IDCM.AppContext;

namespace IDCM.Forms
{
    public partial class WaitingForm : Form
    {
        public WaitingForm()
        {
            InitializeComponent();
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {
        }

        private void WaitingForm_Shown(object sender, EventArgs e)
        {
            ////检查用户工作空间有效性
            //if (WorkSpaceHolder.verifyForLoad(preparepath))
            //{
            //    if (CustomTColDefDAM.checkTableSetting())
            //        this.DialogResult = DialogResult.OK;
            //    else
            //        this.DialogResult = DialogResult.No;
            //}
            //else
            //{
            //    this.DialogResult = DialogResult.Cancel;
            //}
            //this.Close();
        }
    }
}
