using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.Collections.Generic;

namespace PdfMerge
{
    class PdfFunctions
    {
        public static void Merge(List<string> files, String outputPath)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    throw new ArgumentException("No files provided for merging");
                }

                using (var pdf = new PdfDocument(new PdfWriter(outputPath)))
                {
                    var merger = new PdfMerger(pdf);

                    int totalPages = 0;
                    foreach (var file in files)
                    {
                        if (!System.IO.File.Exists(file))
                        {
                            throw new System.IO.FileNotFoundException($"File not found: {file}");
                        }

                        using (var pdfDocument = new PdfDocument(new PdfReader(file)))
                        {
                            int pageCount = pdfDocument.GetNumberOfPages();
                            merger.Merge(pdfDocument, 1, pageCount);
                            totalPages += pageCount;
                        }
                    }
                }

                // Simple verification that the file was created
                if (!System.IO.File.Exists(outputPath))
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
