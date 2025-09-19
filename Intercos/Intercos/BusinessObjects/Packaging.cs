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
  [DbBo(DescriptionProperties = [nameof(Type), nameof(Name)])]
  public class Package : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual PackageType Type { get; set; }
    public virtual Vendor PotentialVendor { get; set; }
    public virtual decimal EstCost { get; set; }
    public virtual string Description { get; set; }
    public virtual decimal EffectiveCost { get; set; }
    public virtual Vendor EffectiveVendor { get; set; }

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
