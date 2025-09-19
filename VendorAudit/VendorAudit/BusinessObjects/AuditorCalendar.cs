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
  [DbBo(DescriptionProperties = new string[] { "Vendor", "Brand", "AuditType", "AuditDate" })]
  public class AuditorCalendar : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual User Auditor { get; set; }
    public virtual Vendor Vendor { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual AuditType AuditType { get; set; }
    public virtual DateOnly AuditDate { get; set; }


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
