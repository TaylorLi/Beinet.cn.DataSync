using System;
using System.Windows.Forms;

namespace Beinet.cn.DataSync
{
    /// <summary>
    /// 带Scroll事件的ListView扩展
    /// </summary>
    public class ListViewEx : ListView
    {
        public event EventHandler Scroll;
        public event EventHandler HScroll;
        public event EventHandler VScroll;

        const int WM_HSCROLL = 0x0114;
        const int WM_VSCROLL = 0x0115;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HSCROLL:
                    var HArg = new EventArgs();
                    OnScroll(this, HArg);
                    OnHScroll(this, HArg);
                    break;
                case WM_VSCROLL:
                    var VArg = new EventArgs();
                    OnScroll(this, VArg);
                    OnVScroll(this, VArg);
                    break;
            }
            base.WndProc(ref m);
        }

        virtual protected void OnHScroll(object sender, EventArgs e)
        {
            if (HScroll != null)
                HScroll(this, e);
        }
        virtual protected void OnVScroll(object sender, EventArgs e)
        {
            if (VScroll != null)
                VScroll(this, e);
        }
        virtual protected void OnScroll(object sender, EventArgs e)
        {
            if (Scroll != null)
                Scroll(this, e);
        }

    }
}
