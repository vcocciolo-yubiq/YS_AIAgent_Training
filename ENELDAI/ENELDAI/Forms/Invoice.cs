using YubikStudioCore.Forms.Fields;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ENELDAI.WorkItems;
using ENELDAI.BusinessObjects;
using System.Diagnostics;
using ENELDAI.CodeLibs;
using Microsoft.VisualBasic;
using YubikStudioCore.Views;
using Azure.AI.DocumentIntelligence;

namespace ENELDAI.Forms
{
  public class FrmInvoice : Form<TestDAIWI>
  {

    public virtual BoLookupField<Locale> Locale { get; set; }
    public virtual BoLookupField<Invoice> Invoice { get; set; }

    [Unbound]
    public virtual TextField CustomerAddress { get; set; }
    [Unbound]
    public virtual TextField CustomerId { get; set; }
    [Unbound]
    public virtual TextField CustomerName { get; set; }
    [Unbound]
    public virtual TextField CustomerTaxId { get; set; }
    [Unbound]
    public virtual DateField InvoiceDate { get; set; }
    [Unbound]
    public virtual TextField InvoiceId { get; set; }
    [Unbound]
    public virtual DecimalField SubTotal { get; set; }
    [Unbound]
    public virtual DecimalField TotalTax { get; set; }
    [Unbound]
    public virtual DecimalField InvoiceTotal { get; set; }
    [Unbound]
    public virtual TextField CurrencySymbol { get; set; }
    [Unbound]
    public virtual TextField CurrencyCode { get; set; }
    [Unbound]
    public virtual TextField VendorAddress { get; set; }
    [Unbound]
    public virtual TextField VendorId { get; set; }
    [Unbound]
    public virtual TextField VendorName { get; set; }
    [Unbound]
    public virtual TextField VendorTaxId { get; set; }
    [Unbound]
    public virtual TableField<AIRetField, AIRetFieldRow> AIRetFields { get; set; }
    public virtual YubikStudioCore.Forms.Fields.DocumentField Document { get; set; }
    [Unbound]
    public virtual ButtonField Recognize { get; set; }

    public override void ConfigureFields()
    {
      base.ConfigureFields();
      Document.Required = true;
      Locale.Required = false;
      Recognize.Label = "Recognize Invoice";
      //Recognize.CssClass = "style=""margin-top: 25p";

      CustomerAddress.DependsOn = [nameof(Recognize)];
      CustomerAddress.Required = false;

      CustomerId.DependsOn = [nameof(Recognize)];
      CustomerId.Required = false;

      CustomerName.DependsOn = [nameof(Recognize)];
      CustomerName.Required = true;

      CustomerTaxId.DependsOn = [nameof(Recognize)];
      CustomerTaxId.Required = true;

      InvoiceDate.DependsOn = [nameof(Recognize)];
      InvoiceDate.Required = true;

      InvoiceId.DependsOn = [nameof(Recognize)];
      InvoiceId.Required = true;

      SubTotal.DependsOn = [nameof(Recognize)];
      SubTotal.Required = true;

      TotalTax.DependsOn = [nameof(Recognize)];
      TotalTax.Required = true;

      InvoiceTotal.DependsOn = [nameof(Recognize)];
      InvoiceTotal.Required = true;

      CurrencySymbol.DependsOn = [nameof(Recognize)];
      CurrencySymbol.Required = true;

      CurrencyCode.DependsOn = [nameof(Recognize)];
      CurrencyCode.Required = true;

      VendorAddress.DependsOn = [nameof(Recognize)];
      VendorAddress.Required = false;

      VendorId.DependsOn = [nameof(Recognize)];
      VendorId.Required = false;

      VendorName.DependsOn = [nameof(Recognize)];
      VendorName.Required = true;

      VendorTaxId.DependsOn = [nameof(Recognize)];
      VendorTaxId.Required = true;

      AIRetFields.ReadOnly = true;
      AIRetFields.CanSort = true;
      AIRetFields.SortingColumns = [nameof(AIRetField.Name), nameof(AIRetField.Confidence)];
      //AIRetFields.CssClass = "FullCalendar";
      AIRetFields.DependsOn = [nameof(Recognize)];
      AIRetFields.IsPaged = true;
      AIRetFields.PageSize = 20;

    }
    public override FormPart GetLayout()
    {
      var r1 = Row(Col(Document), Col(Locale),Col(Recognize));

      var r2 = Row(Col(CustomerName), Col(CustomerTaxId), Col(CustomerId), Col(CustomerAddress));
      r2.Items[0].CssClass = "col-4";
      r2.Items[1].CssClass = "col-2";
      r2.Items[2].CssClass = "col-2";
      r2.Items[3].CssClass = "col-4";


      var r3 = Row(Col(VendorName), Col(VendorTaxId), Col(VendorId), Col(VendorAddress));
      r3.Items[0].CssClass = "col-4";
      r3.Items[1].CssClass = "col-2";
      r3.Items[2].CssClass = "col-2";
      r3.Items[3].CssClass = "col-4";

      var r4 = Row(Col(InvoiceId), Col(InvoiceDate), Col(CurrencyCode), Col(CurrencySymbol));

      var r5 = Row(Col(SubTotal), Col(TotalTax), Col(InvoiceTotal));

      var r6 = Row(Col(AIRetFields));

      var r7 = Row(Col(Html($"<embed src=\"/Document/Download/{10}\"width=\"500\" height=\"375\" type=\"application/pdf\">")));


      var t1 = Flat(r1, r2, r3, r4, r5);

      var t2 = Flat(r6);

      var t3 = Flat(r7);

      var tabs = Tabs(t1, t2, t3);
      tabs.TabHeaders = ["Invoice recognition", "All Invoice Fields", "Invoice PDF"];

      return Flat(tabs);
    }
    public override void OnLoad()
    {
      base.OnLoad();
      if (Invoice.Value != null)
      {
        //VendorName.Value = Invoice.Value.Header?.VendorName;
        //CustomerName.Value = Invoice.Value.Header?.CustomerName;
        //InvoiceDate.Value = Invoice.Value.Header?.InvoiceDate;
        //InvoiceId.Value = Invoice.Value.Header?.InvoiceId;
        //SubTotal.Value = Invoice.Value.Footer?.SubTotal;
        //TotalTax.Value = Invoice.Value.Footer?.TotalTax;
        //InvoiceTotal.Value = Invoice.Value.Footer?.InvoiceTotal;
        //CurrencyCode.Value = Invoice.Value.Header?.CurrencyName;
        //CurrencySymbol.Value = Invoice.Value.Header?.CurrencySymbol;
      }
    }
    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
      if (Document.Value == null)
        return;

