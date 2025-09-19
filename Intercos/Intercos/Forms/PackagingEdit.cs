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
  public class PackagingEdit : Form<PackagingWI>
  {
    public virtual IntField QuotationId { get; set; }
    public virtual TextField Title { get; set; }
    public virtual MemoField Description { get; set; }
    public virtual BoLookupField<Package> PrimaryPackageObj { get; set; }
    public virtual BoLookupField<Package> SecondaryPackageObj { get; set; }

    //--------------------- Unbound Fields ----------------
    [Unbound]
    public virtual DecimalField PrimaryEstCost { get; set; }
    [Unbound]
    public virtual MemoField PrimaryPackageDescription { get; set; }
    [Unbound]
    public virtual BoLookupField<Vendor> PrimaryPotentialVendor { get; set; }
    [Unbound]
    public virtual MemoField SecondaryPackageDescription { get; set; }
    [Unbound]
    public virtual DecimalField SecondaryEstCost { get; set; }
    [Unbound]
    public virtual BoLookupField<Vendor> SecondaryPotentialVendor { get; set; }

    public override void ConfigureFields()
    {
      base.ConfigureFields();
      Title.ReadOnly = true;
      QuotationId.ReadOnly = true;
      QuotationId.IsVisible = false;
    }
    public override FormPart GetLayout()
    {

      var colPrimaryEstCost = Col(PrimaryEstCost);
      colPrimaryEstCost.ColumnWidth = "col-3";
      var colSecondaryEstCost = Col(SecondaryEstCost);
      colSecondaryEstCost.ColumnWidth = "col-3";
      var colPrimaryPotentialVendor = Col(PrimaryPotentialVendor);
      colPrimaryPotentialVendor.ColumnWidth = "col-6";
      var colSecondaryPotentialVendor = Col(SecondaryPotentialVendor);
      colSecondaryPotentialVendor.ColumnWidth = "col-6";

      return Flat(
        // Row(Col(Title)),
        // Row(Col(Description)),
        Row(RawHtml(PackagingLib.GetSeparatorTitle("Imballaggio primario"))),
        Row(Col(PrimaryPackageDescription)),
        Row(colPrimaryPotentialVendor),
        Row(colPrimaryEstCost),
        Row(RawHtml("<div class='mb-7'></div>")),
        Row(RawHtml(PackagingLib.GetSeparatorTitle("Imballaggio secondario"))),
        Row(Col(SecondaryPackageDescription)),
        Row(colSecondaryPotentialVendor),
        Row(colSecondaryEstCost)
      );
    }

    // public Row GetSeparatorTitle(string title)
    // {
    //   return Row(RawHtml($@"<div class=""d-flex flex-column""><div class=""fs-5 fw-bold text-primary"">{title}</div><div class='separator mb-4 '></div></div>"));
    // }
    public override void OnLoad()
    {
      base.OnLoad();
      PrimaryPackageDescription.Value = PrimaryPackageObj.Value?.Description ?? string.Empty;
      SecondaryPackageDescription.Value = SecondaryPackageObj.Value?.Description ?? string.Empty;
      PrimaryEstCost.Value = PrimaryPackageObj.Value?.EstCost ?? 0;
      SecondaryEstCost.Value = SecondaryPackageObj.Value?.EstCost ?? 0;
      PrimaryPotentialVendor.Value = PrimaryPackageObj.Value?.PotentialVendor;
      SecondaryPotentialVendor.Value = SecondaryPackageObj.Value?.PotentialVendor;
    }
  }

}
