
namespace LaTrompa.WebCrap
{

    
    // Pfff, this "parser" is pure CRAP, 
    // but all the rest is either too big, also shit, and/or f*ing using LINQ...
    // From https://gist.github.com/hgarcia/561823
    public class CssParser
    {
        private System.Collections.Generic.List<string> _styleSheets;
        private System.Collections.Generic.SortedList<string, StyleClass> _scc;
        
        public System.Collections.Generic.SortedList<string, StyleClass> Styles
        {
            get { return this._scc; }
            set { this._scc = value; }
        } // End Property Styles


        public StyleClass InlineStyles
        {
            get { return this._scc["inline"]; }
        } // End Read-Only Property InlineStyles


        public CssParser() : this("")
        { }


        public CssParser(string content)
        {
            this._styleSheets = new System.Collections.Generic.List<string>();
            this._scc = new System.Collections.Generic.SortedList<string, StyleClass>();

            if (!string.IsNullOrEmpty(content))
            {
                this.AddInlineStyle(content);
            } // End if (!string.IsNullOrEmpty(content))
        } // End Constructor


        public void AddStyleSheet(string path)
        {
            this._styleSheets.Add(path);
            ProcessStyleSheet(path);
        }


        public void AddInlineStyle(string content)
        {
            this._styleSheets.Add("inline");

            ProcessStyleSheetFromString("inline { " + content + " } ");
        } // End Sub AddInlineStyle 


        public string GetStyleSheet(int index)
        {
            return this._styleSheets[index];
        }


        private void ProcessStyleSheetFromString(string content)
        {
            string strContent = CleanUp(content);
            string[] parts = strContent.Split('}');
            foreach (string s in parts)
            {
                if (CleanUp(s).IndexOf('{') > -1)
                {
                    FillStyleClass(s);
                }
            }
        }


        private void ProcessStyleSheet(string path)
        {
            string content = CleanUp(System.IO.File.ReadAllText(path));
            string[] parts = content.Split('}');
            foreach (string s in parts)
            {
                if (CleanUp(s).IndexOf('{') > -1)
                {
                    FillStyleClass(s);
                }
            }
        }


        private void FillStyleClass(string s)
        {
            StyleClass sc = null;
            string[] parts = s.Split('{');
            string styleName = CleanUp(parts[0]).Trim().ToLower();

            if (this._scc.ContainsKey(styleName))
            {
                sc = this._scc[styleName];
                this._scc.Remove(styleName);
            }
            else
            {
                sc = new StyleClass();
            }

            sc.Name = styleName;

            string[] atrs = CleanUp(parts[1]).Replace("}", "").Split(';');
            foreach (string a in atrs)
            {
                if (a.Contains(":"))
                {
                    string _key = a.Split(':')[0].Trim().ToLower();
                    if (sc.Attributes.ContainsKey(_key))
                    {
                        sc.Attributes.Remove(_key);
                    }
                    sc.Attributes.Add(_key, a.Split(':')[1].Trim().ToLower());
                }
            }
            this._scc.Add(sc.Name, sc);
        }


        private string CleanUp(string s)
        {
            string temp = s;
            string reg = "(/\\*(.|[\r\n])*?\\*/)|(//.*)";
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(reg);
            temp = r.Replace(temp, "");
            temp = temp.Replace("\r", "").Replace("\n", "");
            return temp;
        }


    } // End Class CssParser


    public class StyleClass
    {
        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private System.Collections.Generic.SortedList<string, string> _attributes = new System.Collections.Generic.SortedList<string, string>();
        public System.Collections.Generic.SortedList<string, string> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }


        public override string ToString()
        {
            string strRetValue = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in Attributes)
            {
                sb.Append(kvp.Key + ":" + kvp.Value + ";");
            } // Next kvp

            strRetValue = sb.ToString();
            sb.Length = 0;
            sb = null;

            return strRetValue;
        } // End Function ToString


    } // End Class StyleClass


} // End Namespace LaTrompa.WebCrap
