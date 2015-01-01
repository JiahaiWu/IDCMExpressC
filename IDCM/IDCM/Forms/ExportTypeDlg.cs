using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.OverallSC.Commons.Generator;
using System.IO;
using IDCM.ServiceBL.Common;

namespace IDCM.Forms
{
    public partial class ExportTypeDlg : Form
    {
        public ExportTypeDlg()
        {
            InitializeComponent();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            this.Dispose();
        }
        protected string getDefaultSuffix()
        {
            string suffix = "";
            if (radioButton_json.Checked)
            {
                lastOptionValue = ExportType.JSONList;
                suffix = ".jso";
            }
            if (radioButton_excel.Checked)
            {
                lastOptionValue = ExportType.Excel;
                suffix = ".xlsx";
            }
            if (radioButton_csv.Checked)
            {
                lastOptionValue = ExportType.CSV;
                suffix = ".csv";
            }
            if (radioButton_tsv.Checked)
            {
                lastOptionValue = ExportType.TSV;
                suffix = ".tsv";
            }
            return suffix;
        }
        private void button_confirm_Click(object sender, EventArgs e)
        {
            string suffix = getDefaultSuffix();
            FileInfo fi=new FileInfo(textBox_path.Text.Trim());
            if (fi.Exists || (fi.Directory!=null && fi.Directory.Exists))
            {
                string fpath = fi.FullName;
                if (Directory.Exists(fpath))
                {
                    fpath = Path.GetDirectoryName(fpath) + "\\" + CUIDGenerator.getUID(CUIDGenerator.Radix_32) + suffix;
                }
                if (Path.GetExtension(fpath) == "")
                {
                    fpath += suffix;
                }
                textBox_path.Text = fpath;
                lastFilePath = fpath;
                this.DialogResult = DialogResult.OK;
                this.Close();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("The save file path should be available.");
            }
        }
        /// <summary>
        /// 双击显示文件浏览对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_path_DoubleClick(object sender, EventArgs e)
        {
            SaveFileDialog fbd = new SaveFileDialog();
            fbd.FileName = CUIDGenerator.getUID(CUIDGenerator.Radix_32) + getDefaultSuffix();
            fbd.InitialDirectory =Path.GetDirectoryName(lastFilePath);
            if (radioButton_excel.Checked)
                fbd.Filter = "Excel File(*.xls,*.xlsx)|*.xls;*.xlsx;";
            if (radioButton_json.Checked)
                fbd.Filter = "JSON File(*.jso)|*.jso;";
            if (radioButton_csv.Checked)
                fbd.Filter = "Text File(*.csv)|*.csv;";
            if (radioButton_tsv.Checked)
                fbd.Filter = "Text File(*.tsv)|*.tsv;";
            fbd.SupportMultiDottedExtensions = false;
            fbd.OverwritePrompt = true;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox_path.Text = fbd.FileName;
            }
        }

        private static ExportType lastOptionValue = ExportType.Excel;
        private static string lastFilePath = "C:\\";

        public static string LastFilePath
        {
            get { return ExportTypeDlg.lastFilePath; }
        }

        public static ExportType LastOptionValue
        {
            get { return ExportTypeDlg.lastOptionValue; }
        }
        
    }
}
