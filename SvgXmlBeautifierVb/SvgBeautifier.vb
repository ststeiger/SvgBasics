
Namespace SvgXmlBeautifier


    Public Class Beautifier


        ' Beautifier.SetViewBox()
        Public Shared Sub SetViewBox()
            Dim strInputFile As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            strInputFile = System.IO.Path.Combine(strInputFile, "../../SVG_Chair.svg")
            strInputFile = System.IO.Path.GetFullPath(strInputFile)

            System.Console.WriteLine(strInputFile)

            Dim strInput As String = System.IO.File.ReadAllText(strInputFile)

            Dim content As String = SetViewBox(strInput)
            System.Console.WriteLine(content)
        End Sub


        Public Shared Function SetViewBox(strInput As String) As String
            Dim xdoc As New System.Xml.XmlDocument()
            xdoc.XmlResolver = Nothing
            ' hmmm - lol ?...
            ' http://stackoverflow.com/questions/1874132/how-to-remove-all-comment-tags-from-xmldocument
            Dim settings As New System.Xml.XmlReaderSettings()
            settings.IgnoreComments = True
            settings.XmlResolver = Nothing
            ' hmmm - lol ?...
            Using strr As New System.IO.StringReader(strInput)
                Using reader As System.Xml.XmlReader = System.Xml.XmlReader.Create(strr, settings)
                    xdoc.Load(reader)
                    reader.Close()
                End Using
                ' End Using reader
                strr.Close()
            End Using
            ' string att = xdoc.DocumentElement.GetAttribute("viewBox");


            Dim strViewBox As String = xdoc.DocumentElement.GetAttribute("viewBox")

            If Not String.IsNullOrEmpty(strViewBox) Then
                Return strInput
            End If

            Dim width As String = xdoc.DocumentElement.GetAttribute("width")
            Dim height As String = xdoc.DocumentElement.GetAttribute("height")

            If String.IsNullOrEmpty(width) Then
                Return strInput
            End If

            If String.IsNullOrEmpty(height) Then
                Return strInput
            End If


            Dim dblWidth As Double = 0
            Dim delHeight As Double = 0

            If Double.TryParse(width, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, dblWidth) AndAlso Double.TryParse(height, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, delHeight) Then
                strViewBox = String.Format("0 0 {0} {1}", CInt(dblWidth), CInt(delHeight))
                xdoc.DocumentElement.SetAttribute("viewBox", strViewBox)
            End If


            'Dim xwsSettings As New System.Xml.XmlWriterSettings()
            'xwsSettings.Indent = False
            'xwsSettings.NewLineChars = String.Empty
            'xwsSettings.Encoding = System.Text.Encoding.UTF8
            'xwsSettings.OmitXmlDeclaration = True

            'xwsSettings.Indent = True
            'xwsSettings.NewLineChars = System.Environment.NewLine
            'xwsSettings.OmitXmlDeclaration = False


            'Dim strXmlText As String = Nothing

            'Using sw As New System.IO.StringWriter()
            '    Using wr As System.Xml.XmlWriter = System.Xml.XmlWriter.Create(sw, xwsSettings)
            '        xdoc.Save(wr)
            '        wr.Flush()
            '        wr.Close()
            '    End Using ' wr

            '    strXmlText = sw.ToString()
            '    sw.Close()
            'End Using ' sw

            Return xdoc.OuterXml
        End Function


        Public Sub SetViewBoxOnFile(strInputFile As String)
            Dim xdoc As New System.Xml.XmlDocument()
            xdoc.XmlResolver = Nothing

            ' hmmm - lol ?...
            ' http://stackoverflow.com/questions/1874132/how-to-remove-all-comment-tags-from-xmldocument
            Dim settings As New System.Xml.XmlReaderSettings()
            settings.IgnoreComments = True
            settings.XmlResolver = Nothing

            Using reader As System.Xml.XmlReader = System.Xml.XmlReader.Create(strInputFile, settings)
                xdoc.Load(reader)
                reader.Close()
            End Using ' reader


            Dim strViewBox As String = xdoc.DocumentElement.GetAttribute("viewBox")

            If Not String.IsNullOrEmpty(strViewBox) Then
                Return
            End If

            Dim width As String = xdoc.DocumentElement.GetAttribute("width")
            Dim height As String = xdoc.DocumentElement.GetAttribute("height")

            If String.IsNullOrEmpty(width) Then
                Return
            End If

            If String.IsNullOrEmpty(height) Then
                Return
            End If


            Dim dblWidth As Double = 0
            Dim delHeight As Double = 0

            If Double.TryParse(width, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, dblWidth) AndAlso Double.TryParse(height, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, delHeight) Then
                strViewBox = String.Format("0 0 {0} {1}", CInt(dblWidth), CInt(delHeight))
                xdoc.DocumentElement.SetAttribute("viewBox", strViewBox)
            End If


            Dim xwsSettings As New System.Xml.XmlWriterSettings()
            xwsSettings.Indent = False
            xwsSettings.NewLineChars = String.Empty
            xwsSettings.Encoding = System.Text.Encoding.UTF8
            xwsSettings.OmitXmlDeclaration = True

            xwsSettings.Indent = True
            xwsSettings.NewLineChars = System.Environment.NewLine
            xwsSettings.OmitXmlDeclaration = False


            Using wr As System.Xml.XmlWriter = System.Xml.XmlWriter.Create(strInputFile, xwsSettings)
                'using (System.Xml.XmlTextWriter wr = new System.Xml.XmlTextWriter(System.IO.Path.Combine(strBasePath, "Switzerland.svg"), System.Text.Encoding.UTF8))
                ' wr.Formatting = System.Xml.Formatting.None ' here's the trick !

                xdoc.Save(wr)
                wr.Flush()
                wr.Close()
            End Using ' wr
        End Sub



        Public Shared Sub CleanSVG()
            Dim strInputFile As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            strInputFile = System.IO.Path.Combine(strInputFile, "../../../SvgBasics/Switzerland_regions_orig.svg")
            strInputFile = System.IO.Path.GetFullPath(strInputFile)

            Dim xdoc As New System.Xml.XmlDocument()
            xdoc.XmlResolver = Nothing
            ' hmmm - lol ?...
            'xdoc.Load(strInputFile);
            ' http://stackoverflow.com/questions/1874132/how-to-remove-all-comment-tags-from-xmldocument
            Dim settings As New System.Xml.XmlReaderSettings()
            settings.IgnoreComments = True
            settings.XmlResolver = Nothing
            ' hmmm - lol ?...
            Using reader As System.Xml.XmlReader = System.Xml.XmlReader.Create(strInputFile, settings)
                xdoc.Load(reader)
                reader.Close()
            End Using
            ' End Using reader

            'RemoveComments(xdoc);

            ' http://stackoverflow.com/questions/561822/xpath-on-an-xml-document-with-namespace
            ' var xo = xdoc.SelectNodes(@"//*[namespace-uri() = ""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""][local-name() = ""nodetypes""]", names);
            ' var xo = xdoc.SelectNodes(@"//sodipodi:*", names);


            ' System.Xml.XmlElement xeSelectedElement = xdoc.GetElementById("tspan6069"); // Pffffff, no fallback when no dtd - soooo lame - of course much better to just return NULL ...
            Dim xnSelectedNode As System.Xml.XmlNode = xdoc.SelectSingleNode("//*[@id='tspan6069']")
            ' There - wouldn't be that difficult, wouldn't it ? 
            xnSelectedNode.InnerText = "Cappucino"

            ' Pfff, this "parser", which actually uses a too simple regex, is pure CRAP
            ' CSS parsing is trivial ? But only if you're doing it wrong...
            Dim cpr As New LaTrompa.WebCrap.CssParser(xnSelectedNode.Attributes("style").Value)
            Dim InlineStyle As LaTrompa.WebCrap.StyleClass = cpr.InlineStyles
            InlineStyle.Attributes("fill") = "hotpink"

            Dim newInlineStyle As String = InlineStyle.ToString()
            xnSelectedNode.Attributes("style").Value = newInlineStyle

            'System.Xml.XmlElement xeSelectedElement = (System.Xml.XmlElement)xnSelectedNode;
            'xeSelectedElement.SetAttribute("style", "test");

            'System.Xml.XmlAttribute style = (System.Xml.XmlAttribute)xdoc.SelectSingleNode("//*[@id='tspan6069']/@style");
            'Console.WriteLine(style);


            Dim ls As New System.Collections.Generic.List(Of String)()
            ls.Add("sodipodi:insensitive")
            ls.Add("sodipodi:nodetypes")
            ls.Add("sodipodi:type")
            ls.Add("sodipodi:linespacing")
            ls.Add("sodipodi:role")
            ls.Add("sodipodi:cx")
            ls.Add("sodipodi:cy")
            ls.Add("sodipodi:rx")
            ls.Add("sodipodi:ry")
            ls.Add("sodipodi:version")
            ls.Add("sodipodi:docname")

            ls.Add("inkscape:collect")
            ls.Add("inkscape:connector-curvature")
            ls.Add("inkscape:export-filename")
            ls.Add("inkscape:export-xdpi")
            ls.Add("inkscape:export-ydpi")
            ls.Add("inkscape:groupmode")
            ls.Add("inkscape:version")
            ls.Add("inkscape:output_extension")
            'ls.Add("inkscape:label"); // Wanna keep that

            ' xml:space="preserve" 
            ls.Add("xml:space")
            ' Fix inkscape insanity
            RemoveAttributes(xdoc, ls)


            ls.Clear()
            ls.Add("sodipodi:namedview")
            ls.Add("inkscape:perspective")
            ls.Add("dft:metadata")
            RemoveElements(xdoc, ls)


            ls.Clear()
            ls.Add("cc")
            ls.Add("dc")
            ls.Add("rdf")
            ls.Add("sodipodi")
            RemoveNamespaces(xdoc, ls)


            ls.Clear()
            ls = Nothing


            Dim strBasePath As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "../../")
            strBasePath = System.IO.Path.GetFullPath(strBasePath)


            Dim xwsSettings As New System.Xml.XmlWriterSettings()
            xwsSettings.Indent = False
            xwsSettings.NewLineChars = String.Empty
            xwsSettings.Encoding = System.Text.Encoding.UTF8
            xwsSettings.OmitXmlDeclaration = True

            xwsSettings.Indent = True
            xwsSettings.NewLineChars = System.Environment.NewLine
            xwsSettings.OmitXmlDeclaration = False

            'using (System.Xml.XmlTextWriter wr = new System.Xml.XmlTextWriter(System.IO.Path.Combine(strBasePath, "Switzerland.xml"), System.Text.Encoding.UTF8))
            Using wr As System.Xml.XmlWriter = System.Xml.XmlWriter.Create(System.IO.Path.Combine(strBasePath, "Switzerland.xml"), xwsSettings)
                'using (System.Xml.XmlWriter wr = new XmlTextWriterIndentedStandaloneNo(System.IO.Path.Combine(strBasePath, "Switzerland.xml"), System.Text.Encoding.UTF8))
                'wr.Formatting = System.Xml.Formatting.None; // here's the trick !

                xdoc.Save(wr)
                wr.Flush()
                wr.Close()
            End Using ' wr

            Using wr As System.Xml.XmlWriter = System.Xml.XmlWriter.Create(System.IO.Path.Combine(strBasePath, "Switzerland.svg"), xwsSettings)
                'using (System.Xml.XmlTextWriter wr = new System.Xml.XmlTextWriter(System.IO.Path.Combine(strBasePath, "Switzerland.svg"), System.Text.Encoding.UTF8))
                'wr.Formatting = System.Xml.Formatting.None; // here's the trick !

                xdoc.Save(wr)
                wr.Flush()
                wr.Close()
            End Using ' wr

        End Sub ' CleanSVG 

        Public Shared Sub RemoveComments(xdoc As System.Xml.XmlDocument)
            For Each thisCommentNode As System.Xml.XmlNode In xdoc.SelectNodes("//comment()")
                If thisCommentNode.ParentNode IsNot Nothing Then
                    thisCommentNode.ParentNode.RemoveChild(thisCommentNode)
                End If
            Next thisCommentNode
        End Sub ' RemoveComments


        Public Shared Sub RemoveNamespaces(xdoc As System.Xml.XmlDocument, ls As System.Collections.Generic.List(Of String))
            For Each strNamespaceName As String In ls
                'xdoc.DocumentElement.RemoveAttribute("xmlns:sodipodi");
                xdoc.DocumentElement.RemoveAttribute(Convert.ToString("xmlns:") & strNamespaceName)
            Next
            ' Next strNamespaceName
            Dim att As String = xdoc.DocumentElement.GetAttribute("xmlns:svg")
            xdoc.DocumentElement.RemoveAttribute("xmlns:svg")
            If Not String.IsNullOrEmpty(att) Then
                xdoc.DocumentElement.SetAttribute("xmlns:svg", att)
            End If
            att = Nothing

            att = xdoc.DocumentElement.GetAttribute("xmlns:xlink")
            xdoc.DocumentElement.RemoveAttribute("xmlns:xlink")
            If Not String.IsNullOrEmpty(att) Then
                xdoc.DocumentElement.SetAttribute("xmlns:xlink", att)
            End If
            att = Nothing

            ' Needed for preserving inkscape:label
            att = xdoc.DocumentElement.GetAttribute("xmlns:inkscape")
            xdoc.DocumentElement.RemoveAttribute("xmlns:inkscape")
            If Not String.IsNullOrEmpty(att) Then
                xdoc.DocumentElement.SetAttribute("xmlns:inkscape", att)
            End If
            att = Nothing
        End Sub ' RemoveNamespaces


        ' http://stackoverflow.com/questions/7003983/how-to-remove-xmlns-attribute-from-xmldocument
        Public Shared Sub RemoveElements(xdoc As System.Xml.XmlDocument, ls As System.Collections.Generic.List(Of String))
            Dim names As New System.Xml.XmlNamespaceManager(xdoc.NameTable)
            names.AddNamespace("dft", "http://www.w3.org/2000/svg")
            names.AddNamespace("svg", "http://www.w3.org/2000/svg")
            names.AddNamespace("xlink", "http://www.w3.org/1999/xlink")

            names.AddNamespace("sodipodi", "http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd")
            names.AddNamespace("inkscape", "http://www.inkscape.org/namespaces/inkscape")

            For Each strElementName As String In ls
                Dim nodeList As System.Xml.XmlNodeList = xdoc.SelectNodes(Convert.ToString("//") & strElementName, names)

                For Each thisNode As System.Xml.XmlNode In nodeList
                    If thisNode.ParentNode IsNot Nothing Then
                        thisNode.ParentNode.RemoveChild(thisNode)
                    End If
                Next thisNode
            Next strElementName
        End Sub ' RemoveElements


        ' http://stackoverflow.com/questions/7003983/how-to-remove-xmlns-attribute-from-xmldocument
        Public Shared Sub RemoveAttributes(xdoc As System.Xml.XmlDocument, ls As System.Collections.Generic.List(Of String))
            Dim names As New System.Xml.XmlNamespaceManager(xdoc.NameTable)
            names.AddNamespace("dft", "http://www.w3.org/2000/svg")
            names.AddNamespace("svg", "http://www.w3.org/2000/svg")
            names.AddNamespace("xlink", "http://www.w3.org/1999/xlink")

            names.AddNamespace("sodipodi", "http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd")
            names.AddNamespace("inkscape", "http://www.inkscape.org/namespaces/inkscape")

            For Each strAttributeName As String In ls
                Dim xmlNodes As System.Xml.XmlNodeList = xdoc.SelectNodes((Convert.ToString("//*[@") & strAttributeName) + "]", names)
                ' Console.WriteLine(xmlNodes.Count);
                RemoveNodes(xmlNodes, strAttributeName)
            Next strAttributeName

        End Sub ' RemoveAttributes


        Public Shared Sub RemoveNodes(NodeList As System.Xml.XmlNodeList, attributeName As String)
            For Each thisElement As System.Xml.XmlNode In NodeList
                If thisElement.Attributes IsNot Nothing Then
                    ' && thisElement.Attributes[attributeName] != null)
                    thisElement.Attributes.Remove(thisElement.Attributes(attributeName))

                    ' NamespaceURI: http://www.inkscape.org/namespaces/inkscape
                    ' Name: inkscape:connector-curvature
                    ' Prefix: inkscape
                    ' LocalName: connector-curvature

                    'for (int i = 0; i < thisElement.Attributes.Count; ++i)
                    '{
                    '    System.Xml.XmlAttribute thisAttribute = thisElement.Attributes[i];
                    '    Console.WriteLine(thisAttribute.LocalName);
                    '}

                End If
            Next thisElement

        End Sub ' RemoveNodes 


        Public Class XmlTextWriterIndentedStandaloneNo
            Inherits System.Xml.XmlTextWriter

            Public bStandAlone As Boolean = False
            Public bWriteStartDocument As Boolean = True
            Public bOmitEncodingAndStandAlone As Boolean = True


            Public Sub New(w As System.IO.TextWriter)
                MyBase.New(w)
                Formatting = System.Xml.Formatting.Indented
            End Sub ' End Constructor 


            Public Sub New(strFileName As String, teEncoding As System.Text.Encoding)
                MyBase.New(strFileName, teEncoding)
                Formatting = System.Xml.Formatting.Indented
            End Sub ' End Constructor 


            Public Sub New(w As System.IO.Stream, teEncoding As System.Text.Encoding)
                MyBase.New(w, teEncoding)
                Formatting = System.Xml.Formatting.Indented
            End Sub ' End Constructor 


            Public Overrides Sub WriteStartDocument(standalone As Boolean)
                If bWriteStartDocument Then
                    If bOmitEncodingAndStandAlone Then
                        Me.WriteProcessingInstruction("xml", "version='1.0'")
                        Return
                    End If
                    ' End if (bOmitEncodingAndStandAlone) 
                    MyBase.WriteStartDocument(bStandAlone)
                End If
                ' End if (bWriteStartDocument)
            End Sub ' WriteStartDocument 


            Public Overrides Sub WriteStartDocument()
                ' Suppress by ommitting WriteStartDocument
                If bWriteStartDocument Then

                    If bOmitEncodingAndStandAlone Then
                        Me.WriteProcessingInstruction("xml", "version='1.0'")
                        Return
                    End If
                    ' End if (bOmitEncodingAndStandAlone)
                    ' False: Standalone="no"
                    MyBase.WriteStartDocument(bStandAlone)
                End If
                ' End if (bWriteStartDocument)
            End Sub ' WriteStartDocument  


        End Class ' XmlTextWriterIndentedStandaloneNo 


    End Class ' Beautifier 


End Namespace ' SvgXmlBeautifier 
