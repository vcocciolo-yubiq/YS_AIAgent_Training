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

  public class AuditStage : Form<ExecutionWI>
  {
    public virtual TableField<AuditResponse, AuditResponseRow2> AuditResponse { get; set; }

    public virtual Document VendorReport { get; set; }
    public override void ConfigureFields()
    {
      base.ConfigureFields();

      AuditResponse.ReadOnly = true;
      AuditResponse.IsPaged = true;
      AuditResponse.PageSize = 8;
    }

    public override void OnLoad()
    {
      base.OnLoad();

      //AuditResponse.Value = Context.Item.AuditResponse.Where(x => (x.AuditResponseStatus.Id == 2 || x.AuditResponseStatus.Id == 3)).ToList();
      AuditResponse.Value = Context.Item.AuditResponse;


    }
    public override FormPart GetLayout()
    {
      var r1 = Row(Col(AuditResponse));
      var c1 = Col(Prop(nameof(VendorReport))); c1.CssClass = "col-4";
      var r2 = Row(c1);
      return Flat(r1, r2);
    }
  }

}
