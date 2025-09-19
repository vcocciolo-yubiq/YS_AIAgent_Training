using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using VendorAudit.BusinessObjects;
using VendorAudit.StaticRoles;
using VendorAudit.DynamicRoles;
using VendorAudit.Forms;
using VendorAudit.WorkItems;
using VendorAudit.CodeLibs;
using YubikStudioCore.Runtime;
using System.Diagnostics.CodeAnalysis;

namespace VendorAudit.Workflows
{
  [Visibility<Admin, AuditorCoordinator>]
  [Workflow("103b3d4d-189b-4962-856f-5cc874b85bfd", "WI", Order = 1)]
  [Form<PlannerFormDisplay>]
  public class Planning : Workflow<PlannerWI>
  {
    [Visibility<AuditorCoordinator>]
    [Action<Archive>]
    public class CreateItem : YAction
    {
      public virtual AuditPlanCreate Frm { get; set; }
      public override void OnExecute()
      {
        base.OnExecute();

        Context.Item.AssignedAuditorCoordinator = Context.User;
        Context.Item.PlanDate = DateOnly.FromDateTime(DateTime.Now);

        var coll = new List<VendorAuditToPlan>();

        foreach (var item in Frm.PlantVendorAuditList.Value)
        {
          if (item.ToAudit)
          {
            var p = Context.BO.GetById<Plant>(item.Id.ToString());
            var newItem = new VendorAuditToPlan
            {
              Plant = p,
              Vendor = p.Vendor,
              Brand = p.Vendor.Brand.FirstOrDefault(),
            };
            coll.Add(newItem);
          }
        }
        Context.Item.VendorAuditToPlan = coll;

        foreach (var item in Context.Item.VendorAuditToPlan)
        {
          var newForm = Runtime.Instance.GetWorkflow<Execution>().Actions[0].Forms[0].GetController(Context) as Forms.VendorAuditCreate;
          newForm.Plant.Value = item.Plant;
          newForm.Vendor.Value = item.Vendor;
          newForm.Brand.Value = item.Brand;
          newForm.Process.Value = Context.Item.Process;
          newForm.AuditCategory.Value = Context.Item.AuditCategory;
          newForm.AssignedAuditor.Value = Context.Item.AssignedAuditor;
          newForm.AssignedAuditorCoordinator.Value = Context.Item.AssignedAuditorCoordinator;
          newForm.AuditDate.Value = Context.Item.AuditDate;
          Runtime.Instance.CreateItem(Context, Runtime.Instance.GetWorkflow<Execution>(), newForm);
        }
        Context.Item.Title = $"Audit planned the {Context.Item.PlanDate} in {Context.Item.Country} for {Context.Item.VendorAuditToPlan?.Count} Vendors to {Context.Item.AssignedAuditor.FullName}";

      }

    }

    [Todo<AuditorCoordinator>]
    [Archive]
    public class Archive : Stage
    {

    }


  }

}
