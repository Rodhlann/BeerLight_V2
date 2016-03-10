using System;
using System.Windows.Forms;
using BeerLightApp.Properties;

namespace BeerLightApp
{
    class ProcessIcon
    {

        NotifyIcon ni;
        ContextMenu contextMenu;
        MenuItem menuItem; 

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            Display("none", false, false); 
            Application.Exit(); 
        }

        public void Display(String status, Boolean isVisible, Boolean showCachedStatus)
        {
            this.contextMenu = new ContextMenu();
            this.menuItem = new MenuItem();

            this.contextMenu.MenuItems.AddRange(
                new MenuItem[] { this.menuItem });

            this.menuItem.Index = 0;
            this.menuItem.Text = "Exit";
            this.menuItem.Click += new EventHandler(this.menuItem_Click);

            this.ni.BalloonTipText = "Status changed to: " + status.ToUpper();
            if ("none".Equals(status))
            {
                this.ni.BalloonTipTitle = "Searching For Beer Light Host!";
            }
            else
            {
                this.ni.BalloonTipTitle = "Beer Light Status Updated!";
            }

            if (showCachedStatus == true)
            {
                this.ni.ShowBalloonTip(3);
            }

            switch (status)
            {
                case "red":
                    {
                        ni.Icon = Resources.red;
                        break;
                    }   
                case "yellow":
                    {
                        ni.Icon = Resources.yellow;
                        break;
                    }
                case "green":
                    {
                        ni.Icon = Resources.green;
                        break;
                    }
                default:
                    {
                        ni.Icon = Resources.grey;
                        break;
                    }
            }
            if ("search".Equals(status))
            {
                ni.Text = "Searching for Beer Light Status...";
            }
            else if ("none".Equals(status))
            {
                ni.Text = "Unable to connect to host"; 
            }
            else 
            {
                ni.Text = "Beer Light Status";
            }
            ni.Visible = isVisible;
            ni.ContextMenu = this.contextMenu; 
        }
    }
}
