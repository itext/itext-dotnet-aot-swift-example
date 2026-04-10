using System.Runtime.InteropServices;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iTextNativeAOTLibrary;

public class ITextAOT
{
    [UnmanagedCallersOnly(EntryPoint = "itext_aotsample_print")]
    public static IntPtr Print(IntPtr text, IntPtr filePath)
    {
        new iText.Kernel.Utils.RegisterDefaultDiContainer();
        new iText.Forms.Util.RegisterDefaultDiContainer();

        string pdfPath = Marshal.PtrToStringAnsi(filePath);
        using (PdfDocument pdfDoc = new PdfDocument(new PdfWriter(pdfPath)))
        {
            Document doc = new Document(pdfDoc);
            doc.Add(new Paragraph(Marshal.PtrToStringAnsi(text)).SetFontSize(16));
            doc.Close();
        }

        return Marshal.StringToHGlobalAnsi(pdfPath);
    }
}
