using YubikStudioCore.Forms.Fields;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Intercos.WorkItems;
using Intercos.BusinessObjects;
using System.Linq.Expressions;

namespace Intercos.Forms
{
  public class BRIEFCreate : Form<QuotationWI>
  {

    public virtual BoLookupField<Customer> Customer { get; set; }
    public virtual DateField DeliveryDL { get; set; }
    public virtual MemoField Description { get; set; }
    public virtual DecimalField EsTotalCost { get; set; }
    public virtual TextField Code { get; set; }
    public virtual IntField NumProdItems { get; set; }
    public virtual DateField QuotationDL { get; set; }
    public virtual TextField Title { get; set; }
    public virtual ToggleField HasPackaging { get; set; }
    public virtual BoLookupField<Package> PrimaryPackaging { get; set; }
    public virtual BoLookupField<Package> SecondaryPackaging { get; set; }

    //----------------------- Unbound Fields -----------------------
    [Unbound]
    public virtual MemoField PrimaryPackageDescription { get; set; }
    [Unbound]
    public virtual MemoField SecondaryPackageDescription { get; set; }


    public override FormPart GetLayout()
    {

      var separator = Row(RawHtml("<hr class='my-4 separator'>"));
      var colEsTotalCost = Col(EsTotalCost);
      colEsTotalCost.CssClass = "col-3";

      var colToggleTitle = RawHtml("<div class='form-label mt-5 mb-2'>Richiede una nuovo packaging?</div>");
      var colHasPackHtml = Col(colToggleTitle,
                              HasPackaging
                              );

      var colNumProdItems = Col(NumProdItems);
      colNumProdItems.CssClass = "col-3";

      var colQuotationDL = Col(QuotationDL);
      colQuotationDL.CssClass = "col-4";
      var colDeliveryDL = Col(DeliveryDL);
      colDeliveryDL.CssClass = "col-4";

      var colCustomer = Col(Customer);
      colCustomer.CssClass = "col-4";

      var colPrimaryPackageDescription = Col(PrimaryPackageDescription);
      colPrimaryPackageDescription.CssClass = "col-6";
      var colSecondaryPackageDescription = Col(SecondaryPackageDescription);
      colSecondaryPackageDescription.CssClass = "col-6";

      return Flat(
        Row(Col(Code)),
        Row(Col(Title)),
        Row(Col(Description)),
        Row(colCustomer),
        Row(colNumProdItems),
        Row(colEsTotalCost),
        Row(colQuotationDL),
        Row(colDeliveryDL),
        separator,
        Row(colHasPackHtml),
        Row(colPrimaryPackageDescription),
        Row(colSecondaryPackageDescription)
      );
    }
    public override void OnLoad()
    {
      base.OnLoad();
      Code.Value = "QUOT-" + Context.Item.Id.ToString("D4");
      QuotationDL.Value = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(60));
      DeliveryDL.Value = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(120));

      Code.ReadOnly = true;
      
      PrimaryPackageDescription.Value = PrimaryPackaging.Value?.Description ?? string.Empty;
      SecondaryPackageDescription.Value = SecondaryPackaging.Value?.Description ?? string.Empty;
    }
    public override void ConfigureFields()
    {
      base.ConfigureFields();

      //Visible only if HasPackaging is true
      PrimaryPackageDescription.IsVisible = Exp(() => HasPackaging.Value);
      SecondaryPackageDescription.IsVisible = Exp(() => HasPackaging.Value);

      Title.Required = true;
      Description.Required = true;
      Customer.Required = true;
      QuotationDL.Required = true;
      DeliveryDL.Required = true;
      NumProdItems.Required = true;
      EsTotalCost.Required = true;

    }
    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
    }

  }

}