      var ret = Context.Lib<APInvoice>().DAIInvoke(Document.Value.Content, Locale.Value?.Code);

      CustomerAddress.Value = Context.Lib<APInvoice>().SetDAIStringField("CustomerAddress", ret);
      CustomerId.Value = Context.Lib<APInvoice>().SetDAIStringField("CustomerId", ret);
      CustomerName.Value = Context.Lib<APInvoice>().SetDAIStringField("CustomerName", ret);
      CustomerTaxId.Value = Context.Lib<APInvoice>().SetDAIStringField("CustomerTaxId", ret);
      InvoiceDate.Value = Context.Lib<APInvoice>().SetDAIDateField("InvoiceDate", ret);
      InvoiceId.Value = Context.Lib<APInvoice>().SetDAIStringField("InvoiceId", ret);
      CurrencyCode.Value = Context.Lib<APInvoice>().SetDAIStringField("CurrencyCode", ret);
      CurrencySymbol.Value = Context.Lib<APInvoice>().SetDAIStringField("CurrencySymbol", ret);
      VendorAddress.Value = Context.Lib<APInvoice>().SetDAIStringField("VendorAddress", ret);
      VendorId.Value = Context.Lib<APInvoice>().SetDAIStringField("VendorId", ret);
      VendorName.Value = Context.Lib<APInvoice>().SetDAIStringField("VendorName", ret);
      VendorTaxId.Value = Context.Lib<APInvoice>().SetDAIStringField("VendorTaxId", ret);
      SubTotal.Value = Context.Lib<APInvoice>().SetDAICurrencyField("SubTotal", ret);
      TotalTax.Value = Context.Lib<APInvoice>().SetDAICurrencyField("TotalTax", ret);
      InvoiceTotal.Value = Context.Lib<APInvoice>().SetDAICurrencyField("InvoiceTotal", ret);
      CurrencyCode.Value = Context.Lib<APInvoice>().SetDAICurrencyCodeField("InvoiceTotal", ret);
      CurrencySymbol.Value = Context.Lib<APInvoice>().SetDAICurrencySymbolField("InvoiceTotal", ret);

      var airf = new List<AIRetField>();
      foreach (var field in ret.Documents[0].Fields)
      {
        if (field.Key != "Items" && field.Key != "PaymentDetails" && field.Key != "TaxDetails")
        {
          AIRetField row = new AIRetField()
          {
            Name = field.Key,
            Value = field.Value.Content?.Replace("\n", ""),
            Confidence = (decimal)field.Value.Confidence.Value
          };
          airf.Add(row);
        }
      }
      AIRetFields.Value = airf;

    }



  }

}