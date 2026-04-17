using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PdfMerge
{
    public static class PdfFunctions
    {
        public static void Merge(IEnumerable<string> files, string outputPath)
        {
            if (files == null || !files.Any())
            {
                throw new ArgumentException("No files provided for merging");
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException("Output path cannot be null or empty", nameof(outputPath));
            }

            try
            {

                using (var pdf = new PdfDocument(new PdfWriter(outputPath)))
                {
                    var merger = new PdfMerger(pdf);

                    foreach (var file in files)
                    {
                        if (!File.Exists(file))
                        {
                            throw new FileNotFoundException($"File not found: {file}");
                        }

                        using (var pdfDocument = new PdfDocument(new PdfReader(file)))
                        {
                            int pageCount = pdfDocument.GetNumberOfPages();
                            merger.Merge(pdfDocument, 1, pageCount);
                        }
                    }
                }
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new InvalidOperationException($"PDF merge failed: {ex.Message}", ex);
            }
        }

    }
}
