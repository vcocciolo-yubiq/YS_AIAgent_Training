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
  [DbBo(DescriptionProperties = new string[] { "WIId", "AuditRequirement", "AuditResponseStatus" })]
  public class AuditResponse : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual int WIId { get; set; }
    public virtual AuditRequirement AuditRequirement { get; set; }
    public virtual AuditResponseStatus AuditResponseStatus { get; set; }
    public virtual bool HNC { get; set; }
    public virtual bool ZT { get; set; }
    public virtual bool HNCZT { get; set; }
    public virtual string Observation { get; set; }
    public virtual Document DocumentEvidence { get; set; }
    public virtual string CorrectiveAction { get; set; }
    public virtual string Responsible { get; set; }
    public virtual int DaysExpire { get; set; }
    public virtual bool Verified { get; set; }



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
