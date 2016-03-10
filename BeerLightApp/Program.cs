using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using System.Timers;
using Newtonsoft.Json.Linq;

namespace BeerLightApp
{
    static class BeerLight
    {
        static int timeoutCount = 0; 
        public static String GetBeerLightStatus()
        {
            String urlAddress = "https://beer30v2.sparcedge.com/beer30.json";
            String data = null;
            String beerLightStatus = "none";
            HttpWebResponse response = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {
                using (StreamWriter write = File.AppendText("BeerLightApp_log.txt"))
                {
                    if (timeoutCount > 30)
                    {
                        Log(e, write, true);
                        Application.Exit();
                    }
                    else
                    {
                        Log(e, write, false);
                        beerLightStatus = "none";
                    }
                }
                timeoutCount++; 
            }

            if (response == null) { beerLightStatus = "none"; }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream recieveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(recieveStream);
                    Console.WriteLine("No Content Found...");
                    readStream.Close();
                }
                else
                {
                    readStream = new StreamReader(recieveStream, Encoding.GetEncoding(response.CharacterSet));
                    data = readStream.ReadToEnd();
                    JObject jsonObj = JObject.Parse(data);
                    beerLightStatus = (string)jsonObj["state"];
                    response.Close();
                    readStream.Close();
                }
            }
            return beerLightStatus;
        }

        static ProcessIcon icon = new ProcessIcon();
        static String cachedStatus;
        static void TickEvent(object source, ElapsedEventArgs e)
        {
            if(cachedStatus == null)
            {
                cachedStatus = "search"; 
            }
            String status = GetBeerLightStatus(); 
            if(cachedStatus.Equals(status))
            {
                icon.Display(status, true, false);
            }
            else
            {
                icon.Display(status, true, true);
                cachedStatus = status; 
            }
        }

        static void Log(Exception message, TextWriter write, Boolean close)
        {
            write.Write("\r\nLogger Entry : ");
            write.Write(DateTime.Now.ToLongTimeString());
            write.Write(" : "); 
            write.WriteLine(DateTime.Now.ToLongDateString());
            write.WriteLine(" :");
            write.WriteLine(" :{0}", message);
            if(close == true)
            {
                write.WriteLine("APPLICATION TIMEOUT : CLOSING NOW"); 
            }
            write.WriteLine("--------------------------------"); 
        }

        
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            icon.Display("search", true, false); 
            TickEvent(null, null); 
            System.Timers.Timer tick = new System.Timers.Timer();
            tick.Elapsed += new ElapsedEventHandler(TickEvent);
            tick.Interval = 30000;
            tick.Enabled = true;

            Application.Run();
        }
    }
}
