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
  public class ExecEdit : Form<ExecutionWI>
  {

    public virtual TableField<AuditResponse, AuditResponseRow> AuditResponse { get; set; }
    public virtual Document AuditAtt1 { get; set; }
    public virtual Document AuditAtt2 { get; set; }
    public virtual Document AuditAtt3 { get; set; }
    public virtual Document AuditAtt4 { get; set; }
    public virtual TextField AuditAtt1Descr { get; set; }
    public virtual TextField AuditAtt2Descr { get; set; }
    public virtual TextField AuditAtt3Descr { get; set; }
    public virtual TextField AuditAtt4Descr { get; set; }

    public override void ConfigureFields()
    {
      base.ConfigureFields();

      AuditResponse.GroupBy = "RequirementSection";
    }

    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
    }

    public override void OnLoad()
    {
      base.OnLoad();
    }

    public override FormPart GetLayout()
    {
      // Vendors Selection
      var t1r1 = Row(Col(AuditResponse));
      var t1 = Flat(t1r1);

      // Plant Location
      var t2r1 = Row(Col(Prop(nameof(AuditAtt1))), Col(AuditAtt1Descr));
      var t2r2 = Row(Col(Prop(nameof(AuditAtt2))), Col(AuditAtt2Descr));
      var t2r3 = Row(Col(Prop(nameof(AuditAtt3))), Col(AuditAtt3Descr));
      var t2r4 = Row(Col(Prop(nameof(AuditAtt4))), Col(AuditAtt4Descr));
      var t2 = Flat(t2r1, t2r2, t2r3, t2r4);

      var tabs = Tabs(t1, t2);
      tabs.TabHeaders = ["Checklist", "Document Attachments"];
      return Flat(tabs);
    }
  }

}
