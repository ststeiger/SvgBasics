<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SvgBasics._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    <form style="width:100%;height:100%" runat="server" ID="ReportViewerForm">
        
        <table cellspacing="0" cellpadding="0" width="100%" height="100%"><!--
            --><tr height="100%"><!--
                --><td width="100%">
                    Content
                </td><!--

            --></tr><!--
        --></table>

        <table id ="lol" width="100%" height="100%">
            <tr>
                <td>LoL</td>
            </tr>
        </table>

    </form>


    <script type="text/javascript">

        var objForm = document.querySelector("[id$='ReportViewerForm']");
        var objTable = document.querySelector("[id$='ReportViewerForm'] > table");
        
        objForm.style.removeProperty('height');
        objTable.removeAttribute('height');

    </script>

</body>
</html>
