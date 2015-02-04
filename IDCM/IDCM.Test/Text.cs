using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace IDCM.Test
{
    public partial class Text : Form
    {
        private BindingSource oBS = new BindingSource();

        public Text()
        {
            InitializeComponent();
            loadXML();
        }

        public void loadXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            //XmlReader reader = XmlReader.Create(@"C:\Users\ding\Desktop\EN0JNJ1JRP2EXL2Q4GTU1XFINF.xml");
            XmlReader reader = XmlReader.Create(@"C:\Users\ding\Desktop\test.xml");
            xmlDoc.Load(reader);
            XmlElement rootNode = xmlDoc.DocumentElement;
            XmlNodeList rootNodeList = rootNode.ChildNodes;
            foreach(XmlNode node in rootNodeList)
            {
                XmlNodeList oneNodeList = node.ChildNodes;
                if(oneNodeList==null)
                    Console.WriteLine(node.Name);
            }
            reader.Close();
        }

        private void loadData()
        {
            this.dataGridView1.DataSource = oBS;
            DataTable dt = GetDataSource();
            oBS.DataSource = dt;
        }

        private DataTable GetDataSource()
        {
            DataTable dTable = new DataTable();

            DataRow dRow = null;
            DateTime dTime;
            Random rnd = new Random();

            dTable.Columns.Add("IsChecked", System.Type.GetType("System.Boolean"));
            dTable.Columns.Add("RandomNo");
            dTable.Columns.Add("Date");
            dTable.Columns.Add("Time");
            dTable.Columns.Add("Annoation");

            for (int n = 0; n < 10; ++n)
            {
                dRow = dTable.NewRow();
                dTime = DateTime.Now;

                dRow["IsChecked"] = "false";
                dRow["RandomNo"] = rnd.NextDouble();
                dRow["Date"] = dTime.ToString("MM/dd/yyyy");
                dRow["Time"] = dTime.ToString("hh:mm:ss tt");

                dTable.Rows.Add(dRow);
                dTable.AcceptChanges();
            }

            return dTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BindingSource oBS = (BindingSource)this.dataGridView1.DataSource;
            DataTable dTable = (DataTable)oBS.DataSource;
            duoThread(dTable);
        }
       
        public void duoThread(DataTable dt)
        {
            ParameterizedThreadStart pts = new ParameterizedThreadStart(refreshData);
            Thread thread = new Thread(pts);
            thread.Start(dt);
        }

        private void refreshData(object obj)
        {
            DataTable dTable = (DataTable)obj;
            
            DataRow dRow = dTable.NewRow();
            DateTime dTime = DateTime.Now; ;
            dRow["IsChecked"] = "true";
            dRow["RandomNo"] = 666666;
            dRow["Date"] = dTime.ToString("MM/dd/yyyy");
            dRow["Time"] = dTime.ToString("hh:mm:ss tt");
            dTable.Rows.Add(dRow);
            //oBS.DataSource = dTable;
            this.dataGridView1.Refresh();
        }
    }
}
