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

namespace VendorAudit.Forms
{
  public class AuditResponseRow : SubForm<AuditResponse>
  {

    [Unbound]
    public virtual TextField RequirementSection { get; set; }
    [Unbound]
    public virtual TextField RequirementName { get; set; }
    public virtual MemoField Observation { get; set; }
    public virtual DocumentField DocumentEvidence { get; set; }
    public virtual BoLookupField<AuditRequirement> AuditRequirement { get; set; }
    public virtual BoLookupField<AuditResponseStatus> AuditResponseStatus { get; set; }
    public virtual IntField WIId { get; set; }
    public virtual ToggleField HNC { get; set; }
    public virtual ToggleField ZT { get; set; }
    public virtual MemoField CorrectiveAction { get; set; }
    public virtual MemoField Responsible { get; set; }
    public virtual IntField DaysExpire { get; set; }

    public override void ConfigureFields()
    {
      base.ConfigureFields();

      RequirementName.ReadOnly = true;
      RequirementName.ColumnWidth = "25%";

      AuditRequirement.IsVisible = false;
      WIId.IsVisible = false;

      AuditResponseStatus.ColumnWidth = "15%";

      Observation.ColumnWidth = "15%";
      Observation.Rows = 1;

      HNC.IsVisible = Exp(() => AuditResponseStatus.Value == null ? false : AuditResponseStatus.Value.GetId() == "2" || AuditResponseStatus.Value.GetId() == "3");
      HNC.ColumnWidth = "5%";

      ZT.IsVisible = Exp(() => AuditResponseStatus.Value == null ? false : AuditResponseStatus.Value.GetId() == "2" || AuditResponseStatus.Value.GetId() == "3");
      ZT.ColumnWidth = "5%";

      CorrectiveAction.IsVisible = Exp(() => AuditResponseStatus.Value == null ? false : AuditResponseStatus.Value.GetId() == "2" || AuditResponseStatus.Value.GetId() == "3");
      CorrectiveAction.ColumnWidth = "15%";
      CorrectiveAction.Rows = 1;

      Responsible.IsVisible = Exp(() => AuditResponseStatus.Value == null ? false : AuditResponseStatus.Value.GetId() == "2" || AuditResponseStatus.Value.GetId() == "3");
      Responsible.ColumnWidth = "10%";
      Responsible.Rows = 1;

      DaysExpire.IsVisible = Exp(() => AuditResponseStatus.Value == null ? false : AuditResponseStatus.Value.GetId() == "2" || AuditResponseStatus.Value.GetId() == "3");
      DaysExpire.ColumnWidth = "5%";

    }

    public override FormPart GetLayout()
    {
      return Flat(RequirementName, AuditResponseStatus, Observation, DocumentEvidence,
                  HNC, ZT, CorrectiveAction, Responsible, DaysExpire);
    }

    public override void OnLoad()
    {
      base.OnLoad();
      RequirementSection.Value = BoundItem.AuditRequirement.Section;
      RequirementName.Value = BoundItem.AuditRequirement.Name;
    }
  }
}
