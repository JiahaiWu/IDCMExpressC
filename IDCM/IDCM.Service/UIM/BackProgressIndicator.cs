﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.UIM
{
    public class BackProgressIndicator
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
                if (bar != null && !bar.IsDisposed)
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
                    if (bar != null && !bar.IsDisposed)
                        bar.Visible = false;
                }
            }
            if (backProgressCount < 0)
                backProgressCount = 0;
        }
        public static void shutdownAll()
        {
            lock (BackEndProgress_Lock)
            {
                backProgressCount=0;
                foreach (ToolStripProgressBar bar in bars)
                {
                    if(bar!=null && !bar.IsDisposed)
                        bar.Visible = false;
                }
                bars.Clear();
            }
        }
        private static int backProgressCount = 0;
        private static object BackEndProgress_Lock = new object();
        private static LinkedList<ToolStripProgressBar> bars=new LinkedList<ToolStripProgressBar>();
    }
}
