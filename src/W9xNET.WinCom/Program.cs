using System;
using System.Windows.Forms;
using W9xNET.WinCom.Forms;

namespace W9xNET.WinCom
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
#if !DEBUG
            Application.ThreadException += (sender, error) =>
            {
                new FrmBugCheck { Message = error.Exception.Message }.ShowDialog();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
            {
                new FrmBugCheck { Message = error.ExceptionObject.ToString() }.ShowDialog();
            };

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
#endif
            //ApplicationConfiguration.Initialize();
            Application.SetCompatibleTextRenderingDefault(false);

            var instance = new W9xNET.Explorer.Program().FormEntry;
            if (instance == null) return;

            Application.Run(instance);
        }
    }
}