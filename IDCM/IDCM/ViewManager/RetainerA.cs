using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.ViewManager
{
    public abstract class RetainerA : ManagerI
    {

        public void setMaxToNormal()
        {
        }
        public void setToMaxmize(bool activeFront = false)
        {
        }
        public void setMdiParent(Form pForm)
        {
        }
        public virtual bool isActive()
        {
            return false;
        }
        public virtual bool initView(bool activeShow = true)
        {
            return true;
        }
        public virtual bool isDisposed()
        {
            return _isDisposed;
        }
        public virtual void dispose()
        {
            _isDisposed = true;
        }
        protected volatile bool _isDisposed = false;
    }
}
