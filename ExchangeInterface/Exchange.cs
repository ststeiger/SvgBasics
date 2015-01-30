
using ExchangeInterface.ExchangeWebService;


namespace ExchangeInterface
{


    static class ExchangeHelper
    {


        static void RunExchange()
        {
            // https://webmail.cor-management.ch/ews/Services.wsdl

            ExchangeInterface.ExchangeWebService.ExchangeServiceBinding binding = new ExchangeWebService.ExchangeServiceBinding();
            binding.Credentials = new System.Net.NetworkCredential("username", "password", "domain");
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
        } // End Sub RunExchange 

        
        // http://blogs.mybridgepoint.com/checking-exchange-2010-email-using-the-exchange-web-service/
        public static BaseItemIdType[] GetInboxItemIDs(ExchangeServiceBinding esb)
        {
            BaseItemIdType[] msgIDArray = null;

            // Form the FindItem request.
            FindItemType findItemRequest = new FindItemType();
            findItemRequest.Traversal = ItemQueryTraversalType.Shallow;

            // Define which item properties are returned in the response.
            ItemResponseShapeType itemProperties = new ItemResponseShapeType();
            itemProperties.BaseShape = DefaultShapeNamesType.IdOnly;
            itemProperties.BodyType = BodyTypeResponseType.Text;

            // Add properties shape to the request.
            findItemRequest.ItemShape = itemProperties;

            // Identify which folders to search to find items.
            DistinguishedFolderIdType[] folderIDArray = new DistinguishedFolderIdType[1];
            folderIDArray[0] = new DistinguishedFolderIdType();
            folderIDArray[0].Id = DistinguishedFolderIdNameType.inbox;

            // Add folders to the request.
            findItemRequest.ParentFolderIds = folderIDArray;

            // Send the request and get the response.
            FindItemResponseType findItemResponse = esb.FindItem(findItemRequest);

            // Get the response messages.
            ResponseMessageType[] rmta = findItemResponse.ResponseMessages.Items;

            //Prepare the ItemID Array
            msgIDArray = new BaseItemIdType[rmta.Length];

            foreach (ResponseMessageType rmt in rmta)
            {
                FindItemResponseMessageType ResponseMessageType = (FindItemResponseMessageType)rmt;

                if (ResponseMessageType.ResponseClass == ResponseClassType.Success)
                {
                    FindItemParentType ItemParentType = (FindItemParentType) ResponseMessageType.RootFolder;
                    ArrayOfRealItemsType RealItemsTypeArray = (ArrayOfRealItemsType)ItemParentType.Item;

                    foreach (ItemType messagetype in RealItemsTypeArray.Items)
                    {
                        msgIDArray[0] = messagetype.ItemId;
                    } // Next messagetype

                    //foreach (MessageType messagetype in RealItemsTypeArray.Items)
                    //{
                    //    msgIDArray[0] = messagetype.ItemId;
                    //} // Next messagetype 

                } // End if (ResponseMessageType.ResponseClass == ResponseClassType.Success)

            } // Next rmt

            return msgIDArray;
        } // End Function GetInboxItemIDs


        public static ItemType[] GetMessages(ExchangeServiceBinding esb, BaseItemIdType[] msgIDArray)
        {
            GetItemType git = new GetItemType();
            ItemResponseShapeType irst = new ItemResponseShapeType();
            irst.BaseShape = DefaultShapeNamesType.AllProperties;
            irst.IncludeMimeContent = false;
            git.ItemShape = irst;
            git.ItemIds = msgIDArray;

            GetItemResponseType responsetype = esb.GetItem(git);

            return ((ItemInfoResponseMessageType)((ArrayOfResponseMessagesType)responsetype.ResponseMessages).Items[0]).Items.Items;
        } // End Function GetMessages


        public static ExchangeServiceBinding GetExchangeServiceBindingObject(string user, string password, string domain, string exchangehost)
        {
            ExchangeServiceBinding esb = new ExchangeServiceBinding();
            esb.Credentials = new System.Net.NetworkCredential(user, password, domain);
            esb.Url = "https://" + exchangehost + "/EWS/Exchange.asmx";
            
            return esb;
        } // End Function GetExchangeServiceBindingObject

        
        public static string GetLastestBody()
        {
            //ExchangeServiceBinding esb = GetExchangeServiceBindingObject("validechangeuser@validemailaddress.com", "WouldntYouLikeToKnow2?", "youractivedirectorydomain.local", "yourexchangeserver.com");
            ExchangeServiceBinding esb = GetExchangeServiceBindingObject("username", "password", "youractivedirectorydomain.local", "webmail.cor-management.ch");
            BaseItemIdType[] ids = GetInboxItemIDs(esb);
            ItemType[] messages = GetMessages(esb, ids);
            return messages[0].Body.Value;
        } // End Function GetLastestBody
        

    } // End Class Program 


} // End Namespace ExchangeInterface 
