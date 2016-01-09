using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PolskaBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us");

            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show(e.Exception.ToString(), "Exception at UI Thread");
                Environment.Exit(1);
            };
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                DialogResult result = MessageBox.Show(e.ExceptionObject.ToString(), "Exception");
                Environment.Exit(1);
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Bot());
        }
    }
}