using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomManegerApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static HttpServer server;
        [STAThread]
        static void Main()
        {
            server = new HttpServer();
            server.Start();

            Application.ApplicationExit += OnClosing;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormDangNhap());
        }

        static void OnClosing(object  sender, EventArgs e)
        {
            server?.Stop();
        }
    }
}
