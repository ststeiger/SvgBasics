using System;
using System.Collections.Generic;
using System.Windows.Forms;


using System.Net;

using ExchangeInterface.ExchangeWebService;


namespace ExchangeInterface
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // https://webmail.cor-management.ch/ews/Services.wsdl



            ExchangeInterface.ExchangeWebService.ExchangeServiceBinding binding = new ExchangeWebService.ExchangeServiceBinding();
            binding.Credentials = new NetworkCredential("username", "password", "COR");
            binding.Url = @"https://ExchangeServer.exampledomain.com/EWS/Exchange.asmx";
            binding.Url = @"https://webmail.cor-management.ch/EWS/Exchange.asmx";

            // http://stackoverflow.com/questions/19623169/exchangeservicebinding-ews-exchange-web-service
            // http://msdn.microsoft.com/en-us/library/office/exchangewebservices.exchangeservicebinding(v=exchg.150).aspx
            // Set up the binding for Exchange impersonation.
            binding.ExchangeImpersonation = new ExchangeImpersonationType();
            binding.ExchangeImpersonation.ConnectingSID = new ConnectingSIDType();
            
            // binding.ExchangeImpersonation.ConnectingSID.PrimarySmtpAddress = "USER2@exampledomain.com";

            // Create the request.
            FindItemType request = new FindItemType();
            request.ItemShape = new ItemResponseShapeType();
            request.ItemShape.BaseShape = DefaultShapeNamesType.Default;
            request.Traversal = ItemQueryTraversalType.Shallow;
            request.ParentFolderIds = new BaseFolderIdType[1];
            DistinguishedFolderIdType inbox = new DistinguishedFolderIdType();
            inbox.Id = DistinguishedFolderIdNameType.inbox;
            request.ParentFolderIds[0] = inbox;

            // Send the request and get the response by using the binding object.
            FindItemResponseType response = binding.FindItem(request);
        }
    }
}
