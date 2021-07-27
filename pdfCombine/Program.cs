using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Filters;
using iText.Kernel.Pdf.Xobject;

namespace pdfCombine
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputPdfPath = args[0];
            var outputPdfPath = string.Empty;

            if(args.Length > 1){
                outputPdfPath = args[1];
            }
            else {
                outputPdfPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(inputPdfPath), System.IO.Path.GetFileNameWithoutExtension(inputPdfPath) + "_combined" + System.IO.Path.GetExtension(inputPdfPath));
            }

            PdfDocument srcDoc = new PdfDocument(new PdfReader(inputPdfPath));
            PdfDocument targetDoc = new PdfDocument(new PdfWriter(outputPdfPath));
 
            float a4_width = PageSize.A4.GetWidth();
            float a4_height = PageSize.A4.GetHeight();
            
            int pages = srcDoc.GetNumberOfPages();
            
            PageSize pagesize = new PageSize(a4_width, a4_height * pages);
            targetDoc.SetDefaultPageSize(pagesize);
            PdfCanvas canvas = new PdfCanvas(targetDoc.AddNewPage());
 
            for (int i = 1; i <= pages; i++)
            {
                PdfFormXObject page = srcDoc.GetPage(i).CopyAsFormXObject(targetDoc);
                canvas.AddXObjectAt(page, 0, (a4_height * (pages - i)));   
            }

            targetDoc.Close();
            Console.WriteLine("Closed");
        }
    }
}
