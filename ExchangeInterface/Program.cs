
using System;
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
            bool bShowWindow = false;

            if (bShowWindow)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            } // End if (bShowWindow)

            CleanSVG();
        } // End Sub Main


        public class XmlTextWriterIndentedStandaloneNo : System.Xml.XmlTextWriter
        {

            public bool bStandAlone = false;
            public bool bWriteStartDocument = true;
            public bool bOmitEncodingAndStandAlone = true;


            public XmlTextWriterIndentedStandaloneNo(System.IO.TextWriter w)
                : base(w)
            {
                Formatting = System.Xml.Formatting.Indented;
            } // End Constructor 


            public XmlTextWriterIndentedStandaloneNo(string strFileName, System.Text.Encoding teEncoding)
                : base(strFileName, teEncoding)
            {
                Formatting = System.Xml.Formatting.Indented;
            } // End Constructor 


            public XmlTextWriterIndentedStandaloneNo(System.IO.Stream w, System.Text.Encoding teEncoding)
                : base(w, teEncoding)
            {
                Formatting = System.Xml.Formatting.Indented;
            } // End Constructor 


            public override void WriteStartDocument(bool standalone)
            {
                if (bWriteStartDocument)
                {
                    if (bOmitEncodingAndStandAlone)
                    {
                        this.WriteProcessingInstruction("xml", "version='1.0'");
                        return;
                    } // End if (bOmitEncodingAndStandAlone) 

                    base.WriteStartDocument(bStandAlone);
                }

            } // End Sub WriteStartDocument 


            public override void WriteStartDocument()
            {
                // Suppress by ommitting WriteStartDocument
                if (bWriteStartDocument)
                {

                    if (bOmitEncodingAndStandAlone)
                    {
                        this.WriteProcessingInstruction("xml", "version='1.0'");
                        return;
                    } // End if (bOmitEncodingAndStandAlone)

                    base.WriteStartDocument(bStandAlone);
                    // False: Standalone="no"
                } // End if (bWriteStartDocument)

            } // End Sub WriteStartDocument 


        } // End Class XmlTextWriterIndentedStandaloneNo 


        public static void CleanSVG()
        {
            string strInputFile = @"D:\Stefan.Steiger\Documents\visual studio 2013\Projects\VariousRoundingFunctions\SvgBasics\Switzerland_regions_orig.svg";

            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.XmlResolver = null;
            //xdoc.Load(strInputFile);

            // http://stackoverflow.com/questions/1874132/how-to-remove-all-comment-tags-from-xmldocument
            System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.XmlResolver = null;
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(strInputFile, settings))
            {
                xdoc.Load(reader);
                reader.Close();
            } // End Using reader

            // http://stackoverflow.com/questions/561822/xpath-on-an-xml-document-with-namespace
            // var xo = xdoc.SelectNodes(@"//*[namespace-uri() = ""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""][local-name() = ""nodetypes""]", names);
            // var xo = xdoc.SelectNodes(@"//sodipodi:*", names);

            //RemoveComments(xdoc);


            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();
            ls.Add("sodipodi:insensitive");
            ls.Add("sodipodi:nodetypes");
            ls.Add("sodipodi:type");
            ls.Add("sodipodi:linespacing");
            ls.Add("sodipodi:role");
            ls.Add("sodipodi:cx");
            ls.Add("sodipodi:cy");
            ls.Add("sodipodi:rx");
            ls.Add("sodipodi:ry");
            ls.Add("sodipodi:version");
            ls.Add("sodipodi:docname");

            ls.Add("inkscape:collect");
            ls.Add("inkscape:connector-curvature");
            ls.Add("inkscape:export-filename");
            ls.Add("inkscape:export-xdpi");
            ls.Add("inkscape:export-ydpi");
            ls.Add("inkscape:groupmode");
            ls.Add("inkscape:version");
            ls.Add("inkscape:output_extension");
            //ls.Add("inkscape:label");

            ls.Add("xml:space"); // xml:space="preserve" 
            
            RemoveAttributes(xdoc, ls);


            ls.Clear();
            ls.Add("sodipodi:namedview");
            ls.Add("inkscape:perspective");
            ls.Add("dft:metadata");
            RemoveElements(xdoc, ls);


            ls.Clear();
            ls.Add("cc");
            ls.Add("dc");
            ls.Add("rdf");
            ls.Add("sodipodi");
            RemoveNamespaces(xdoc, ls);


            ls.Clear();
            ls = null;


            string strBasePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),"../../");
            strBasePath = System.IO.Path.GetFullPath(strBasePath);


            System.Xml.XmlWriterSettings xwsSettings = new System.Xml.XmlWriterSettings();
            xwsSettings.Indent = false;
            xwsSettings.NewLineChars = String.Empty;
            xwsSettings.Encoding = System.Text.Encoding.UTF8;
            xwsSettings.OmitXmlDeclaration = true;

            xwsSettings.Indent = true;
            xwsSettings.NewLineChars = Environment.NewLine;
            xwsSettings.OmitXmlDeclaration = false;

            //using (System.Xml.XmlTextWriter wr = new System.Xml.XmlTextWriter(System.IO.Path.Combine(strBasePath, "Switzerland.xml"), System.Text.Encoding.UTF8))
            using (System.Xml.XmlWriter wr = System.Xml.XmlWriter.Create(System.IO.Path.Combine(strBasePath, "Switzerland.xml"), xwsSettings))
            //using (System.Xml.XmlWriter wr = new XmlTextWriterIndentedStandaloneNo(System.IO.Path.Combine(strBasePath, "Switzerland.xml"), System.Text.Encoding.UTF8))
            {
                //wr.Formatting = System.Xml.Formatting.None; // here's the trick !
                
                xdoc.Save(wr);
                wr.Flush();
                wr.Close();
            } // End Using wr

            using (System.Xml.XmlWriter wr = System.Xml.XmlWriter.Create(System.IO.Path.Combine(strBasePath, "Switzerland.svg"), xwsSettings))
            //using (System.Xml.XmlTextWriter wr = new System.Xml.XmlTextWriter(System.IO.Path.Combine(strBasePath, "Switzerland.svg"), System.Text.Encoding.UTF8))
            {
                //wr.Formatting = System.Xml.Formatting.None; // here's the trick !
                
                xdoc.Save(wr);
                wr.Flush();
                wr.Close();
            } // End Using wr
            
        } // End Sub CleanSVG 


        public static void RemoveComments(System.Xml.XmlDocument xdoc)
        {
            foreach (System.Xml.XmlNode thisCommentNode in xdoc.SelectNodes("//comment()"))
            {
                if (thisCommentNode.ParentNode != null)
                    thisCommentNode.ParentNode.RemoveChild(thisCommentNode);
            } // Next thisCommentNode

        } // End Sub RemoveComments
        

        public static void RemoveNamespaces(System.Xml.XmlDocument xdoc, System.Collections.Generic.List<string> ls)
        {
            foreach (string strNamespaceName in ls)
            {
                //xdoc.DocumentElement.RemoveAttribute("xmlns:sodipodi");
                xdoc.DocumentElement.RemoveAttribute("xmlns:" + strNamespaceName);
            } // Next strNamespaceName

            xdoc.DocumentElement.RemoveAttribute("xmlns:svg");
            xdoc.DocumentElement.SetAttribute("xmlns:svg", "http://www.w3.org/2000/svg");

            xdoc.DocumentElement.RemoveAttribute("xmlns:xlink");
            xdoc.DocumentElement.SetAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");

            // Needed for preserving inkscape:label
            xdoc.DocumentElement.RemoveAttribute("xmlns:inkscape");
            xdoc.DocumentElement.SetAttribute("xmlns:inkscape", "http://www.inkscape.org/namespaces/inkscape");
        } // End Sub RemoveNamespaces


        // http://stackoverflow.com/questions/7003983/how-to-remove-xmlns-attribute-from-xmldocument
        public static void RemoveElements(System.Xml.XmlDocument xdoc, System.Collections.Generic.List<string> ls)
        {
            System.Xml.XmlNamespaceManager names = new System.Xml.XmlNamespaceManager(xdoc.NameTable);
            names.AddNamespace("dft", "http://www.w3.org/2000/svg");
            names.AddNamespace("svg", "http://www.w3.org/2000/svg");
            names.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

            names.AddNamespace("sodipodi", "http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd");
            names.AddNamespace("inkscape", "http://www.inkscape.org/namespaces/inkscape");

            foreach (string strElementName in ls)
            {
                System.Xml.XmlNodeList nodeList = xdoc.SelectNodes("//" + strElementName, names);

                foreach (System.Xml.XmlNode thisNode in nodeList)
                {
                    if (thisNode.ParentNode != null)
                        thisNode.ParentNode.RemoveChild(thisNode);
                } // Next thisNode

            } // Next strElementName

        } // End Sub RemoveElements


        // http://stackoverflow.com/questions/7003983/how-to-remove-xmlns-attribute-from-xmldocument
        public static void RemoveAttributes(System.Xml.XmlDocument xdoc, System.Collections.Generic.List<string> ls)
        {
            System.Xml.XmlNamespaceManager names = new System.Xml.XmlNamespaceManager(xdoc.NameTable);
            names.AddNamespace("dft", "http://www.w3.org/2000/svg");
            names.AddNamespace("svg", "http://www.w3.org/2000/svg");
            names.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

            names.AddNamespace("sodipodi", "http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd");
            names.AddNamespace("inkscape", "http://www.inkscape.org/namespaces/inkscape");

            foreach (string strAttributeName in ls)
            {
                System.Xml.XmlNodeList xmlNodes = xdoc.SelectNodes("//*[@" + strAttributeName + "]", names);
                // Console.WriteLine(xmlNodes.Count);
                RemoveNodes(xmlNodes, strAttributeName);
            } // Next strAttributeName

        } // End Sub RemoveAttributes


        public static void RemoveNodes(System.Xml.XmlNodeList NodeList, string attributeName)
        {
            foreach (System.Xml.XmlNode thisElement in NodeList)
            {
                if (thisElement.Attributes != null) // && thisElement.Attributes[attributeName] != null)
                    thisElement.Attributes.Remove(thisElement.Attributes[attributeName]);

                // NamespaceURI: http://www.inkscape.org/namespaces/inkscape
                // Name: inkscape:connector-curvature
                // Prefix: inkscape
                // LocalName: connector-curvature

                //for (int i = 0; i < thisElement.Attributes.Count; ++i)
                //{
                //    System.Xml.XmlAttribute thisAttribute = thisElement.Attributes[i];
                //    Console.WriteLine(thisAttribute.LocalName);
                //}
                
            } // Next thisElement

        } // End Sub RemoveNodes 


        static void RunExchange()
        {
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
        } // End Sub RunExchange 


    } // End Class Program 


} // End Namespace ExchangeInterface 
