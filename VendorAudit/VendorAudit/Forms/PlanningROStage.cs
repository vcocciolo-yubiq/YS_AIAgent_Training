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
  public class PlanStage : Form<ExecutionWI>
  {
    public virtual BoLookupField<Vendor> Vendor { get; set; }
    public virtual BoLookupField<Brand> Brand { get; set; }
    public virtual BoLookupField<Process> Process { get; set; }
    public virtual BoLookupField<AuditCategory> AuditCategory { get; set; }
    public virtual BoLookupField<AuditType> AuditType { get; set; }
    public virtual UserLookupField AssignedAuditor { get; set; }
    public virtual DateField AuditDate { get; set; }

    [Unbound]
    public virtual TextField IsExternalAuditor2 { get; set; }
    [Unbound]
    public virtual TextField VendorContactName { get; set; }
    [Unbound]
    public virtual TextField VendorContactmail { get; set; }

    public override FormPart GetLayout()
    {
      var r1 = Row(Col(Vendor), Col(Brand), Col(Process));
      var r2 = Row(Col(AuditCategory), Col(AssignedAuditor),Col(IsExternalAuditor2), Col(AuditDate));
      var r3 = Row(Col(AuditType), Col(VendorContactName), Col(VendorContactmail));
      return Flat(r1, r2, r3);
    }
    public override void OnLoad()
    {
      base.OnLoad();
      IsExternalAuditor2.Value = Context.Item.IsExternalAuditor ? "Yes" : "No";
      VendorContactmail.Value=Context.Item.Vendor.ContactUser.Email;
      VendorContactName.Value = Context.Item.Vendor.ContactUser.FullName;
    }
  }

}
