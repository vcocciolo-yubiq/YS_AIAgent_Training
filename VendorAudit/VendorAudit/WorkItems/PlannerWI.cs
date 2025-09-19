using YubikStudioCore.Attributes;
using YubikStudioCore;
using YubikStudioCore.Documents;
using VendorAudit.BusinessObjects;
using VendorAudit.StaticRoles;
using VendorAudit.DynamicRoles;

namespace VendorAudit.WorkItems
{
  public class PlannerWI : WorkItem
  {

    public virtual DateOnly PlanDate { get; set; }
    public virtual ICollection<VendorAuditToPlan> VendorAuditToPlan { get; set; }
    public virtual Document Reputation { get; set; }
    public virtual DateOnly AuditDate { get; set; }
    public virtual AuditCategory AuditCategory { get; set; }
    public virtual Process Process { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual Country Country { get; set; }
    public virtual User AssignedAuditor { get; set; }
    public virtual User AssignedAuditorCoordinator { get; set; }

  }

}
