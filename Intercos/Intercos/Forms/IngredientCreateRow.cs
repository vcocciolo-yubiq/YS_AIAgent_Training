using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Intercos.WorkItems;
using Intercos.BusinessObjects;

namespace Intercos.Forms
{
  public class IngredientCreateRow : SubForm<Ingredient>
  {
 public virtual TextField Description { get; set; }
    public virtual BoLookupField<Material> Material { get; set; }
    public virtual DecimalField Percentage { get; set; }
    public virtual EnumField<FormulaPhase> Phase { get; set; }
    public virtual DecimalField Quantity { get; set; }
    [Unbound]
    public virtual TextField UoM { get; set; }
    // [Unbound]
    // public virtual ButtonField Approve { get; set; }
    // [Unbound]
    // public virtual ButtonField Reject { get; set; }
    public override void OnLoad()
    {
      base.OnLoad();
      UoM.Value = Material.Value?.UoM ?? string.Empty;

    }

    public override void ConfigureFields()
    {
      base.ConfigureFields();
      //Description.DependsOn = [nameof(Material)];
      Material.PageSize = 100;

      Material.PageSize = 100;
      Material.OnGetOptions = () =>
      {
        return [.. Context.BO.All<Material>(0, 100).OrderBy(m => m.Description)];
      };
      //Material.Required = true;
    }


    public override FormPart GetLayout()
    {

      var colPercentage = Col(Percentage);
      colPercentage.ColumnWidth = "10%";

      var colQuantity = Col(Quantity);
      colQuantity.ColumnWidth = "10%";

      var colUoM = Col(UoM);
      colUoM.ColumnWidth = "10%";

      

      return Flat(
        Material,
        Description,
        Percentage,
        Phase,
        Quantity
      //,(Approve), (Reject)
      );

    }
    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
      if (changedProperties.Contains(nameof(Material)))
      {
        // Reset the percentage and quantity when material changes
        Description.Value = Material.Value?.Description ?? string.Empty;
      }
    }
  }

}
