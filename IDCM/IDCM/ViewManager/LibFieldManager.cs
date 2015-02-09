using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.Forms;
using IDCM.Service;
using IDCM.Modules;

namespace IDCM.ViewManager
{
    class LibFieldManager : ManagerA
    {
        #region 构造&析构
        public LibFieldManager()
        {
            libFieldView = new LibFieldSettingView();
            libFieldView.setManager(this);

            libFieldView.OnRemoveField += OnFieldViewRemoveField;
            libFieldView.OnSelectTemplate += OnFieldViewSelectTemplate;
            libFieldView.OnAppendField += OnFieldViewAppendField;
            libFieldView.OnOverwriteField += OnFieldViewOverwriteField;
            libFieldView.OnSubmitSetting += OnFieldViewSubmitSetting;

            filedBuilder = new LibFieldBuilder(libFieldView.getTemplateChx(), libFieldView.getFieldDGV());
        }

        ~LibFieldManager()
        {
            dispose();
        }

        #endregion
        #region 实例对象保持部分
        
        //页面窗口实例
        private volatile LibFieldSettingView libFieldView = null;
        private volatile LibFieldBuilder filedBuilder = null;

        internal LibFieldBuilder FiledBuilder
        {
            get { return filedBuilder; }
        }
        #endregion
        #region 接口实例化部分
        public override void dispose()
        {
            _isDisposed = true;

            if (filedBuilder != null)
            {
                filedBuilder.Dispose();
                filedBuilder = null;
            }
            if (libFieldView != null && !libFieldView.IsDisposed)
            {
                libFieldView.Close();
                libFieldView.Dispose();
                libFieldView = null;
            }
        }
        /// <summary>
        /// Manager对象实例化初始化请求方法,一般需支持内置窗口的检查与重建操作。
        /// @author JiahaiWu
        /// </summary>
        /// <returns></returns>
        public override bool initView(bool activeShow = true)
        {
            if (libFieldView == null || libFieldView.IsDisposed)
            {
                libFieldView = new LibFieldSettingView();
                libFieldView.setManager(this);
                filedBuilder = new LibFieldBuilder(libFieldView.getTemplateChx(), libFieldView.getFieldDGV());
            }
            if (activeShow)
            {
                libFieldView.Show();
                libFieldView.Activate();
            }
            else
            {
                libFieldView.Hide();
            }
            return true;
        }
        public override void setMaxToNormal()
        {
            if (libFieldView.WindowState.Equals(FormWindowState.Maximized))
                libFieldView.WindowState = FormWindowState.Normal;
        }
        public override void setToMaxmize(bool activeFront = false)
        {
            libFieldView.WindowState = FormWindowState.Maximized;
            if (activeFront)
            {
                libFieldView.Show();
                libFieldView.Activate();
            }
        }
        public override void setMdiParent(Form pForm)
        {
            libFieldView.MdiParent = pForm;
        }
        public override bool isDisposed()
        {
            if (_isDisposed == false)
            {
                _isDisposed = (libFieldView == null || libFieldView.Disposing || libFieldView.IsDisposed);
            }
            return _isDisposed;
        }
        public override bool isActive()
        {
            if (libFieldView == null || libFieldView.Disposing || libFieldView.IsDisposed)
                return false;
            else
                return libFieldView.Visible;
        }
        #endregion

        private void OnFieldViewSelectTemplate(object sender, Core.IDCMViewEventArgs e)
        {
            //e.values.length = 1 ### e.values[0] = comboBox_templ.SelectedIndex
            filedBuilder.selectTemplate(e.values[0]);
        }
        private void OnFieldViewSubmitSetting(object sender, Core.IDCMViewEventArgs e)
        {
            filedBuilder.submitCustomSetting();
        }
        public bool checkFields()
        {
            return filedBuilder.checkFieldsInCustom();
        }
        private void OnFieldViewAppendField(object sender, Core.IDCMViewEventArgs e)
        {
            //e.values.length = 2 ### e.values[0] = dataGridView_fields.Rows[e.RowIndex] e.values[1] = this.comboBox_templ.SelectedItem.ToString()
            filedBuilder.appendField(e.values[0], e.values[1]);
        }
        private void OnFieldViewRemoveField(object sender, Core.IDCMViewEventArgs e)
        {
            //e.values.length = 1 ###  e.values[0] = dataGridView_fields.Rows[rowIndex]
            filedBuilder.removeField(e.values[0]);
        }
        private void OnFieldViewOverwriteField(object sender, Core.IDCMViewEventArgs e)
        {
            ////e.values.length = 2 ### e.values[0] = dataGridView_fields.Rows[e.RowIndex] e.values[1] = this.comboBox_templ.SelectedItem.ToString()
            filedBuilder.overwriteField(e.values[0], e.values[1]);
        }
    }
}
