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
using YubikStudio.UI.Components;
using Intercos.CodeLibs;
using System.Numerics;
using Intercos.Workflows;
using static YubikStudioCore.Forms.MapPart;

namespace Intercos.Forms
{
  public class FormulaDefView : Form<QuotationWI>
  {
    public virtual TextField Code { get; set; }
    public virtual BoLookupField<Customer> Customer { get; set; }
    public virtual MemoField Description { get; set; }
    public virtual BoLookupField<Formula> Formula { get; set; }
    [Unbound]
    public virtual TextField NumProdItemsTxt { get; set; }
    public virtual BoLookupField<Technology> Technology { get; set; }
    public virtual DateField DeliveryDL { get; set; }
    public virtual DecimalField EsTotalCost { get; set; }
    public virtual DateField QuotationDL { get; set; }
    public virtual ToggleField PackagingCompleted { get; set; }
    public virtual BoLookupField<Package> PrimaryPackaging { get; set; }
    public virtual BoLookupField<Package> SecondaryPackaging { get; set; }
    public virtual BoLookupField<Plant> Site { get; set; }

    //---------------- Formula Definition Fields ----------------
    [Unbound]
    public virtual MemoField FormulaDescription { get; set; }
    [Unbound]
    public virtual TextField FormulaCode { get; set; }
    [Unbound]
    public virtual TableField<Ingredient, IngredientRow> Ingredients { get; set; }
    [Unbound]
    public virtual ButtonField Button { get; set; }

    [Unbound]
    public virtual ChartPart PercentagePie { get; set; }

    [Unbound]
    public virtual HtmlPart Stats { get; set; }
    [Unbound]
    public virtual HtmlPart FormulaStats { get; set; }

    [Unbound]
    public virtual DecimalField PercProdItems { get; set; }
    [Unbound]
    public virtual DecimalField PercValue { get; set; }
    [Unbound]
    public virtual DecimalField PercIngredients { get; set; }
    [Unbound]
    public virtual DecimalField TotalMaterialCost { get; set; }

    //---------------------- Packaging Fields ----------------------
    [Unbound]
    public virtual TextField PrimaryPackageName { get; set; }
    [Unbound]
    public virtual BoLookupField<Vendor> PrimaryPotentialVendor { get; set; }
    [Unbound]
    public virtual TextField PrimaryPackageEstCost { get; set; }
    [Unbound]
    public virtual TextField SecondaryPackageName { get; set; }
    [Unbound]
    public virtual BoLookupField<Vendor> SecondaryPotentialVendor { get; set; }
    [Unbound]
    public virtual TextField SecondaryPackageEstCost { get; set; }
    [Unbound]
    public virtual HtmlPart PackagingWFStatus { get; set; }
    [Unbound]
    public virtual TextField PriEffectiveCost { get; set; }
    [Unbound]
    public virtual TextField SecEffectiveCost { get; set; }
    [Unbound]
    public virtual HtmlPart PriDiffCost { get; set; }
    [Unbound]
    public virtual HtmlPart SecDiffCost { get; set; }


    //---------------------- Tech and Site Selection Fields ----------------------
    [Unbound]
    public virtual TextField TechCategory { get; set; }
    [Unbound]
    public virtual MapPart Map { get; set; }
    [Unbound]
    public virtual TextField PlantAddress { get; set; }
    [Unbound]
    public virtual TextField PlantCode { get; set; }
    [Unbound]
    public virtual TextField PlantCountry { get; set; }
    [Unbound]
    public virtual BoLookupField<Vendor> PlantVendor { get; set; }
    [Unbound]
    public virtual TextField PlantMaxProdCap { get; set; }
    [Unbound]
    public virtual TextField Characteristics { get; set; }

    //---------------------- Unbound Fields ----------------------
    [Unbound]
    public virtual DecimalField AvgCost { get; set; }
    [Unbound]
    public virtual DecimalField NumDaysForProduction { get; set; }
    [Unbound]
    public virtual HtmlPart TimeLineHtml { get; set; }


