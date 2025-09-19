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
  public class FormulaDefCreateFromExisting : Form<QuotationWI>
  {
    public virtual MemoField Description { get; set; }

    public virtual TextField Title { get; set; }

    public virtual BoLookupField<Technology> Technology { get; set; }


    //---------------- Formula Definition Fields ----------------
    [Unbound]
    public virtual TextField FormulaDescription { get; set; }
    [Unbound]
    public virtual TextField FormulaCode { get; set; }
    [Unbound]
    public virtual TableField<Ingredient, IngredientTxt> Ingredients { get; set; }
    [Unbound]
    public virtual BoLookupField<Formula> FormulaNew { get; set; }
    [Unbound]
    public virtual HtmlPart FormulaStatsHtml { get; set; }
    [Unbound]
    public virtual HtmlPart Upload { get; set; }

    public override FormPart GetLayout()
    {

      var colFormula = Col(FormulaNew);
      colFormula.CssClass = "col-10";
      var colFormulaDescription = Col(FormulaDescription);
      colFormulaDescription.CssClass = "col-10";
      var colTechnology = Col(Technology);
      colTechnology.CssClass = "col-10";

      FormulaStatsHtml.CssClass = " d-flex justify-content-end align-items-end";
      Upload.CssClass = "d-flex justify-content-between align-items-center";

      var Card2 = Flat(
        Row(Col(Row(colFormula),
                Row(colTechnology),
                Row(colFormulaDescription)),
            FormulaStatsHtml),
        Row(Col(Upload)),
        Row(Col(Ingredients))
      );

      var flat = Card("Crea formula da esistente",
          //card1,
          Card2
      );

      return Card2;
    }
    public override void ConfigureFields()
    {
      base.ConfigureFields();
      Ingredients.DependsOn = [nameof(FormulaNew)];
      FormulaDescription.DependsOn = [nameof(FormulaNew)];
      FormulaStatsHtml.DependsOn = [nameof(FormulaNew)];

      Title.ReadOnly = true;

      Description.ReadOnly = true;

      Ingredients.ReadOnly = true;
      Ingredients.CanFilter = false;
      Ingredients.CanSort = false;

    }

    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
      if (changedProperties.Contains(nameof(FormulaNew)))
      {
        refreshFormulaTable();

        var percIngredients = Ingredients.Value
          .Sum(i => i.Percentage);

        var cost = Ingredients.Value
          .Sum(i => i.Quantity * i.Material?.Price ?? 0);

        FormulaStatsHtml.RawHTML = QuotationLib.GetStatsHtml(percIngredients);
        FormulaStatsHtml.RawHTML += QuotationLib.GetTotalCost(cost);
        // FormulaStatsHtml.RawHTML += QuotationLib.GetUploadFromExcel();

        Upload.RawHTML = QuotationLib.GetUploadFromExcel();
        Upload.RawHTML += QuotationLib.GetDownloadToExcel();
      }

    }

    public override void OnLoad()
    {
      base.OnLoad();
      var formulas = Context.BO.All<Formula>(0, 100)
        .OrderBy(f => f.Code)
        .ToList();
      FormulaNew.OnGetOptions = () => formulas;

      var technologies = Context.BO.All<Technology>(0, 100)
        .OrderBy(t => t.Name)
        .ToList();
      Technology.OnGetOptions = () => technologies;


      FormulaStatsHtml.RawHTML = QuotationLib.GetTotalCost(0);
      FormulaStatsHtml.RawHTML += QuotationLib.GetStatsHtml(0);
      // FormulaStatsHtml.RawHTML += QuotationLib.GetUploadFromExcel();

      Upload.RawHTML = QuotationLib.GetUploadFromExcel();
      Upload.RawHTML += QuotationLib.GetDownloadToExcel();


    }

    void refreshFormulaTable()
    {
      if (FormulaNew.Value == null)
      {
        Ingredients.Value = new List<Ingredient>();
        FormulaDescription.Value = string.Empty;
        FormulaCode.Value = string.Empty;
        return;
      }
      FormulaCode.Value = FormulaNew.Value?.Code;
      FormulaDescription.Value = FormulaNew.Value?.Description;

      Ingredients.Value = FormulaNew.Value?.Ingredients ?? new List<Ingredient>();

    }

  }

}
