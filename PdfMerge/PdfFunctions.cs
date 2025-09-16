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
            try
            {
                if (files == null || files.Count == 0)
                {
                    throw new ArgumentException("No files provided for merging");
                }

                var pdf = new PdfDocument(new PdfWriter(ouputPath));
                var merger = new PdfMerger(pdf);

                int totalPages = 0;
                foreach (var file in files)
                {
                    if (!System.IO.File.Exists(file))
                    {
                        throw new System.IO.FileNotFoundException($"File not found: {file}");
                    }

                    var pdfDocument = new PdfDocument(new PdfReader(file));
                    int pageCount = pdfDocument.GetNumberOfPages();
                    merger.Merge(pdfDocument, 1, pageCount);
                    totalPages += pageCount;
                    pdfDocument.Close();
                }

                pdf.Close();
                
                // Simple verification that the file was created
                if (!System.IO.File.Exists(ouputPath))
                {
                    throw new Exception("Output file was not created");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"PDF merge failed: {ex.Message}", ex);
            }
        }

    }
}
