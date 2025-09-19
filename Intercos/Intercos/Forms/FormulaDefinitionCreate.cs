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
  public class FormulaDefCreate : Form<QuotationWI>
  {

    public virtual TextField Code { get; set; }
    public virtual MemoField Description { get; set; }
    public virtual BoLookupField<Formula> Formula { get; set; }
    public virtual BoLookupField<Technology> Technology { get; set; }

    //---------------- Unbound Formula Definition Fields ----------------
    [Unbound]
    public virtual TextField FormulaDescription { get; set; }
    [Unbound]
    public virtual TextField FormulaCode { get; set; }
    [Unbound]
    public virtual TableField<Ingredient, IngredientCreateRow> Ingredients { get; set; }
    [Unbound]
    public virtual ButtonField Button { get; set; }
    [Unbound]
    public virtual HtmlPart Upload { get; set; }


    public override FormPart GetLayout()
    {
      //var colButton = Col(Button);
      // var colNumProdItems = Col(NumProdItems);
      // colNumProdItems.CssClass = "col-2";

      var colDescription = Col(Description);
      colDescription.CssClass = "col-6";
      var colFormula = Col(Formula);
      colFormula.CssClass = "col-6";
      var colFormulaDescription = Col(FormulaDescription);
      colFormulaDescription.CssClass = "col-6";

      Upload.CssClass = "d-flex justify-content-between align-items-center";

      return Flat(
        Row(Col(Description)),
        Row(Col(Technology)),
        Row(Col(FormulaDescription)),
        Row(Col(Upload)),
        Row(Col(Ingredients))
      );
    }
    public override void ConfigureFields()
    {
      base.ConfigureFields();

      Description.ReadOnly = true;

      Ingredients.CanFilter = false;
      Ingredients.CanSort = false;
      Ingredients.CanEdit = true;
      Ingredients.CanAdd = true;
      Ingredients.CanRemove = true;
      Ingredients.IsEditInline = true;

      Technology.PageSize = 100;

    }


    public override void OnLoad()
    {
      base.OnLoad();
      var numIngredients = 10;

      Ingredients.Value = new List<Ingredient>();
      for (int i = 0; i < numIngredients; i++)
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
        //  emptyIngredient.Id = i ; // Ensure unique IDs for each ingredient
        //   emptyIngredient.Description = (i + 1).ToString(); // Reset description for each ingredient
        // ingredientList.Add(emptyIngredient);
        Ingredients.Value.Add(emptyIngredient);

        Upload.RawHTML = QuotationLib.GetUploadFromExcel();
        Upload.RawHTML += QuotationLib.GetDownloadToExcel();
      }



    }


  }

}
