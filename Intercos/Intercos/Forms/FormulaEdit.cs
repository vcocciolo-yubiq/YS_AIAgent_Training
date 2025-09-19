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
using System.Net;

namespace Intercos.Forms
{
  public class FormulaDefiEdit : Form<QuotationWI>
  {

    public virtual MemoField Description { get; set; }
    public virtual BoLookupField<Formula> Formula { get; set; }
    public virtual TextField Title { get; set; }
    public virtual BoLookupField<Technology> Technology { get; set; }
    //---------------- Formula Definition Fields ----------------
    [Unbound]
    public virtual TextField FormulaDescription { get; set; }
    [Unbound]
    public virtual TextField FormulaCode { get; set; }
    [Unbound]
    public virtual TableField<Ingredient, IngredientRow> Ingredients { get; set; }
    [Unbound]
    public virtual DecimalField SumPerc { get; set; }
    [Unbound]
    public virtual HtmlPart FormulaStatsHtml { get; set; }
    [Unbound]
    public virtual HtmlPart Upload { get; set; }

    // [Unbound]
    // public virtual ToggleField IsFormulaNew { get; set; }
    // [Unbound]
    // public virtual TextField FormulaName { get; set; }
    // [Unbound]
    // public virtual ObjectField<Formula> FormulaNew { get; set; }
    // [Unbound]
    // public virtual MultiBoLookupField<Ingredient> IngredientsNew { get; set; }

    // [Unbound]
    // public virtual ButtonField Button { get; set; }

    public override FormPart GetLayout()
    {
      var colTitle = Col(Title);
      colTitle.CssClass = "col-10";
      var colDescription = Col(Description);
      colDescription.CssClass = "col-10";
      var colFormula = Col(Formula);
      colFormula.CssClass = "col-10";
      var colFormulaDescription = Col(FormulaDescription);
      colFormulaDescription.CssClass = "col-10";
      var colTechnology = Col(Technology);
      colTechnology.CssClass = "col-10";

      var colIngredients = Col(Ingredients);
      colIngredients.CssClass = "col-12";

      // var colFormulaStats = Col(FormulaStatsHtml);
      // colFormulaStats.CssClass = "d-flex justify-content-end align-items-end";
      FormulaStatsHtml.CssClass = "d-flex justify-content-end align-items-end";
      // var card1 = Card(
      //   "Dettagli richiesta quotazione",
      //   Row(colTitle),
      //   Row(colDescription)
      // //   Row(RawHtml("<div class='mb-3 mt-5'>Selezione se vuoi partire da una formula esistente o crearne una nuova.</div>")),
      // //   Row(colIsFormulaNew)
      // //Row(Col(Button))
      // );
      Upload.CssClass = "d-flex justify-content-between align-items-center";
      var Card2 = Card("Modifica Formula",
        Row(Col(Row(colFormula),
                Row(colTechnology),
                Row(colFormulaDescription)),
            FormulaStatsHtml),
        Row(Col(Upload)),
        Row(Col(Ingredients))
      );

      var flat = Flat(
          //card1,
          Card2
      );

      return Card2;
    }
    public override void ConfigureFields()
    {
      base.ConfigureFields();
      // Ingredients.DependsOn = [nameof(Formula)];
      // FormulaCode.DependsOn = [nameof(Formula)];
      // FormulaDescription.DependsOn = [nameof(Formula)];

      // Formula.DependsOn = [nameof(IsFormulaNew)];
      // FormulaDescription.DependsOn = [nameof(IsFormulaNew)];
      // Ingredients.DependsOn = [nameof(IsFormulaNew)];
      //FormulaName.DependsOn = [nameof(IsFormulaNew)];
      // FormulaName.DependsOn = [nameof(IsFormulaNew)];
      FormulaStatsHtml.DependsOn = [nameof(Ingredients)];


      Title.ReadOnly = true;
      Description.ReadOnly = true;
      Formula.ReadOnly = true;
      Ingredients.CanRemove = true;
      Ingredients.CanEdit = true;
      Ingredients.CanAdd = true;
      Ingredients.CanFilter = false;
      Ingredients.CanSort = false;
      Ingredients.CanEdit = true;
      Ingredients.IsEditInline = true;

      SumPerc.DependsOn = [nameof(Ingredients)];

    }

    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
      if (changedProperties.Contains(nameof(Formula)))
      {
        refreshFormulaTable();
      }

      if (changedProperties.Contains(nameof(Ingredients)))
      {
        SumPerc.Value = Ingredients.Value?.Sum(i => i.Percentage) ?? 0;
      }

    }

    public override void OnLoad()
    {
      base.OnLoad();
      //RefreshFormulaFields();
      refreshFormulaTable();
      var technologies = Context.BO.All<Technology>(0, 100)
        .OrderBy(t => t.Name)
        .ToList();
      Technology.OnGetOptions = () => technologies;

      Technology.Value = Context.Item.Technology;

      var percIngredients = Ingredients.Value
          .Sum(i => i.Percentage);

      var cost = Ingredients.Value
        .Sum(i => i.Quantity * i.Material?.Price ?? 0);

      FormulaStatsHtml.RawHTML = QuotationLib.GetTotalCost(cost);
      FormulaStatsHtml.RawHTML += QuotationLib.GetStatsHtml(percIngredients);

      Upload.RawHTML = QuotationLib.GetUploadFromExcel();
      Upload.RawHTML += QuotationLib.GetDownloadToExcel();

    }

    void refreshFormulaTable()
    {

      FormulaCode.Value = Formula.Value?.Code;
      FormulaDescription.Value = Formula.Value?.Description;

      //Count the number of ingredients in the formula
      var numIngredients = Formula.Value?.Ingredients?.Count ?? 0;

      var maxIngredients = 10;

      // If there are no ingredients, initialize with an empty list
      if (numIngredients == 0)
        Ingredients.Value = new List<Ingredient>();
      else
        Ingredients.Value = Formula.Value.Ingredients;

      // If there are fewer than 10 ingredients, add empty ingredients to fill the table
      if (numIngredients < maxIngredients)
      {
        for (int i = numIngredients; i < maxIngredients; i++)
        {
          var emptyIngredient = new Ingredient
          {
            Id = i, // Ensure unique IDs for each ingredient
            Description = string.Empty,
            Material = null,
            Percentage = 0,
            Phase = FormulaPhase.Other,
            Quantity = 0
          };
          Ingredients.Value.Add(emptyIngredient);
        }
      }

    }


  }


}
