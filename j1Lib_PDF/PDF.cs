using System;
using System.IO;
using System.Windows.Forms;

namespace j1Lib
{
    public static class PDF
    {
        public static void PDFSupport(this WebBrowser target, bool value)
        {
            string file = "";
            if (value)
            {
                target.Navigating += (sender, e) =>
                {
                    WebBrowser sender_ = ((WebBrowser)sender);
                    if (!e.Url.ToString().StartsWith("http://mozilla.github.io/pdf.js/web/viewer.html?file=") && e.Url.ToString().ToLower().EndsWith(".pdf"))
                    {
                        e.Cancel = true;
                        if (e.Url.IsFile)
                        {
                            file = atob(e.Url);
                            (sender_).Url = new Uri("http://mozilla.github.io/pdf.js/web/viewer.html?file=");
                        }
                        else
                        {
                            (sender_).Url = new Uri("http://mozilla.github.io/pdf.js/web/viewer.html?file=" + e.Url.ToString());
                        }
                    }
                };
                target.DocumentCompleted += (sender, e) =>
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
                };
            }
        }

        public static string atob(Uri path)
        {
            using (FileStream reader = new FileStream(path.LocalPath, FileMode.Open))
            {
                byte[] buffer = new byte[reader.Length];
                reader.Read(buffer, 0, (int)reader.Length);
                return Convert.ToBase64String(buffer);
            }
        }
    }
}