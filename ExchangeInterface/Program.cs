
namespace ExchangeInterface
{


    static class Program
    {


         /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            bool bShowWindow = false;

            if (bShowWindow)
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run(new Form1());
            } // End if (bShowWindow)

            ExchangeHelper.GetLastestBody();
        } // End Sub Main


    } // End Class Program 


} // End Namespace ExchangeInterface 
