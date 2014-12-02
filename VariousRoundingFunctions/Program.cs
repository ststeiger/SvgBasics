
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
        public static System.Data.SqlTypes.SqlDouble RoundTowardNegInfinity(System.Data.SqlTypes.SqlDouble val)
        {
            System.Data.SqlTypes.SqlDouble frac = val - Math.Truncate(val.Value);
            if (frac == 0.5 || frac == -0.5)
            {
                return Math.Floor(val.Value);
            }

            return Math.Round(val.Value);
        } // End Function RoundTowardNegInfinity


        [Microsoft.SqlServer.Server.SqlFunction()]
        public static System.Data.SqlTypes.SqlDouble RoundTowardZero(System.Data.SqlTypes.SqlDouble val)
        {
            System.Data.SqlTypes.SqlDouble frac = val - Math.Truncate(val.Value);
            if (frac == 0.5 || frac == -0.5)
            {
                return Math.Truncate(val.Value);
            }

            return Math.Round(val.Value);
        } // End Function RoundTowardZero



        
        

        // http://www.codeproject.com/KB/aspnet/ASPNET_20_Webconfig.aspx
        // http://www.codeproject.com/KB/database/Connection_Strings.aspx
        [Microsoft.SqlServer.Server.SqlFunction()]
        public static System.Data.SqlTypes.SqlString DeCrypt(System.Data.SqlTypes.SqlString SourceText)
        {
            string strReturnValue = "";

            if (string.IsNullOrEmpty(SourceText.Value))
            {
                return strReturnValue;
            } // End if (string.IsNullOrEmpty(SourceText)) 


            // Als symmetrischer Key kann irgendein Text verwendet werden.'
            string strSymmetricKey = "z67f3GHhdga78g3gZUIT(6/&ns289hsB_5Tzu6";

            using (System.Security.Cryptography.TripleDESCryptoServiceProvider Des = new System.Security.Cryptography.TripleDESCryptoServiceProvider())
            {

                using (System.Security.Cryptography.MD5CryptoServiceProvider HashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    Des.Key = HashMD5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strSymmetricKey));
                    Des.Mode = System.Security.Cryptography.CipherMode.ECB;

                    System.Security.Cryptography.ICryptoTransform desdencrypt = Des.CreateDecryptor();
                    byte[] buff = System.Convert.FromBase64String(SourceText.Value);
                    strReturnValue = System.Text.Encoding.UTF8.GetString(desdencrypt.TransformFinalBlock(buff, 0, buff.Length));
                } // End Using HashMD5

            } // End Using Des

            return strReturnValue;
        } // End Function DeCrypt


        public static System.Data.SqlTypes.SqlString Crypt(string SourceText)
        {
            string strReturnValue = "";
            // Als symmetrischer Key kann irgendein Text verwendet werden.'
            string strSymmetricKey = "z67f3GHhdga78g3gZUIT(6/&ns289hsB_5Tzu6";

            using (System.Security.Cryptography.TripleDESCryptoServiceProvider Des = new System.Security.Cryptography.TripleDESCryptoServiceProvider())
            {

                using (System.Security.Cryptography.MD5CryptoServiceProvider HashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    Des.Key = HashMD5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strSymmetricKey));
                    Des.Mode = System.Security.Cryptography.CipherMode.ECB;
                    System.Security.Cryptography.ICryptoTransform desdencrypt = Des.CreateEncryptor();
                    byte[] buff = System.Text.Encoding.UTF8.GetBytes(SourceText);

                    strReturnValue = System.Convert.ToBase64String(desdencrypt.TransformFinalBlock(buff, 0, buff.Length));
                } // End Using HashMD5

            } // End UsingDes

            return strReturnValue;
        } // End Function Crypt



    } // End Class Program


} // End Namespace VariousRoundingFunctions
