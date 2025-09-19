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
using Intercos.CodeLibs;

namespace Intercos.Forms
{
  public class PackagingView : Form<PackagingWI>
  {

    public virtual TextField Description { get; set; }

    public virtual TextField Title { get; set; }
    public virtual ToggleField PackagingComplete { get; set; }
    public virtual BoLookupField<Package> PrimaryPackageObj { get; set; }
    public virtual BoLookupField<Package> SecondaryPackageObj { get; set; }

    //----------------Unbound Fields----------------
    [Unbound]
    public virtual MemoField PrimaryPackageDescription { get; set; }
    [Unbound]
    public virtual MemoField SecondaryPackageDescription { get; set; }
    [Unbound]
    public virtual BoLookupField<Vendor> PrimaryPotentialVendor { get; set; }
    [Unbound]
    public virtual BoLookupField<Vendor> SecondaryPotentialVendor { get; set; }
    [Unbound]
    public virtual DecimalField PrimaryEstCost { get; set; }

    [Unbound]
    public virtual DecimalField SecondaryEstCost { get; set; }

    //----------- Quotation Fields -----------

    [Unbound]
    public virtual TextField QuotationTitle { get; set; }
    [Unbound]
    public virtual MemoField QuotationDescription { get; set; }
    [Unbound]
    public virtual DecimalField QuotationTotalEstCost { get; set; }
    [Unbound]
    public virtual DecimalField QuotationNumProdItems { get; set; }




    public override FormPart GetLayout()
    {
      var colPrimaryPotentialVendor = Col(PrimaryPotentialVendor);
      colPrimaryPotentialVendor.CssClass = "col-4";
      var colSecondaryPotentialVendor = Col(SecondaryPotentialVendor);
      colSecondaryPotentialVendor.CssClass = "col-4";

      var colPrimaryPackageEstCost = Col(PrimaryEstCost);
      colPrimaryPackageEstCost.CssClass = "col-4";
      var colSecondaryPackageEstCost = Col(SecondaryEstCost);
      colSecondaryPackageEstCost.CssClass = "col-4";

      var colPrimary = Col(
                        PrimaryPackageDescription,
                        colPrimaryPotentialVendor,
                        colPrimaryPackageEstCost
                        );
      colPrimary.CssClass = "mb-5";


      var colSecondary = Col(
                          SecondaryPackageDescription,
                          colSecondaryPotentialVendor,
                          colSecondaryPackageEstCost
                          );

      var card1 = Card(title: "Definizione packaging",
                      Row(Col(Title)),
                      Row(Col(Description)),
                      Row(RawHtml(PackagingLib.GetSeparatorTitle("Imballaggio primario"))),
                      colPrimary,
                      Row(RawHtml("<div class='mb-7'></div>")),
                      Row(RawHtml(PackagingLib.GetSeparatorTitle("Imballaggio secondario"))),
                      colSecondary
                      );

      var card2 = Card(title: "Dettagli Quotation",
        Row(Col(QuotationTitle)),
        Row(Col(QuotationDescription))
      );
      return Flat(
        card1,
        card2
      );
    }
    public override void OnLoad()
    {
      base.OnLoad();

      PrimaryPackageDescription.Value = Context.Item.PrimaryPackageObj?.Description;
      SecondaryPackageDescription.Value = Context.Item.SecondaryPackageObj?.Description;
      PrimaryPotentialVendor.Value = Context.Item.PrimaryPackageObj?.PotentialVendor;
      SecondaryPotentialVendor.Value = Context.Item.SecondaryPackageObj?.PotentialVendor;
      PrimaryEstCost.Value = Context.Item.PrimaryPackageObj?.EstCost ?? 0;
      SecondaryEstCost.Value = Context.Item.SecondaryPackageObj?.EstCost ?? 0;

      var newContext = new ActionContext();
      var quotation = Runtime.Instance.GetItem(Context, Context.Item.QuotationId) as QuotationWI;

      if (quotation != null)
      {
        QuotationTitle.Value = quotation.Title;
        QuotationDescription.Value = quotation.Description;
        QuotationTotalEstCost.Value = quotation.EsTotalCost;
      }
    }
  }
}

