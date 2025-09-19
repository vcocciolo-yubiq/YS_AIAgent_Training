using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VendorAudit.WorkItems;
using VendorAudit.BusinessObjects;
using YubikStudioCore.Forms.Fields;
using System.Security.Cryptography;

namespace VendorAudit.Forms
{
  public class AuditResponseRow2 : SubForm<AuditResponse>
  {
    [Unbound]
    public virtual TextField RequirementSection { get; set; }
    [Unbound]
    public virtual TextField RequirementName { get; set; }
    public virtual BoLookupField<AuditRequirement> AuditRequirement { get; set; }
    public virtual BoLookupField<AuditResponseStatus> AuditResponseStatus { get; set; }
    public virtual IntField WIId { get; set; }
    public virtual ToggleField HNCZT { get; set; }
    [Unbound]
    public virtual TextField HNCZT2 { get; set; }
    public virtual MemoField Observation { get; set; }
    public virtual MemoField CorrectiveAction { get; set; }
    public virtual TextField Responsible { get; set; }
    public virtual IntField DaysExpire { get; set; }

    public override void ConfigureFields()
    {
      base.ConfigureFields();
      Observation.Weight = 1;
      CorrectiveAction.Weight = 1;
      RequirementName.ReadOnly = true;
      HNCZT.IsVisible = false;
      HNCZT2.ReadOnly = true;
    }
    public override void OnLoad()
    {
      base.OnLoad();
      HNCZT2.Value = HNCZT.Value ? "Yes" : "No";
      RequirementSection.Value = AuditRequirement != null ? AuditRequirement.Value.Name : "nan";
      RequirementName.Value = AuditRequirement != null ? AuditRequirement.Value.Section : "nan";
    }

    public override FormPart GetLayout()
    {
      return Flat(RequirementSection, RequirementName, AuditResponseStatus, HNCZT2, Observation, CorrectiveAction, Responsible, DaysExpire);
    }
  }
}
