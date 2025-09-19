using Intercos.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;

namespace Intercos.BusinessObjects
{
  // [WsBo]
  // [CustomBo<>]
  // [InMemoryBo]
  [DbBo(DescriptionProperties = [nameof(Description)])]
  public class Ingredient : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual string Description { get; set; }
    public virtual Material Material { get; set; }
    public virtual decimal Percentage { get; set; }
    public virtual FormulaPhase Phase { get; set; }
    public virtual decimal Quantity { get; set; }
    public override string GetId()
    {
      return Id.ToString();
    }

    public override void SetId(string id)
    {
      Id = int.Parse(id);
    }

  }

}
