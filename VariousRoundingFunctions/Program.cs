
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace VariousRoundingFunctions
{


    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        } // End Sub Main 


        [Microsoft.SqlServer.Server.SqlFunction()]
        public static System.Data.SqlTypes.SqlDouble StatRound(System.Data.SqlTypes.SqlDouble val, System.Data.SqlTypes.SqlInt32 digits)
        {
            return Math.Round(val.Value, digits.Value, MidpointRounding.ToEven);
        } // End Function StatRound


        [Microsoft.SqlServer.Server.SqlFunction()]
        public static double RoundTowardNegInfinity(System.Data.SqlTypes.SqlDouble val)
        {
            System.Data.SqlTypes.SqlDouble frac = val - Math.Truncate(val.Value);
            if (frac == 0.5 || frac == -0.5)
            {
                return Math.Floor(val.Value);
            }

            return Math.Round(val.Value);
        } // End Function RoundTowardNegInfinity


        [Microsoft.SqlServer.Server.SqlFunction()]
        public static double RoundTowardZero(System.Data.SqlTypes.SqlDouble val)
        {
            System.Data.SqlTypes.SqlDouble frac = val - Math.Truncate(val.Value);
            if (frac == 0.5 || frac == -0.5)
            {
                return Math.Truncate(val.Value);
            }

            return Math.Round(val.Value);
        } // End Function RoundTowardZero


    } // End Class Program


} // End Namespace VariousRoundingFunctions
