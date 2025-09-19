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

namespace VendorAudit.Forms
{
  public class PlannerFormDisplay : Form<PlannerWI>
  {

    public virtual DateField PlanDate { get; set; }
    [Unbound]
    public virtual IntField NumVendors { get; set; }
    public virtual UserLookupField AssignedAuditor { get; set; }
    public virtual UserLookupField AssignedAuditorCoordinator { get; set; }
    public virtual BoLookupField<Brand> Brand { get; set; }
    public virtual BoLookupField<Country> Country { get; set; }
    public override FormPart GetLayout()
    {
      return Grid(3);
    }
    public override void OnLoad()
    {
      base.OnLoad();
      NumVendors.Value = Context.Item.VendorAuditToPlan?.Count ?? 0;
    }
  }

}
