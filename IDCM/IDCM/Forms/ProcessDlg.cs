using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Forms
{
    public partial class ProcessDlg : Form
    {
        public ProcessDlg(string tipstr = null)
        {
            InitializeComponent();
            if (tipstr != null)
            {
                this.label1.Text = tipstr;
            }
        }
    }
}
