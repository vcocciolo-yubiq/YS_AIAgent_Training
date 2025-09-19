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
  [DbBo(DescriptionProperties = [nameof(Code), nameof(Description)])]
  public class Formula : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual string Description { get; set; }
    public virtual ICollection<Ingredient> Ingredients { get; set; }
    public virtual string Version { get; set; }
    public virtual string Code { get; set; }
    public virtual ProductType ProductType { get; set; }


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
