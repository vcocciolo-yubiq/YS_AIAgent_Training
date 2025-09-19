using VendorAudit.BusinessObjects;
using YubikStudioCore.Attributes;
using YubikStudioCore;
using YubikStudioCore.Documents;

namespace VendorAudit.WorkItems
{

  public class ExecutionWI : WorkItem
  {
    public virtual Vendor Vendor { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual Category Category { get; set; }
    public virtual User AssignedAuditor { get; set; }
    public virtual User AssignedAuditorCoordinator { get; set; }
    public virtual User AssignedVendorContact { get; set; }
    public virtual bool IsExternalAuditor { get; set; } = false;
    public virtual Process Process { get; set; }
    public virtual AuditCategory AuditCategory { get; set; }
    public virtual AuditType AuditType { get; set; }
    public virtual DateOnly? AuditDate { get; set; } = null;
    public virtual ICollection<AuditResponse> AuditResponse { get; set; }
    public virtual bool AuditComplete { get; set; } = false;
    public virtual DateOnly? ExecStart { get; set; } = null;
    public virtual DateOnly? ExecEdit { get; set; } = null;
    public virtual DateOnly? ExecFinish { get; set; } = null;
    public virtual decimal? RespScore { get; set; } = null;
    public virtual decimal? RespRating { get; set; } = null;
    public virtual bool RespHNCZT { get; set; } = false;
    public virtual bool RespHNC { get; set; } = false;
    public virtual bool RespZT { get; set; } = false;
    public virtual int RespNC { get; set; } = 0;
    public virtual int RespRequirements { get; set; } = 0;
    public virtual int RespTotRequirements { get; set; } = 0;
    public virtual Document VendorReport { get; set; }
    public virtual string AuditAtt4Descr { get; set; }
    public virtual string AuditAtt3Descr { get; set; }
    public virtual string AuditAtt2Descr { get; set; }
    public virtual string AuditAtt1Descr { get; set; }
    public virtual Document AuditAtt4 { get; set; }
    public virtual Document AuditAtt3 { get; set; }
    public virtual Document AuditAtt2 { get; set; }
    public virtual Document AuditAtt1 { get; set; }
    public virtual Plant Plant { get; set; }
    public virtual Document ExcelChecklistDL { get; set; }
    public virtual Document ExcelChecklistUL { get; set; }
    public virtual string AuditorFinalRemarks { get; set; }
  }
}
