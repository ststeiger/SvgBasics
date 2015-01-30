
namespace ExchangeInterface
{


    static class Beautifier
    {



        public static void CleanSVG()
        {
            string strInputFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            strInputFile = System.IO.Path.Combine(strInputFile, "../../../SvgBasics/Switzerland_regions_orig.svg");
            strInputFile = System.IO.Path.GetFullPath(strInputFile);
            
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.XmlResolver = null; // hmmm - lol ?...
            //xdoc.Load(strInputFile);

            // http://stackoverflow.com/questions/1874132/how-to-remove-all-comment-tags-from-xmldocument
            System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.XmlResolver = null; // hmmm - lol ?...
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(strInputFile, settings))
            {
                xdoc.Load(reader);
                reader.Close();
            } // End Using reader


            //RemoveComments(xdoc);

            // http://stackoverflow.com/questions/561822/xpath-on-an-xml-document-with-namespace
            // var xo = xdoc.SelectNodes(@"//*[namespace-uri() = ""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""][local-name() = ""nodetypes""]", names);
            // var xo = xdoc.SelectNodes(@"//sodipodi:*", names);


            // System.Xml.XmlElement xeSelectedElement = xdoc.GetElementById("tspan6069"); // Pffffff, no fallback when no dtd - soooo lame - of course much better to just return NULL ...
            System.Xml.XmlNode xnSelectedNode = xdoc.SelectSingleNode("//*[@id='tspan6069']"); // There - wouldn't be that difficult, wouldn't it ? 
            xnSelectedNode.InnerText = "Cappucino";

            // Pfff, this "parser", which actually uses a too simple regex, is pure CRAP
            // CSS parsing is trivial ? But only if you're doing it wrong...
            LaTrompa.WebCrap.CssParser cpr = new LaTrompa.WebCrap.CssParser(xnSelectedNode.Attributes["style"].Value);
            LaTrompa.WebCrap.StyleClass InlineStyle = cpr.InlineStyles;
            InlineStyle.Attributes["fill"] = "hotpink";

            string newInlineStyle = InlineStyle.ToString();
            xnSelectedNode.Attributes["style"].Value = newInlineStyle;

            //System.Xml.XmlElement xeSelectedElement = (System.Xml.XmlElement)xnSelectedNode;
            //xeSelectedElement.SetAttribute("style", "test");

            //System.Xml.XmlAttribute style = (System.Xml.XmlAttribute)xdoc.SelectSingleNode("//*[@id='tspan6069']/@style");
            //Console.WriteLine(style);


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
            //ls.Add("inkscape:label"); // Wanna keep that

            // xml:space="preserve" 
            ls.Add("xml:space"); // Fix inkscape insanity
            
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
            xwsSettings.NewLineChars = string.Empty;
            xwsSettings.Encoding = System.Text.Encoding.UTF8;
            xwsSettings.OmitXmlDeclaration = true;

            xwsSettings.Indent = true;
            xwsSettings.NewLineChars = System.Environment.NewLine;
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

            string att = xdoc.DocumentElement.GetAttribute("xmlns:svg");
            xdoc.DocumentElement.RemoveAttribute("xmlns:svg");
            if(!string.IsNullOrEmpty(att))
                xdoc.DocumentElement.SetAttribute("xmlns:svg", att);
            att = null;

            att = xdoc.DocumentElement.GetAttribute("xmlns:xlink");
            xdoc.DocumentElement.RemoveAttribute("xmlns:xlink");
            if (!string.IsNullOrEmpty(att))
                xdoc.DocumentElement.SetAttribute("xmlns:xlink", att);
            att = null;

            // Needed for preserving inkscape:label
            att = xdoc.DocumentElement.GetAttribute("xmlns:inkscape");
            xdoc.DocumentElement.RemoveAttribute("xmlns:inkscape");
            if (!string.IsNullOrEmpty(att))
                xdoc.DocumentElement.SetAttribute("xmlns:inkscape", att);
            att = null;
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
                } // End if (bWriteStartDocument)

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


    } // End Class Program 


} // End Namespace ExchangeInterface 
