using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.ViewManager
{
    interface ManagerI
    {
        /// <summary>
        /// Manager对象实例化初始化请求方法,一般需支持内置窗口的检查与重建操作。
        /// @author JiahaiWu
        /// </summary>
        /// <param name="activeShow"></param>
        /// <returns></returns>
        bool initView(bool activeShow = true);
        /// <summary>
        /// 释放内置对象，用于对象主动销毁或是重建前的清理操作需要
        /// </summary>
        void dispose();
        bool isActive();
        bool isDisposed();
        void setMaxToNormal();
        void setToMaxmize(bool activeFront = false);
        void setMdiParent(Form pForm);
    }
}
