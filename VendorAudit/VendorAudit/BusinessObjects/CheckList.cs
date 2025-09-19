using VendorAudit.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;

namespace VendorAudit.BusinessObjects
{

  [DbBo(DescriptionProperties = new string[] { "AuditType", "Section", "Name" })]
  public class AuditRequirement : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual AuditType AuditType { get; set; }
    public virtual string Section { get; set; }
    public virtual string Name { get; set; }


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
