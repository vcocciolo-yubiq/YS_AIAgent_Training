using VendorAudit.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;

namespace VendorAudit.BusinessObjects
{

  [DbBo(DescriptionProperties = new string[] { "Phase", "Name" })]
  public class Process : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    // [Readonly(true)]
    // [Required]
    public virtual string Phase { get; set; }
    // [Readonly(true)]
    // [Required]
    public virtual string Name { get; set; }
    // [Readonly(true)]
    // [Required]
    public virtual string Description { get; set; }
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
