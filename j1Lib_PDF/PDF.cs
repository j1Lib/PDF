using System;
using System.IO;
using System.Windows.Forms;

namespace j1Lib
{
    public static class PDF
    {
        public static void PDFSupport(this WebBrowser target, bool value)
        {
            if (value)
            {
                j1Lib_PDF.PDFSupport pdf = new j1Lib_PDF.PDFSupport();
                target.setCache("PDFSupport", pdf);
                target.Navigating += pdf.PDFSupport_;
                target.DocumentCompleted += pdf.PDFSupport_;
            }
            else
            {
                j1Lib_PDF.PDFSupport pdf = (j1Lib_PDF.PDFSupport)target.getCache("PDFSupport", null);
                target.Navigating -= pdf.PDFSupport_;
                target.DocumentCompleted -= pdf.PDFSupport_;
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