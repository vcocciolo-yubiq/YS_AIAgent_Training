using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.CodeLibraries;
using VendorAudit.WorkItems;

using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;

// https://kb.itextpdf.com/itext/creating-form-fields


namespace VendorAudit.CodeLibs
{
  public class PDFManipulate : CodeLibrary<ExecutionWI>
  {


    public void HelloWorld(string docId)
    {
        var inputStream =new MemoryStream (Context.DOCS.GetById(docId).Content);
        var outputstream =new MemoryStream ();
        
        var reader = new PdfReader(inputStream);
        var writer = new PdfWriter(outputstream);
        var pdf = new PdfDocument(reader,writer);
        var page = pdf.GetPage(1);
        //do something with the page
        pdf.Close();

        Console.WriteLine("Hello World");
    }

    public void AddField()
    {

      var inputStream =new MemoryStream (Context.DOCS.GetById("1").Content);
      var outputstream =new MemoryStream ();

      // Leggi il PDF originale e prepara l’output
      PdfReader reader = new PdfReader(inputStream);
      PdfWriter writer = new PdfWriter(outputstream);
      PdfDocument pdf = new PdfDocument(reader, writer);

      // Ottieni la prima pagina dove vuoi aggiungere il campo
      PdfPage page = pdf.GetPage(1);

      // Ottieni il modulo
      PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);

      // Definisci posizione e dimensione del campo
      Rectangle rect = new Rectangle(100, 600, 300, 20); // x, y, width, height


      PdfTextFormField field = new TextFormFieldBuilder(pdf, "FieldName").SetWidgetRectangle(new Rectangle(36, 788, 523, 18)).CreateText();
      field.SetValue("");

      // Aggiungi il campo alla pagina
      form.AddField(field, page);

      // Chiudi il PDF
      pdf.Close();

      Document doc = new Document();
      doc.FileName="";
      doc.Creator=Context.User;
      outputstream.Position = 0;
      doc.Content=outputstream.ToArray();
      Context.Item.Attachments.Attach(doc);

      Console.WriteLine("Campo di testo aggiunto con successo.");
    }

    
    public void WriteFieldPDF()
    {
      var pdf = new PdfDocument(new PdfReader(@"~/Downloads/test2.pdf"));
      var form = PdfAcroForm.GetAcroForm(pdf, true);
      var fields = form.GetField("FieldName");
      fields.SetValue("Test");
      pdf.Close();
    }
    




    public PDFManipulate(ActionContext ctx) : base(ctx)
    {
    }
  }

}
