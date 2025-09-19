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
  public class BRIEFView : Form<QuotationWI>
  {

    public virtual TextField Code { get; set; }
    public virtual BoLookupField<Customer> Customer { get; set; }
    public virtual DateField DeliveryDL { get; set; }
    public virtual MemoField Description { get; set; }
    // use Textfile to be able to formt currency remember to save it during action
    [Unbound]
    public virtual TextField EsTotalCostTxt { get; set; }
    public virtual DateField QuotationDL { get; set; }

    public virtual ToggleField HasPackaging { get; set; }
    [Unbound]
    public virtual TextField NumProdItemsTxt { get; set; }
    public virtual IntField MaxEstTotalCost { get; set; }
    public virtual IntField MaxNumItems { get; set; }
    public virtual BoLookupField<Package> PrimaryPackaging { get; set; }
    public virtual BoLookupField<Package> SecondaryPackaging { get; set; }
    public virtual BoLookupField<Technology> Technology { get; set; }

    //----------------------- Unbound Fields -----------------------

    [Unbound]
    public virtual HtmlPart Stats { get; set; }
    [Unbound]
    public virtual MemoField PrimaryPackageDescription { get; set; }
    [Unbound]
    public virtual MemoField SecondaryPackageDescription { get; set; }
    [Unbound]
    public virtual HtmlPart TimeLineHtml { get; set; }
    [Unbound]
    public virtual DecimalField AvgCost { get; set; }
    [Unbound]
    public virtual DecimalField NumDaysForProduction { get; set; }



    public override FormPart GetLayout()
    {
      var colEsTotalCost = Col(EsTotalCostTxt);
      colEsTotalCost.CssClass = "col-4";

      var colQuotationDL = Col(QuotationDL);
      colQuotationDL.CssClass = "col-4";

      var colDeliveryDL = Col(DeliveryDL);
      colDeliveryDL.CssClass = "col-4";

      var colNumProdItems = Col(NumProdItemsTxt);
      colNumProdItems.CssClass = "col-4";

      var colCustomer = Col(Customer);
      //colCustomer.CssClass = "col-4";

      var colToggleTitle = RawHtml("<div class='form-label mt-5 mb-2'>Richiede una nuovo packaging?</div>");
      var colHasPackaging = Col(colToggleTitle,
                              HasPackaging
                              );
      colHasPackaging.CssClass = "col-4 mb-5";


      var colEmpty = Col(TimeLineHtml);
      colEmpty.CssClass = "col-6";

      var separator = Row(Col(RawHtml("<hr class='my-4 separator'>")));

      var colStats = Col(Row(colEmpty, Stats));

      var colPrimaryPackaging = Col(PrimaryPackageDescription);
      colPrimaryPackaging.CssClass = "col-6";

      var colSecondaryPackaging = Col(SecondaryPackageDescription);
      colSecondaryPackaging.CssClass = "col-6";

      var colDescr = Col(Description);
      colDescr.CssClass = "col-10 mb-5";

      var rowPackaging = Col(
                //separator,
                Row(colHasPackaging),
                Row(colPrimaryPackaging),
                Row(colSecondaryPackaging)
            );

      var colDetails = Col(
                Row(Col(Code)),
                Row(colCustomer),
                Row(colDescr)
                );

      var cardCollapse = Card(title: "Maggiori dettagli",
                      Row(colNumProdItems, colEsTotalCost, AvgCost),
                      Row(colQuotationDL, colDeliveryDL, NumDaysForProduction)
                  );
      cardCollapse.CanCollapse = true;
      //cardCollapse.CanExpand = true;

      return Card(title: "BRIEF details",
                Row(colDetails, colStats),
                Row(cardCollapse),
                Row(rowPackaging)
            );
    }
    public override void OnLoad()
    {
      base.OnLoad();

      PrimaryPackageDescription.IsVisible = Exp(() => HasPackaging.Value);
      SecondaryPackageDescription.IsVisible = Exp(() => HasPackaging.Value);

      TimeLineHtml.RawHTML = QuotationLib.GetTimeline(
        Context.Item.QuotationDL,
        Context.Item.DeliveryDL
      );

      Code.Value = "QUOT-" + Context.Item.Id.ToString("D4");
      Context.Item.Code = Code.Value;

      PrimaryPackageDescription.Value = PrimaryPackaging.Value?.Description ?? string.Empty;
      SecondaryPackageDescription.Value = SecondaryPackaging.Value?.Description ?? string.Empty;

      EsTotalCostTxt.Value = Context.Item.EsTotalCost.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);
      NumProdItemsTxt.Value = Context.Item.NumProdItems.ToString("N0");

      decimal maxProdItems = 300000; // Example threshold
      decimal percProdItems = Context.Item.NumProdItems / maxProdItems * 100;

      decimal maxValue = 300000; // Example maximum value for the chart
      decimal percValue = Context.Item.EsTotalCost / maxValue * 100;

      AvgCost.Value = Context.Item.NumProdItems != 0 ? Context.Item.EsTotalCost / Context.Item.NumProdItems : 1;
      NumDaysForProduction.Value = Context.Item.DeliveryDL.DayNumber - Context.Item.QuotationDL.DayNumber;

      Stats.RawHTML = QuotationLib.GetStatsHtml(
        percProdItems,
        percValue
      );


    }
  }

}