    public override FormPart GetLayout()
    {

      var colSeparator = Col(RawHtml($"<div class='separator my-6'></div>"));
      colSeparator.CssClass = "col-10";

      //------------- Quotation Details TAB ----------------
      //var colButton = Col(Button);
      var colNumProdItems = Col(NumProdItemsTxt);
      colNumProdItems.CssClass = "col-4";
      var colButton = Col(Button);

      var colEsTotalCost = Col(EsTotalCost);
      colEsTotalCost.CssClass = "col-4";

      var colQuotationDL = Col(QuotationDL);
      colQuotationDL.CssClass = "col-4";

      var colDeliveryDL = Col(DeliveryDL);
      colDeliveryDL.CssClass = "col-4";

      colButton.CssClass = "d-flex justify-content-end mt-3 me-5";
      var tab2Col1 = Col(Row(Col(Code)),
                Row(Col(Customer)),
                Row(Col(Description)));
      // Row(colNumProdItems, colEsTotalCost),
      // Row(colQuotationDL, colDeliveryDL));
      tab2Col1.CssClass = "col-6";

      var cardCollapse = Card(title: "Maggiori dettagli",
                          Row(colNumProdItems, colEsTotalCost, AvgCost),
                          Row(colQuotationDL, colDeliveryDL, NumDaysForProduction)
                      );
      cardCollapse.CanCollapse = true;
      var colEmpty = Col(TimeLineHtml);
      colEmpty.CssClass = "col-6";

      var tab2Col2 = Col(
                        Row(colEmpty, Col(Stats))
                        );

      //------------- Formula Details TAB ----------------

      var colIngredients = Col(Ingredients);
      colIngredients.CssClass = "col-12";

      var tab1Col1 = Col(
      Row(Col(Formula)),
      Row(Col(Technology)),
      Row(Col(FormulaDescription)));
      tab1Col1.CssClass = "col-5";

      var tab1Col2 = Col(PercentagePie);
      tab1Col2.CssClass = "col-4 d-flex flex-column h-200px justify-content-start align-items-center";
      tab1Col2.Title = "Fasi di lavorazione";

      var tab1Col3 = Col(FormulaStats);
      tab1Col3.Title = "Statistiche formula";
      tab1Col3.CssClass = "col-3";

      //Show a warning message if no formula is defined
      if (Context.Item.Formula == null)
      {
        tab1Col1 = Col(
          Row(RawHtml(@$"<div class='d-flex align-items-center justify-content-center p-5'>
                          <i class=""fa-solid fa-triangle-exclamation fa-4x text-muted me-5""></i>
                          <div class=""d-flex flex-column"">
                            <h4 class=""mb-1"">Ancora nessuna formula definita per questa quotazione.</h4>
                            <span class=""text-muted"">Per procedere, è necessario creare o selezionare una formula esistente.</span>
                          </div>
                         </div>"))
        );
        tab1Col1.CssClass = "col-12";
        tab1Col2 = Col();
        tab1Col2.CssClass = "col-0";
        tab1Col3 = Col();
        tab1Col3.CssClass = "col-0";
      }

      if (Context.Item.Formula == null)
      {
        colSeparator = Col();
        colIngredients = Col();
      }

      //----------- Packaging Details TAB ----------------
      var colPrimaryDescription = Col(PrimaryPackageName);
      colPrimaryDescription.CssClass = "col-10";

      var colSecondaryDescription = Col(SecondaryPackageName);
      colSecondaryDescription.CssClass = "col-10";

      var colPrimaryVendor = Col(PrimaryPotentialVendor);
      colPrimaryVendor.CssClass = "col-10";

      var colSecondaryVendor = Col(SecondaryPotentialVendor);
      colSecondaryVendor.CssClass = "col-10";

      var colPrimaryEstCost = Col(PrimaryPackageEstCost);
      colPrimaryEstCost.CssClass = "col-10";

      var colSecondaryEstCost = Col(SecondaryPackageEstCost);
      colSecondaryEstCost.CssClass = "col-10";

      var colstatusHtml = Col(PackagingWFStatus);
      colstatusHtml.CssClass = "col-4";

      //------------- Tech and Site Selection TAB ----------------

      var colPlant = Col(Site);
      colPlant.CssClass = "col-8";

      var colMap = Col(Map);
      colMap.CssClass = "col-5";

      var col1 = Col(//Row(Col(Code)),
                     // Row(RawHtml("<div class='form-label mb-3'>Seleziona la tecnologia</div>")),
                      Row(Col(Technology)),
                      Row(Col(TechCategory)),
                      // Row(RawHtml("<div class='form-label mt-5 mb-3'>Seleziona il sito usando il Paese</div>")),
                      // Row(colCountry),
                      Row(colPlant),
                      Row(Col(PlantAddress, PlantCountry)),
                      Row(PlantVendor),
                      Row(PlantMaxProdCap),
                      Row(Characteristics));
      col1.CssClass = "col-7";



      var tabTec = Flat(
                      Row(col1, colMap)
                  //Row(PlantTable)
                  );
      if (Context.Item.Site == null)
      {
        tabTec = Col(
          Row(Col(Technology)),
          Row(Col(TechCategory)),
          Row(RawHtml(@$"<div class='d-flex align-items-center justify-content-center p-5'>
                          <i class=""fa-solid fa-triangle-exclamation fa-4x text-muted me-5""></i>
                          <div class=""d-flex flex-column"">
                            <h4 class=""mb-1"">Ancora nessun sito selezionato per questa quotazione.</h4>
                            <span class=""text-muted"">Per procedere, è necessario selezionare un sito di produzione.</span>
                          </div>
                         </div>"))
        );
      }

      //------------- TABS ----------------
      var tabFormula = Flat(
          Row(
            tab1Col1, tab1Col2, tab1Col3
          ),
          Row(colSeparator),
          Row(colIngredients)
      //Row(colButton)
      );

      var tabQuotation = Flat(
          Row(tab2Col1, tab2Col2),
          Row(cardCollapse)
      );

      var tabPackaging = Flat(
        Row(
          //Row1 Col1
          Col(
            //Col1 Row1
            Row(
              Col(
                Row(colPrimaryDescription),
                Row(colPrimaryVendor),
                Row(colPrimaryEstCost),
                Row(Col(PriEffectiveCost))
              ),
              PriDiffCost
            ),
            Row(colSeparator),
            Row(
              Col(
                Row(colSecondaryDescription),
                Row(colSecondaryVendor),
                Row(colSecondaryEstCost),
                Row(Col(SecEffectiveCost))
              ),
              SecDiffCost
            )
          ),
          //Row1 Col2
          colstatusHtml
        )
      );

      //Show technology Tab only if the item is in a technology-related stage
      var tabs = Tabs();
      if (Context.Item.Stage.Name == nameof(Quotation.TechnologySelection)
          || Context.Item.Stage.Name == nameof(Quotation.RoutingAndInvestmentValidation)
          )
      {
        tabs = Tabs(tabTec, tabFormula, tabQuotation, tabPackaging);
        tabs.TabHeaders = [
          L.T("Tecnologia"),
          L.T("DefinizioneFormula"),
          "BRIEF",
          L.T("Packaging")
        ];
      }
      else if (Context.Item.Stage.Name == nameof(Quotation.ControllingConfiguration)
              || Context.Item.Stage.Name == nameof(Quotation.SalesConfiguration)
              || Context.Item.Stage.Name == nameof(Quotation.PriceDefinition)
              || Context.Item.Stage.Name == nameof(Quotation.Archive))
      {
        tabs = Tabs(tabQuotation, tabFormula, tabTec, tabPackaging);
        tabs.TabHeaders = [
          "BRIEF",
          L.T("DefinizioneFormula"),
          L.T("Tecnologia"),
          L.T("Packaging")
        ];

      }
      else if (Context.Item.Stage.Name == nameof(Quotation.WaitingForPackaging))
      {
        tabs = Tabs(tabPackaging, tabQuotation, tabFormula, tabTec);
        tabs.TabHeaders = [
          L.T("Packaging"),
          "BRIEF",
          L.T("DefinizioneFormula"),
          L.T("Tecnologia")
        ];
      }
      else // Formula Definition Stage and Formula Validation Stage
      {
        tabs = Tabs(tabFormula, tabQuotation, tabPackaging);
        tabs.TabHeaders = [
          "Formula",
          "BRIEF",
          L.T("Packaging")
        ];
      }

      return Card(title: "Quotation", Flat(tabs));
    }

    public override void ConfigureFields()
    {
      base.ConfigureFields();

      // Formula.IsVisible = Exp(() => Context.Item.Formula != null);
      // FormulaDescription.IsVisible = Exp(() => Formula.Value != null);
      // Ingredients.IsVisible = false;
      // Technology.IsVisible = false;
      // TimeLineHtml.IsVisible = false;
      // Stats.IsVisible = false;

      //check if packaging is completed
      if (Context.Item.HasPackaging && Context.Item.PrimaryPackaging != null && Context.Item.SecondaryPackaging != null)
      {
        var packagingItem = Runtime.Instance.GetItems<PackagingWI>(Context, x => x.QuotationId == Context.Item.Id
          ).FirstOrDefault();
        PackagingCompleted.Value = packagingItem != null && packagingItem.PackagingComplete;
      }

      if (Context.Item.Formula != null && Context.Item.Formula.Ingredients != null)
      {

        //Createe stats for ingredients
        var ingredientGroups = Context.Item.Formula.Ingredients
          .GroupBy(i => i.Phase.ToString())
          .Select(g => new
          {
            Phase = g.Key,
            TotalPercentage = g.Sum(i => i.Percentage)
          })
          .OrderBy(x => x.Phase)
          .ToList();

        // Create a pie chart for the ingredient phases
        PercentagePie.Config = new ChartJsConfig
        {
          Type = "pie",
          Data = new ChartJsData
          {
            Labels = ingredientGroups.Select(g => g.Phase).ToList(),
            Datasets = [new ChartJsDataset<decimal>() {
                Data = ingredientGroups.Select(g => g.TotalPercentage).ToList(),
          }]
          },
          Options = new ChartJsOptions
          {
            Responsive = true,
            MaintainAspectRatio = false,
            Plugins = new ChartJsOptionsPlugins
            {
              Title = new ChartJsPluginTitle
              {
                Display = false,
                Text = "Phase Percentages",
              },
              Legend = new ChartJsPluginLegend
              {
                Display = true,
                Position = "left"
              },
              //TODO: add labels plugin
              // Labels = new ChartJsPluginLabels
              // {
              //   Render = "percentage",
              //   FontColor = "white",
              //   Precision = 2
              // }
            }
          }
        };
        PercentagePie.CssClass = "w-300px h-150px";
      }

      //Set the Ingredients table to read-only and disable filtering and sorting
      Ingredients.ReadOnly = true;
      Ingredients.CanFilter = false;
      Ingredients.CanSort = false;

      //TODO: The button should be enabled only if the formula is not new
      if (Formula.Value != null)
      {
        Button.CustomView = new ExecuteActionButton()
        {
          ItemId = Context.Item.Id,
          Label = "Edit",
          ActionName = "EditFormula",
          CssClass = "btnStatoInvio",
          //ExtraParams = $"{this.Categoria.Value}:{this.Ente?.Value.Nome.Replace("&", "%26")}"
        };
      }

    }
    public override void OnLoad()
    {
      base.OnLoad();

      //------------- Tech and Site Selection TAB ----------------
      TechCategory.Value = Technology.Value?.Category ?? string.Empty;
      if (Context.Item.Site != null)
      {
        //Get the country from the Site
        // Country.Value = Context.BO.All<Country>(0, 100).FirstOrDefault(x => x.Name == Context.Item.Site.Country);

        //Load Plant information in the PlantTable
        // PlantTable.Value = Context.BO.All<Plant>(0, int.MaxValue)
        // .Where(x => Context.Item.Site == null || x.Id == Context.Item.Site.Id)
        // .ToList();

        //Set the Map places based on the Site
        var places = new List<Place>();
        var place = new Place
        {
          Name = $"{Context.Item.Site.Address} ({Context.Item.Site.Name})",
          Lat = Context.Item.Site.Latitude,
          Lon = Context.Item.Site.Longitude,
          Address = Context.Item.Site.Address
        };
        places.Add(place);
        Map.Places = [.. places];

        //Set the Site details
        PlantAddress.Value = Context.Item.Site.Address;
        PlantCountry.Value = Context.Item.Site.Country;
        PlantVendor.Value = Context.BO.All<Vendor>(0, int.MaxValue)
          .FirstOrDefault(x => x.Id == Context.Item.Site.Vendor.Id);
        PlantMaxProdCap.Value = Context.Item.Site.MaxProdCap.ToString("N0");
        Characteristics.Value = Context.Item.Site.Characteristics;
      }


      //----------------- Packaging Information -----------------
      if (Context.Item.HasPackaging && Context.Item.PrimaryPackaging != null && Context.Item.SecondaryPackaging != null)
      {
        PrimaryPackageName.Value = PrimaryPackaging.Value?.Description ?? string.Empty;
        SecondaryPackageName.Value = SecondaryPackaging.Value?.Description ?? string.Empty;
        PrimaryPotentialVendor.Value = PrimaryPackaging.Value?.PotentialVendor;
        SecondaryPotentialVendor.Value = SecondaryPackaging.Value?.PotentialVendor;
        PrimaryPackageEstCost.Value = PrimaryPackaging.Value.EstCost.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);
        SecondaryPackageEstCost.Value = SecondaryPackaging.Value.EstCost.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);
        PriEffectiveCost.Value = PrimaryPackaging.Value.EffectiveCost.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);
        SecEffectiveCost.Value = SecondaryPackaging.Value.EffectiveCost.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);

        PriDiffCost.RawHTML = QuotationLib.GetDiffHtml(
          PrimaryPackaging.Value.EstCost,
          PrimaryPackaging.Value.EffectiveCost
        );
        SecDiffCost.RawHTML = QuotationLib.GetDiffHtml(
          SecondaryPackaging.Value.EstCost,
          SecondaryPackaging.Value.EffectiveCost
        );

        //--------- Packaging Workflow Status  ---------
        var packagingItem = Runtime.Instance.GetItems<PackagingWI>(Context, x => x.QuotationId == Context.Item.Id
            ).FirstOrDefault();

        if (packagingItem != null)
        {

          PackagingWFStatus.RawHTML = QuotationLib.GetPackagingWFStatusHtml(
            packagingItem.PackagingComplete,
            packagingItem.Stage.Name,
            packagingItem.AssOwner?.FullName ?? "N/A",
            packagingItem.LastModifiedDate.ToString("dd/MM/yyyy")
          );
        }

      }



      //----------------- Quotation statistics -----------------
      AvgCost.Value = Context.Item.NumProdItems != 0
        ? Context.Item.EsTotalCost / Context.Item.NumProdItems
        : 0;
      NumDaysForProduction.Value = Context.Item.DeliveryDL.DayNumber - Context.Item.QuotationDL.DayNumber;

      NumProdItemsTxt.Value = Context.Item.NumProdItems.ToString("N0");
      FormulaCode.Value = Formula.Value?.Code;
      FormulaDescription.Value = Formula.Value?.Description;

      Ingredients.Value = Formula.Value?.Ingredients ?? new List<Ingredient>();

      decimal maxProdItems = 300000; // Example threshold
      PercProdItems.Value = Context.Item.NumProdItems / maxProdItems * 100;

      decimal maxValue = 300000; // Example maximum value for the chart
      PercValue.Value = Context.Item.EsTotalCost / maxValue * 100;

      //Calculate total material cost based on ingredients and their quantities
      TotalMaterialCost.Value = Formula.Value?.Ingredients?.Sum(i => i.Quantity * i.Material?.Price ?? 0) ?? 0;

      //Soglia per numero prodotti e costo stimato
      Stats.RawHTML = QuotationLib.GetStatsHtml(
        PercProdItems.Value ?? 0,
        PercValue.Value ?? 0
      );

      //--------- Formula statistics ---------
      if (Formula.Value != null)
      {
        PercIngredients.Value = Formula.Value?.Ingredients?.Sum(i => i.Percentage) ?? 0;
        //Completamento formula percentuale
        FormulaStats.RawHTML = QuotationLib.GetStatsHtml(
          PercIngredients?.Value ?? 0
        );
        FormulaStats.RawHTML += QuotationLib.GetTechnologyStatsHtml(
          Context.Item.Technology?.Id ?? 0
        );
      }

      //--------- Timeline Statistics  ---------
      TimeLineHtml.RawHTML = QuotationLib.GetTimeline(
        Context.Item.QuotationDL,
        Context.Item.DeliveryDL
      );




    }
    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
    }
  }

}
