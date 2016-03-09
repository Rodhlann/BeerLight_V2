using System;
using System.Diagnostics;
using System.Windows.Forms;
//using SystemTrayApp.Properties;

namespace SystemTrayApp
{
    class ProcessIcon : IDisposable
    {
        public ProcessIcon()
        {
            NotifyIcon ni = new NotifyIcon(); 
        }

        public void Display()
        {
            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Icon = Resource.BeerLightApp;
            ni.Text = "Beer Light App";
            ni.Visible = true;

            ni.ContextMenuStrip = new ContextMenus().Create(); 
        }

        public void Dispose()
        {
            ni.Dispose(); 
        }

        void ni_MouseClick(object sender, MouseEventArgs e) 
        {
            if (e.Button == MouseButton.Left)
            {
                Process.Start("explorer", null); 
            }
        }
    }
}
