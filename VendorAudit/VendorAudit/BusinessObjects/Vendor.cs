using VendorAudit.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;

namespace VendorAudit.BusinessObjects
{
  // [WsBo]
  // [CustomBo<>]
  // [InMemoryBo]
  [DbBo(DescriptionProperties = new string[] { "Name" })]
  public class Vendor : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual ICollection<Brand> Brand { get; set; }
    public virtual Category Category { get; set; }
    public virtual User ContactUser { get; set; }
    public virtual string WebSite { get; set; }
    public virtual Document AIReputationDoc { get; set; }

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
