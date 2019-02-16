using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegaChecker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.Run(new Form1());
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            XtraMessageBox.Show("Došlo je do greške u radu programa. Tekst greške:" + Environment.NewLine + e.Exception.Message /*+ Environment.NewLine + e.Exception.InnerException.Message.ToString()*/, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
