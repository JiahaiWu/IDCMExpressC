using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Modules
{
    class BackProgressIndicator
    {
        public static void addIndicatorBar(ToolStripProgressBar progressBar)
        {
            bars.AddLast(progressBar);
        }
        public static void removeIndicatorBar(ToolStripProgressBar progressBar)
        {
            bars.Remove(progressBar);
        }
        public static void startBackProgress()
        {
            foreach (ToolStripProgressBar bar in bars)
            {
                bar.Visible = true;
            }
            lock (BackEndProgress_Lock)
            {
                ++backProgressCount;
            }
        }
        public static void endBackProgress()
        {
            lock (BackEndProgress_Lock)
            {
                --backProgressCount;
            }
            if (backProgressCount < 1)
            {
                foreach (ToolStripProgressBar bar in bars)
                {
                    bar.Visible = false;
                }
            }
            if (backProgressCount < 0)
                backProgressCount = 0;
        }
        private static int backProgressCount = 0;
        private static object BackEndProgress_Lock = new object();
        private static LinkedList<ToolStripProgressBar> bars=new LinkedList<ToolStripProgressBar>();
    }
}
