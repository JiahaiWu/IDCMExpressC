using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IDCM.ViewManager;
using IDCM.Service.Utils;
using IDCM.Data.Base;
using IDCM.Data.Base.Utils;

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
            if (radioButton_xml.Checked)
            {
                lastOptionValue = ExportType.XML;
                suffix = ".xml";
            }
            return suffix;
        }

        private void updatOptionalStatus()
        {
            exportStainTree = export_strain_tree_checkBox.Checked;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            string suffix = getDefaultSuffix();
            updatOptionalStatus();
            Console.WriteLine(ExportTypeDlg.ExportStainTree);
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
                fbd.Filter = "CSV File(*.csv)|*.csv;";
            if (radioButton_tsv.Checked)
                fbd.Filter = "Text File(*.tsv)|*.tsv;";
            if (radioButton_xml.Checked)
                fbd.Filter = "XML File(*.xml)|*.xml;";
            fbd.SupportMultiDottedExtensions = false;
            fbd.OverwritePrompt = true;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox_path.Text = fbd.FileName;
            }
        }

        public void setCheckBoxVisible(bool visible = false)
        {
            groupbox_optional.Visible = visible;
            export_strain_tree_checkBox.Visible = visible;
        }

        private static ExportType lastOptionValue = ExportType.Excel;
        private static string lastFilePath = "C:\\idcm_export";
        private static bool exportStainTree = false;

        public static bool ExportStainTree
        {
            get { return ExportTypeDlg.exportStainTree; }
        }

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
