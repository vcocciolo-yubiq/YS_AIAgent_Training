using YubikStudioCore.Attributes;
using VendorAudit.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Forms.Fields;
using VendorAudit.WorkItems;
using Microsoft.VisualBasic;

namespace VendorAudit.Forms
{
  public class PlanList : Form<ExecutionWI>
  {
    public virtual BoLookupField<Vendor> Vendor { get; set; }
    public virtual BoLookupField<Brand> Brand { get; set; }
    public virtual BoLookupField<Process> Process { get; set; }
    public virtual BoLookupField<AuditCategory> AuditCategory { get; set; }
    public virtual UserLookupField AssignedAuditor { get; set; }
    public virtual ToggleField RespHNCZT { get; set; }
    [Unbound]
    public virtual TextField RespHNCZT2 { get; set; }
    public virtual DateField ExecFinish { get; set; }
    public virtual DateField ExecEdit { get; set; }
    public virtual DateField ExecStart { get; set; }
    public virtual IntField RespNC { get; set; }
    public virtual DecimalField RespRating { get; set; }
    [Unbound]
    public virtual TextField RespRating2 { get; set; }
    public virtual ToggleField AuditComplete { get; set; }
    public virtual DateField AuditDate { get; set; }
    public virtual BoLookupField<Plant> Plant { get; set; }
    public override FormPart GetLayout()
    {

      RespRating2.CssClass =
        RespRating.Value == null ? "" :
        RespRating.Value <= 30 ? "badge badge-danger" :
        RespRating.Value < 70 ? "badge badge-warning" :
        "badge badge-success";

      var rows = new List<Row>(){
          Row(Col(Plant),Col(Vendor), Col(Brand), Col(Process)),
          Row(Col(AuditCategory), Col(AssignedAuditor)),
          Row(Col(ExecStart), Col(ExecFinish), Col(ExecEdit)),
          Row(Col(RespNC), Col(RespHNCZT2), Col(RespRating2)),
      };
      return Flat(rows.ToArray());
    }
    public override void OnLoad()
    {
      base.OnLoad();
      RespRating2.Value = Context.Item.AuditComplete ? ((int)RespRating.Value).ToString() + "/100" : "--";
      RespHNCZT2.Value = Context.Item.AuditComplete ? RespHNCZT.Value ? "Yes" : "No" : "--";
    }
  }

}
