using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Service.Utils;
using IDCM.Service.Common;
using IDCM.Core;

namespace IDCM.Modules
{
    class LocalDBSearchBuilder
    {
        #region 构造&析构
        public LocalDBSearchBuilder(TableLayoutPanel dbSearchPanel, SplitContainer searchSplter)
        {
            this.splter = searchSplter;
            this.searchPanel = dbSearchPanel;
            bindComboBoxSearchLibHandle();
            int rowCount = this.searchPanel.RowCount - 2;
            panelList = new List<Panel>(rowCount);
            for (int i =1; i <=rowCount; i++)
            {
                Panel panel = this.searchPanel.GetControlFromPosition(0, i) as Panel;
                panelList.Add(panel);
            }
            initCombineGroupClickHandle();
        }
        ~LocalDBSearchBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            if(panelList!=null)
                panelList.Clear();
            searchPanel = null;
            splter = null;
        }
        #endregion
        #region 实例对象保持部分
        private TableLayoutPanel searchPanel = null;
        private SplitContainer splter = null;
        private List<Panel> panelList = null;
        private string[] combineItems = new string[] { "And", "Or", "Not" };
        private string[] conditionItems = new string[] { "Contains", "Equals", "Is Less than","Is greater than","Is start with","Is end with" };
        private const string panel_searchHead = "panel_searchHead";
        private const string comboBox_comd = "comboBox_comd";
        private const string comboBox_attr = "comboBox_attr";
        private const string comboBox_cond = "comboBox_cond";
        private const string textBox_search = "textBox_search";
        private const string button_add = "button_add";
        private const string button_rem = "button_rem";
        private const string btn_options = "btn_options";
        private const string btn_search = "btn_search";
        private const string comboBox_searchLib = "comboBox_searchLib";
        private const string checkBox_case = "checkBox_case";
        private const string checkBox_words = "checkBox_words";
        #endregion
        public void bindComboBoxSearchLibHandle()
        {
            Control btn = searchPanel.GetControlFromPosition(0,0).Controls[comboBox_searchLib];
            ControlUtil.ClearEvent(btn, "Click");
            btn.Click += delegate(object tsender, EventArgs te) { refreshLibSearchList(tsender, te); };
        }
        /// <summary>
        /// 初始化组合条件各行包含的控件的事件处理方法
        /// </summary>
        public void initCombineGroupClickHandle()
        {
            Control addBtn = panelList[0].Controls[button_add + 1];
            if (addBtn != null)
            {
                ControlUtil.ClearEvent(addBtn, "Click");
                addBtn.Click += delegate(object tsender, EventArgs te) { addSearchComb(tsender, te); };
            }
            for (int i = panelList.Count(); i > 1; i--)
            {
                Control remBtn = panelList[i - 1].Controls[button_rem + i];
                if (remBtn != null)
                {
                    ControlUtil.ClearEvent(remBtn, "Click");
                    remBtn.Click += delegate(object tsender, EventArgs te) { reduceSearchComb(tsender, te); };
                }
            }
            for (int i = panelList.Count(); i > 0; i--)
            {
                Control panel = panelList[i - 1];
                Control ctrl = null;
                if (i > 1)
                {
                    ctrl=panel.Controls[comboBox_comd + i];
                    (ctrl as ComboBox).Items.Clear();
                    (ctrl as ComboBox).Items.AddRange(combineItems);
                }
                ctrl = panel.Controls[comboBox_cond + i];
                (ctrl as ComboBox).Items.Clear();
                (ctrl as ComboBox).Items.AddRange(conditionItems);
            }
        }
        /// <summary>
        /// 显示或折叠数据查询输入表单界面
        /// </summary>
        public void showDBDataSearch()
        {
            if (splter.Panel1Collapsed == true)
            {
                int defaultRowCount = 2;
                Dictionary<string,int> viewDBMap =LocalRecordMHub.getCustomViewDBMapping(DataSourceHolder.DataSource);
                for (int i = panelList.Count(); i>0; i--)
                {
                    Control panel = panelList[i-1];
                    panel.Controls[textBox_search + i].Text = "";
                    panel.Controls[textBox_search + i].Focus();
                    if (i > defaultRowCount)
                    {
                        panel.Hide();
                        searchPanel.RowStyles[i].Height = 0;
                    }
                    ///////////////////
                    Control ctrl =null;
                    if (i > 1)
                    {
                        ctrl = panel.Controls[comboBox_comd + i];
                        (ctrl as ComboBox).SelectedIndex = 0;
                    }
                    ctrl = panel.Controls[comboBox_attr + i];
                    (ctrl as ComboBox).DataSource = new BindingSource(viewDBMap, null);
                    (ctrl as ComboBox).DisplayMember = "Key";
                    (ctrl as ComboBox).ValueMember = "Value";
                    (ctrl as ComboBox).SelectedIndex = 0;
                    ctrl = panel.Controls[comboBox_cond + i];
                    (ctrl as ComboBox).SelectedIndex = 0;
                }
                this.splter.SplitterDistance = (defaultRowCount+1) * panelList[0].Height;
                this.splter.Panel1Collapsed = false;
            }
            else
                splter.Panel1Collapsed = true;
        }
        public void refreshLibSearchList(object sender, EventArgs e)
        {
            Dictionary<string, int> libSearchMap = LocalRecordMHub.getSearchMap(DataSourceHolder.DataSource);
            (sender as ComboBox).DataSource = new BindingSource(libSearchMap, null);
            (sender as ComboBox).DisplayMember = "Key";
            (sender as ComboBox).ValueMember = "Value";
            (sender as ComboBox).Refresh();
        }
        public void addSearchComb(object sender, EventArgs e)
        {
            int idx = Convert.ToInt32((sender as Control).Name.Substring((sender as Control).Name.Length - 1));
            while (idx < panelList.Count() && idx>0)
            {
                ++idx;
                panelList[idx - 1].Controls[textBox_search + idx].Text = "";
                panelList[idx - 1].Controls[textBox_search + idx].Focus();
                if (panelList[idx - 1].Visible == false)
                {
                    panelList[idx - 1].Show();
                    searchPanel.RowStyles[idx].Height = panelList[0].Height;
                    break;
                }
            }
            if (this.splter.SplitterDistance < (idx + 1) * panelList[0].Height)
                this.splter.SplitterDistance = (idx + 1) * panelList[0].Height;
        }
        public void reduceSearchComb(object sender, EventArgs e)
        {
            int idx = Convert.ToInt32((sender as Control).Name.Substring((sender as Control).Name.Length - 1));
            if (idx <= panelList.Count && idx > 1)
            {
                panelList[idx - 1].Controls[textBox_search + idx].Text = "";
                panelList[idx - 1].Hide();
                searchPanel.RowStyles[idx].Height = 0;
            }
        }
        /// <summary>
        /// 根据用户界面的输入信息构造目标查询条件
        /// </summary>
        /// <returns></returns>
        public string buildWhereCmd()
        {
            for (int i = 0; i <panelList.Count(); i++)
            {
                Control panel = panelList[i];
            }
            return null;
        }
    }
}
