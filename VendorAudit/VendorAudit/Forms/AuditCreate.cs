using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Runtime;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VendorAudit.WorkItems;
using VendorAudit.BusinessObjects;
using YubikStudioCore.Forms.Fields;

namespace VendorAudit.Forms
{
  public class VendorAuditCreate : Form<ExecutionWI>
  {

    public virtual BoLookupField<Plant> Plant { get; set; }
    public virtual BoLookupField<Vendor> Vendor { get; set; }
    public virtual BoLookupField<Brand> Brand { get; set; }
    public virtual UserLookupField AssignedAuditor { get; set; }
    public virtual UserLookupField AssignedAuditorCoordinator { get; set; }
    public virtual BoLookupField<Process> Process { get; set; }
    public virtual BoLookupField<AuditCategory> AuditCategory { get; set; }
    public virtual DateField AuditDate { get; set; }

    public override void ConfigureFields()
    {
      base.ConfigureFields();

      Vendor.Required = false;
      AssignedAuditor.Required = false;
      AssignedAuditorCoordinator.Required = false;
      Process.Required = false;
      AuditCategory.Required = false;

      Brand.Required = false;
      Brand.DependsOn = [nameof(Vendor)];
      Brand.OnGetOptions = () => Vendor.Value == null ? [] : Vendor.Value.Brand;

      AssignedAuditor.OnGetOptions = () => Runtime.Instance.GetUsers(AssignedAuditor.SearchTerm, AssignedAuditor.Page, AssignedAuditor.PageSize)
            .Where(x => x.IsInRole("Auditor"))
            .Select(x => new User() { UserName = x.UserName, FullName = $"{x.FullName} - {x.Profile["Location"]} ({x.Profile["Position"]})", UserImg = x.UserImg })
            .Skip(AssignedAuditor.Page * AssignedAuditor.PageSize)
            .Take(AssignedAuditor.PageSize);

      AssignedAuditorCoordinator.OnGetOptions = () => Runtime.Instance.GetUsers(AssignedAuditor.SearchTerm, AssignedAuditor.Page, AssignedAuditor.PageSize)
            .Where(x => x.IsInRole("AuditorCoordinator"))
            .Select(x => new User() { UserName = x.UserName, FullName = $"{x.FullName}", UserImg = x.UserImg })
            .Skip(AssignedAuditor.Page * AssignedAuditor.PageSize)
            .Take(AssignedAuditor.PageSize);
    }
    public override FormPart GetLayout()
    {
      return Grid(3);
    }

    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
      if (changedProperties.Contains(nameof(Vendor)))
      {
        Brand.Value = null; // Reset Brand when Vendor changes
        Brand.OnGetOptions = () => Vendor.Value == null ? [] : Vendor.Value.Brand;
      }
    }
  }


}
