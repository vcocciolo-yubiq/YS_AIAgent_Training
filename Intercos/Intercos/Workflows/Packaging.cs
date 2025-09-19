using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using Intercos.BusinessObjects;
using Intercos.StaticRoles;
using Intercos.DynamicRoles;
using Intercos.Forms;
using Intercos.WorkItems;
using Intercos.CodeLibs;
using static Intercos.Workflows.Quotation;

namespace Intercos.Workflows
{
  [Visibility<Admin>]
  [Workflow("7bfbb5af-1e47-42f9-8164-6c532cad4512", "WI", order: -1, category: "packaging")]
  [Form<PackagingFormWI>]
  public class Packaging : Workflow<PackagingWI>
  {
    [Visibility<Admin>]
    [Action<NewPackaging>]
    public class CreateItem : YAction
    {
      public virtual PackagingCreate form { get; set; }
      public override void OnExecute()
      {
        base.OnExecute();
        Context.Item.LastModifiedDate = DateTime.Now;
        //Context.Item.AssOwner = Context.User;
      }

    }
    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Watch<Sales>]
    [Todo<PackDevelopment>]
    [Form<PackagingView>]
    public class PackageDefinition : Stage
    {


      [Action<PackageDefinition>(Modal = ModalMode.Large)]
      [Visibility<Admin>]
      public class Edit : YAction
      {
        public virtual PackagingEdit form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          Context.Item.PrimaryPackageObj.Description = form.PrimaryPackageDescription.Value;
          Context.Item.SecondaryPackageObj.Description = form.SecondaryPackageDescription.Value;
          Context.Item.PrimaryPackageObj.EstCost = form.PrimaryEstCost.Value ?? 0;
          Context.Item.SecondaryPackageObj.EstCost = form.SecondaryEstCost.Value ?? 0;
          Context.Item.PrimaryPackageObj.PotentialVendor = form.PrimaryPotentialVendor.Value;
          Context.Item.SecondaryPackageObj.PotentialVendor = form.SecondaryPotentialVendor.Value;

          Context.Item.LastModifiedDate = DateTime.Now;
          Context.Item.AssOwner = Context.User;
        }

      }



      [Action<PackageSourcing>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class SendToSourcing : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          Context.Item.LastModifiedDate = DateTime.Now;
        }

        public override bool CanView()
        {
          if (Context.Item.PrimaryPackageObj.Description == null || Context.Item.SecondaryPackageObj.Description == null
             || Context.Item.PrimaryPackageObj.PotentialVendor == null || Context.Item.SecondaryPackageObj.PotentialVendor == null
             || Context.Item.PrimaryPackageObj.EstCost <= 0 || Context.Item.SecondaryPackageObj.EstCost <= 0)
          {
            return false;
          }
          return base.CanView();
        }
      }
    }
    // [Todo<Admin>]
    // [Watch<Admin>]
    [Watch<PackDevelopment, Sourcing, Sales>]
    [Form<SourcingView>]
    [Archive]
    public class Archive : Stage
    {

    }


    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Watch<PackDevelopment>]
    [Form<SourcingView>]
    [Todo<Sourcing>]
    public class PackageSourcing : Stage
    {


      [Action<PackageValidation>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class SendToValidation : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override bool CanView()
        {
          if (Context.Item.PrimaryPackageObj.EffectiveVendor == null || Context.Item.SecondaryPackageObj.EffectiveVendor == null
             || Context.Item.PrimaryPackageObj.EffectiveCost <= 0 || Context.Item.SecondaryPackageObj.EffectiveCost <= 0)
          {
            return false;
          }
          return base.CanView();
        }
      }

      [Action<PackageSourcing>(Modal = ModalMode.Large)]
      [Visibility<Admin>]
      public class AddSourcingInfo : YAction
      {
        public virtual SourcingEdit form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          Context.Item.LastModifiedDate = DateTime.Now;
          if (Context.Item.PrimaryPackageObj != null)
          {
            Context.Item.PrimaryPackageObj.EffectiveVendor = form.PriEffectiveVendor.Value;
            Context.Item.PrimaryPackageObj.EffectiveCost = form.PriEffectiveCost.Value ?? 0;
          }

          if (Context.Item.SecondaryPackageObj != null)
          {
            Context.Item.SecondaryPackageObj.EffectiveVendor = form.SecEffectiveVendor.Value;
            Context.Item.SecondaryPackageObj.EffectiveCost = form.SecEffectiveCost.Value ?? 0;
          }

        }

      }
    }

    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Form<SourcingView>]
    [Watch<Sourcing>]
    [Todo<Sales, PackDevelopment>]
    public class PackageValidation : Stage
    {


      [Action<Archive>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class ValidatePackage : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          Context.Item.LastModifiedDate = DateTime.Now;
          Context.Item.PackagingComplete = true;

        }

      }

      [Action<PackageSourcing>]
      [Visibility<Admin>]
      public class RejectPackageValidation : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

      }
    }

    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]


    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Form<PackagingView>]
    [Todo<PackDevelopment>]
    public class NewPackaging : Stage
    {


      [Action<PackageDefinition>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class TakeInCharge : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          Context.Item.AssOwner = Context.User;
          Context.Item.LastModifiedDate = DateTime.Now;
          Context.Item.PackagingComplete = false;
        }

      }
    }
  }

}
