using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.Forms;

namespace IDCM.ViewManager
{
    class LibFieldManager : ManagerA
    {
        #region 构造&析构
        public LibFieldManager()
        {
            libFieldView = new LibFieldSettingView();
            //LongTermHandleNoter.note(libFieldView);
            //libFieldView.setManager(this);
            //filedBuilder = new LibFieldBuilder(libFieldView.getTemplateChx(), libFieldView.getFieldDGV());
        }
        public static LibFieldManager getInstance()
        {
            //ManagerI hvm = IDCMAppContext.MainManger.getManager(typeof(LibFieldManager));
            //return hvm == null ? null : (hvm as LibFieldManager);
            return null;
        }
        ~LibFieldManager()
        {
            dispose();
        }

        #endregion
        #region 实例对象保持部分
        
        //页面窗口实例
        private volatile LibFieldSettingView libFieldView = null;
        //private volatile LibFieldBuilder filedBuilder = null;

        //internal LibFieldBuilder FiledBuilder
        //{
        //    get { return filedBuilder; }
        //}
        #endregion
        #region 接口实例化部分
        public override void dispose()
        {
            _isDisposed = true;

            //if (filedBuilder != null)
            //{
            //    filedBuilder.Dispose();
            //    filedBuilder = null;
            //}
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
                //LongTermHandleNoter.note(libFieldView);
                //libFieldView.setManager(this);
                //filedBuilder = new LibFieldBuilder(libFieldView.getTemplateChx(), libFieldView.getFieldDGV());
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

        public void selectTemplate(int sIndex)
        {
            //filedBuilder.selectTemplate(sIndex);
        }
        public void submitSetting()
        {
            //filedBuilder.submitCustomSetting();
        }
        public bool checkFields()
        {
            //return filedBuilder.checkFieldsInCustom();
            return false;
        }
        public void appendField(DataGridViewRow dgvr, string groupName)
        {
            //filedBuilder.appendField(dgvr, groupName);
        }
        public void removeField(DataGridViewRow dgvr)
        {
            //filedBuilder.removeField(dgvr);
        }
        public void overwriteField(DataGridViewRow dgvr, string groupName)
        {
            //filedBuilder.overwriteField(dgvr, groupName);
        }
    }
}
