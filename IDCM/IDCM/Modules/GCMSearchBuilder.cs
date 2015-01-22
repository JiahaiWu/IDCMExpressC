using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Modules
{
    class GCMSearchBuilder
    {
        #region 构造&析构
        public GCMSearchBuilder(TableLayoutPanel searchPanel, SplitContainer spliter)
        {
            this.searchPanel = searchPanel;
            this.splter = spliter;
        }
        ~GCMSearchBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            searchPanel = null;
            splter = null;
        }
        #endregion
        #region 实例对象保持部分
        private TableLayoutPanel searchPanel = null;
        private SplitContainer splter = null;
        #endregion
    }
}
