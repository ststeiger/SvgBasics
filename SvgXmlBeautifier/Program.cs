
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace SvgXmlBeautifier
{


    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bShowWindow = false;
            if (bShowWindow)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }

            Beautifier.CleanSVG();
            System.Console.WriteLine(" --- Finished ! --- ");
            System.Threading.Thread.Sleep(1000);
        } // End Sub Main


    }


}
