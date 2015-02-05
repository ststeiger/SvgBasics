
Namespace LaTrompa.WebCrap


    ' Pfff, this "parser" is pure CRAP, 
    ' but all the rest is either too big, also crap, or using LINQ, 
    ' which doesn't work in .NET 2.0
    ' From https://gist.github.com/hgarcia/561823
    Public Class CssParser
        Private _styleSheets As System.Collections.Generic.List(Of String)
        Private _scc As System.Collections.Generic.SortedList(Of String, StyleClass)


        Public Property Styles() As System.Collections.Generic.SortedList(Of String, StyleClass)
            Get
                Return Me._scc
            End Get
            Set(value As System.Collections.Generic.SortedList(Of String, StyleClass))
                Me._scc = value
            End Set
        End Property ' Styles


        Public ReadOnly Property InlineStyles() As StyleClass
            Get
                Return Me._scc("inline")
            End Get
        End Property ' Read-Only InlineStyles 


        Public Sub New()
            Me.New("")
        End Sub


        Public Sub New(content As String)
            Me._styleSheets = New System.Collections.Generic.List(Of String)()
            Me._scc = New System.Collections.Generic.SortedList(Of String, StyleClass)()

            If Not String.IsNullOrEmpty(content) Then
                Me.AddInlineStyle(content)
                ' End if (!string.IsNullOrEmpty(content))
            End If
        End Sub ' End Constructor


        Public Sub AddStyleSheet(path As String)
            Me._styleSheets.Add(path)
            ProcessStyleSheet(path)
        End Sub


        Public Sub AddInlineStyle(content As String)
            Me._styleSheets.Add("inline")

            ProcessStyleSheetFromString((Convert.ToString("inline { ") & content) + " } ")
        End Sub ' AddInlineStyle 


        Public Function GetStyleSheet(index As Integer) As String
            Return Me._styleSheets(index)
        End Function


        Private Sub ProcessStyleSheetFromString(content As String)
            Dim strContent As String = CleanUp(content)
            Dim parts As String() = strContent.Split("}"c)
            For Each s As String In parts
                If CleanUp(s).IndexOf("{"c) > -1 Then
                    FillStyleClass(s)
                End If
            Next
        End Sub


        Private Sub ProcessStyleSheet(path As String)
            Dim content As String = CleanUp(System.IO.File.ReadAllText(path))
            Dim parts As String() = content.Split("}"c)
            For Each s As String In parts
                If CleanUp(s).IndexOf("{"c) > -1 Then
                    FillStyleClass(s)
                End If
            Next
        End Sub


        Private Sub FillStyleClass(s As String)
            Dim sc As StyleClass = Nothing
            Dim parts As String() = s.Split("{"c)
            Dim styleName As String = CleanUp(parts(0)).Trim().ToLower()

            If Me._scc.ContainsKey(styleName) Then
                sc = Me._scc(styleName)
                Me._scc.Remove(styleName)
            Else
                sc = New StyleClass()
            End If

            sc.Name = styleName

            Dim atrs As String() = CleanUp(parts(1)).Replace("}", "").Split(";"c)
            For Each a As String In atrs
                If a.Contains(":") Then
                    Dim _key As String = a.Split(":"c)(0).Trim().ToLower()
                    If sc.Attributes.ContainsKey(_key) Then
                        sc.Attributes.Remove(_key)
                    End If
                    sc.Attributes.Add(_key, a.Split(":"c)(1).Trim().ToLower())
                End If
            Next
            Me._scc.Add(sc.Name, sc)
        End Sub


        Private Function CleanUp(s As String) As String
            Dim temp As String = s
            Dim reg As String = "(/\*(.|[" & vbCr & vbLf & "])*?\*/)|(//.*)"
            Dim r As New System.Text.RegularExpressions.Regex(reg)
            temp = r.Replace(temp, "")
            temp = temp.Replace(vbCr, "").Replace(vbLf, "")
            Return temp
        End Function


    End Class ' CssParser


    Public Class StyleClass
        Private _name As String = String.Empty
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        Private _attributes As New System.Collections.Generic.SortedList(Of String, String)()
        Public Property Attributes() As System.Collections.Generic.SortedList(Of String, String)
            Get
                Return _attributes
            End Get
            Set(value As System.Collections.Generic.SortedList(Of String, String))
                _attributes = value
            End Set
        End Property


        Public Overrides Function ToString() As String
            Dim strRetValue As String = Nothing
            Dim sb As New System.Text.StringBuilder()

            For Each kvp As System.Collections.Generic.KeyValuePair(Of String, String) In Attributes
                sb.Append(kvp.Key + ":" + kvp.Value + ";")
            Next
            ' Next kvp
            strRetValue = sb.ToString()
            sb.Length = 0
            sb = Nothing

            Return strRetValue
        End Function ' ToString


    End Class ' StyleClass


End Namespace ' LaTrompa.WebCrap
