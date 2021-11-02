using System;
using System.Windows.Forms;

namespace ShyBar
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (TrayIcon trayIcon = new TrayIcon())
            using (AppBar appBar = new AppBar())
            using (WindowMonitor windowMonitor = new WindowMonitor(appBar))
            {
                Application.Run();
            }
        }
    }
}
