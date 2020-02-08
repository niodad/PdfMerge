using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfMerge
{
    class PdfFunctions
    {
        public static void Merge(List<string> files, String ouputPath)
        {

            var pdf = new PdfDocument(new PdfWriter(ouputPath));
            var merger = new PdfMerger(pdf);

            foreach (var file in files)
            {
                var pdfDocument = new PdfDocument(new PdfReader(file));
                merger.Merge(pdfDocument, 1, pdfDocument.GetNumberOfPages());
                pdfDocument.Close();
            }

            pdf.Close();
        }

    }
}
