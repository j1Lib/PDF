using j1Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace j1Lib_PDF
{
    public class PDFSupport
    {
        private string file = "";

        public void PDFSupport_(object sender, WebBrowserNavigatingEventArgs e)
        {
            WebBrowser sender_ = ((WebBrowser)sender);
            if (!e.Url.ToString().StartsWith("http://mozilla.github.io/pdf.js/web/viewer.html?file=") && e.Url.ToString().ToLower().EndsWith(".pdf"))
            {
                e.Cancel = true;
                if (e.Url.IsFile)
                {
                    file = PDF.atob(e.Url);
                    (sender_).Url = new Uri("http://mozilla.github.io/pdf.js/web/viewer.html?file=");
                }
                else
                {
                    (sender_).Url = new Uri("http://mozilla.github.io/pdf.js/web/viewer.html?file=" + e.Url.ToString());
                }
            }
        }

        public void PDFSupport_(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser sender_ = ((WebBrowser)sender);
            if (!string.IsNullOrEmpty(file) && e.Url.ToString().Equals("http://mozilla.github.io/pdf.js/web/viewer.html?file="))
            {
                HtmlElement script = sender_.Document.CreateElement("script");
                script.InnerHtml = "window.openPdfAsBase64=function(n){for(var o=window.atob(n),e=o.length,i=new Uint8Array(e),a=0;e>a;a++)i[a]=o.charCodeAt(a);PDFViewerApplication.open(i)}";
                sender_.Document.GetElementsByTagName("head")[0].AppendChild(script);
                sender_.Document.InvokeScript("openPdfAsBase64", new[] { file });
                file = "";
            }
        }
    }
}