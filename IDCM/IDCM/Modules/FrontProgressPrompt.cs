using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Forms;
using System.Collections.Concurrent;

namespace IDCM.Modules
{
    /// <summary>
    /// 用于前端显示运行中等待提示窗口的实现类
    /// 说明：
    /// 1.可并入，可重用
    /// </summary>
    public class FrontProgressPrompt
    {
        public static void startFrontProgress(string tag)
        {
            processingCounter.AddOrUpdate(tag, 1, (key, oldVlaue) => oldVlaue + 1);
            if(processingCounter.Count==1)
                processDlg.ShowDialog();
        }

        public static void endFrontProgress(string tag)
        {
            int value = 0;
            processingCounter.TryRemove(tag, out value);
            if ((value--) >0)
                processingCounter.AddOrUpdate(tag,1, (key, oldVlaue) => oldVlaue + value);
            if (processingCounter.Count<1)
                processDlg.Close();
        }
        
        internal static ProcessDlg processDlg = new ProcessDlg();
        public static ConcurrentDictionary<string, int> processingCounter = new ConcurrentDictionary<string, int>();
    }
}
