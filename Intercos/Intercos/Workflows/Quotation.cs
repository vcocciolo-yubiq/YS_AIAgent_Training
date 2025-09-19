using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using Intercos.BusinessObjects;
using Intercos.StaticRoles;
using Intercos.DynamicRoles;
using Intercos.Forms;
using Intercos.WorkItems;
using Intercos.CodeLibs;
using System.Security.Cryptography.X509Certificates;

namespace Intercos.Workflows
{
  [Visibility<Admin, Sales>]
  [Workflow("26f336d1-41a3-49c3-80ec-8420ac973e26", "WI", 0, category: "quotation")]
  [Form<QuotationFormWI>]
  public class Quotation : Workflow<QuotationWI>
  {
    [Visibility<Admin, Sales>]
    [Action<NewBRIEF>(Modal = ModalMode.XtraLarge, TodoList = true)]
    public class CreateItem : YAction
    {
      public virtual BRIEFCreate form { get; set; }
      public override void OnExecute()
      {
        base.OnExecute();
        //Context.Item.Title = form.Title.Value;
        // Context.Item.Code = "QUOT-" + ;
        Context.Item.MaxEstTotalCost = 300000;
        Context.Item.MaxNumItems = 300000;
        Context.Item.RAndICompleted = false;
        Context.Item.PackagingCompleted = false;

        if (Context.Item.HasPackaging)
        {
          // Create a new Packaging item
          var priPkg = new Package
          {
            Type = PackageType.Primary,
            EstCost = 0,
            PotentialVendor = null,
            Description = form.PrimaryPackageDescription.Value
          };
          var secPkg = new Package
          {
            Type = PackageType.Secondary,
            EstCost = 0,
            PotentialVendor = null,
            Description = form.SecondaryPackageDescription.Value
          };

          // Insert the primary and secondary packages into the database
          // and set them to the quotation item
          Context.BO.Insert<Package>(priPkg);
          Context.BO.Insert<Package>(secPkg);
          Context.Item.PrimaryPackaging = priPkg;
          Context.Item.SecondaryPackaging = secPkg;
        }
      }


    }
    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Todo<Sales>]
    [Form<BRIEFView>]
    public class NewBRIEF : Stage
    {


      [Action<FormulaDefinition>(Modal = ModalMode.Medium, TodoList = true)]
      [Visibility<Admin>]
      public class SendToFormulaDefinition : YAction
      {
        //public virtual ActionConfirm form { get; set; }
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();

          // if (Context.Item.HasPackaging)
          // {
          //   var v = new ValErr("Packaging is required for this quotation.");
          //   return;
          // }



          if (Context.Item.HasPackaging)
          {
            //create a new Packaging Workflow item;
            var newForm = Runtime.Instance.GetWorkflow<Packaging>().Actions[0].Forms[0].GetController(Context) as Forms.PackagingCreate;
            newForm.QuotationId.Value = this.Context.Item.Id;
            newForm.Title.Value = "Packaging: " + this.Context.Item.Title;
            newForm.PrimaryPackageObj.Value = this.Context.Item.PrimaryPackaging;
            newForm.SecondaryPackageObj.Value = this.Context.Item.SecondaryPackaging;

            var id = Runtime.Instance.CreateItem(Context, Runtime.Instance.GetWorkflow<Packaging>(), newForm);

          }
        }

        public override void OnPrepare()
        {
          // if (Context.Item.HasPackaging)
          // {
          //   var v = new ValErr("Packaging is required for this quotation.");
          //   return;
          // }
          base.OnPrepare();

        }
      }

      [Action<NewBRIEF>(Modal = ModalMode.Large, TodoList = true)]
      [Visibility<Admin>]
      public class Edit : YAction
      {
        public virtual BRIEFCreate form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

      }
      public override void OnEnter()
      {
        // Your Code here.
        // Context.Item.Code = "QUOT-" + Context.Item.Id.ToString("D4");

      }


    }
    // [Todo<Admin>]
    // [Watch<Admin>]
    [Archive]
    public class Archive : Stage
    {

    }


    // [Todo<Admin>]

    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Watch<Sales>]
    [Todo<BU>]
    [Form<FormulaDefView>]
    public class FormulaDefinition : Stage
    {



      [Action<FormulaValidation>(Modal = ModalMode.Large, TodoList = true)]
      [Visibility<Admin>]
      public class SendToFormulaValidation : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override bool CanView()
        {
          var ingredientsSum = Context.Item.Formula?.Ingredients?.Sum(i => i.Percentage) ?? 0;
          if (Context.Item.Formula == null || Context.Item.Formula.Id == 0 || ingredientsSum != 100)
          {
            // If the formula is not set or the ingredients do not sum to 100%, return false
            return false; // You need to add a formula to sent to validation.
          }
          return base.CanView();
        }
      }

      [Action<FormulaDefinition>(Modal = ModalMode.None)]
      [Visibility<Admin>]
      public class EditFormula : YAction
      {
        public virtual FormulaDefiEdit form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          var newIngredients = new List<Ingredient>();
          //transform ingredients in the form to a list of Ingredient objects
          foreach (var ingredient in form.Ingredients.Value)
          {

            if (
             //ingredient.Description == null || ingredient.Description == string.Empty ||
             ingredient.Material == null || ingredient.Material.Id == 0

            )
            {
              continue; // Skip ingredients with no description or material
            }
            newIngredients.Add(new Ingredient
            {
              Id = ingredient.Id,
              Quantity = ingredient.Quantity,
              Material = ingredient.Material,
              Percentage = ingredient.Percentage,
              Phase = ingredient.Phase,
              Description = ingredient.Description
            });
            Context.BO.Insert<Ingredient>(newIngredients.Last());
            Context.BO.Delete<Ingredient>(ingredient.Id.ToString());
          }
          // Use the existing formula
          Context.Item.Formula.Ingredients = newIngredients;
        }

        public override bool CanView()
        {
          if (Context.Item.Formula == null || Context.Item.Formula.Id == 0)
          {
            return false; // You need to add a formula to edit it.
          }
          return true; // Edit the formula from the button in the FormulaDefinition view.
        }
      }

      [Action<FormulaDefinition>(Modal = ModalMode.XtraLarge)]
      [Visibility<Admin>]
      public class CreateNewFormula : YAction
      {
        public virtual FormulaDefCreate form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          var newIngredients = new List<Ingredient>();
          // Copy ingredients from the existing formula to the new one
          foreach (var ingredient in form.Ingredients.Value)
          {
            if (
             //ingredient.Description == null || ingredient.Description == string.Empty ||
             ingredient.Material == null || ingredient.Material.Id == 0
            || ingredient.Quantity <= 0 || ingredient.Percentage <= 0
            )
            {
              continue; // Skip ingredients with no description or material
            }


            newIngredients.Add(new Ingredient
            {
              Id = 0, // Reset ID for new ingredient
              Quantity = ingredient.Quantity,
              Material = ingredient.Material,
              Percentage = ingredient.Percentage,
              Phase = ingredient.Phase,
              Description = ingredient.Description
            });
            Context.BO.Insert<Ingredient>(newIngredients.Last());
          }
          // Use the existing formula
          var newFormula = new Formula
          {

            //Code = Context.Item.Formula.Code,
            Description = form.Description.Value,
            Ingredients = newIngredients,
            Version = "1",
            //ProductType = form.Formula.Value.ProductType
          };

          Context.BO.Insert<Formula>(newFormula);
          newFormula.Code = "NEW-" + newFormula.Id.ToString("D4");
          Context.Item.Formula = newFormula;

        }

        public override bool CanView()
        {
          if (Context.Item.Formula != null && Context.Item.Formula.Id != 0)
          {
            return false; // You already have a formula, you cannot create a new one.
          }
          return base.CanView();
        }
      }

      [Action<FormulaDefinition>(Modal = ModalMode.XtraLarge)]
      [Visibility<Admin>]
      public class UseExistingFormula : YAction
      {
        public virtual FormulaDefCreateFromExisting form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          if (form.FormulaNew != null && form.FormulaNew.Value.Id != 0)
          {
            var newIngredients = new List<Ingredient>();
            var Ingredients = form.Ingredients.Value;
            // Copy ingredients from the existing formula to the new one
            foreach (var ingredient in Ingredients)
            {
              newIngredients.Add(new Ingredient
              {
                Id = 0, // Reset ID for new ingredient
                Quantity = ingredient.Quantity,
                Material = ingredient.Material,
                Percentage = ingredient.Percentage,
                Phase = ingredient.Phase,
                Description = ingredient.Description
              });
              Context.BO.Insert<Ingredient>(newIngredients.Last());
            }
            // Use the existing formula
            var newFormula = new Formula
            {

              //Code = Context.Item.Formula.Code,
              Description = form.Title.Value,
              Ingredients = newIngredients,
              Version = form.FormulaNew.Value.Version,
              ProductType = form.FormulaNew.Value.ProductType
            };

            Context.BO.Insert<Formula>(newFormula);
            newFormula.Code = "NEW-" + newFormula.Id.ToString("D4");
            Context.Item.Formula = newFormula;

          }
        }

        public override bool CanView()
        {
          if (Context.Item.Formula != null && Context.Item.Formula.Id != 0)
          {
            return false; // You already have a formula, you cannot create a new one.
          }
          return base.CanView();
        }
      }
    }

    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Watch<BU>]
    [Todo<Sales>]
    [Form<FormulaDefView>]
    public class FormulaValidation : Stage
    {
      [Action<CheckThreshold>(Modal = ModalMode.Large)]
      [Visibility<Admin>]
      public class ApproveFormula : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

      }



      [Action<FormulaDefinition>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class Reject : YAction
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
    //[Form<TecDefView>]
    [Watch<Sales, BU>]
    [Todo<Industrial>]
    [Form<FormulaDefView>]
    public class TechnologySelection : Stage
    {


      [Action<RoutingAndInvestmentValidation>(Modal = ModalMode.Large)]
      [Visibility<Admin>]
      public class SendToTechValidation : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override bool CanView()
        {
          if ((Context.Item.Technology == null || Context.Item.Technology.Id == 0)
          || (Context.Item.Site == null || Context.Item.Site.Id == 0))
          {
            // If the technology or site is not selected, return false
            // If the technology is not selected, return false
            return false; // You need to select a technology to send to validation.
          }
          return base.CanView();
        }
      }

      [Action<TechnologySelection>(Modal = ModalMode.XtraLarge)]
      [Visibility<Admin>]
      public class SelectTecAndSite : YAction
      {
        public virtual TechAndSiteSelection form { get; set; }
        public override void OnExecute()
        {
          Context.Item.Technology = form.Technology.Value;
          Context.Item.Site = form.Plant.Value;
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
    [Watch<Industrial, Controlling, BU>]
    [Todo<Sales>]
    [Form<FormulaDefView>]
    public class SalesConfiguration : Stage
    {


      [Action<PriceDefinition>(Modal = ModalMode.Large)]
      [Visibility<Admin>]
      public class SalesSendToPriceDefinition : YAction
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
    [Watch<Industrial, Sales, BU>]
    [Todo<Controlling>]
    [Form<FormulaDefView>]
    public class ControllingConfiguration : Stage
    {


      [Action<PriceDefinition>(Modal = ModalMode.Large)]
      [Visibility<Admin>]
      public class ContrSendToPriceDefinition : YAction
      {
        public virtual QuotationFormWI form { get; set; }
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
    [Watch<Controlling>]
    [Todo<Sales>]
    [Form<FormulaDefView>]
    public class PriceDefinition : Stage
    {


      [Action<Archive>]
      [Visibility<Admin>]
      public class Finish : YAction
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
    public class CheckThreshold : Stage
    {


      //First check if any of the three thresholds are met
      [Action<TechnologySelection>(TodoList = false, WatchList = false, Order = 1)]
      [Visibility<Admin>]
      public bool SendToTechSelection()
      {

        // If the threshold is not met, send to Technology Selection
        if ((Context.Item.Technology == null || Context.Item.Technology.Id == 1
        || Context.Item.NumProdItems > Context.Item.MaxNumItems
        || Context.Item.EsTotalCost > Context.Item.MaxEstTotalCost)
        && Context.Item.RAndICompleted == false)
        {
          // If the technology is not selected or the number of production items exceeds the threshold, return true
          return true;
        }
        return false;
      }
      [Action<WaitingForPackaging>(TodoList = false, WatchList = false, Order = 2)]
      [Visibility<Admin>]
      public bool SendToWaitingForPackaging()
      {
        var item = Runtime.Instance.GetItems<PackagingWI>(Context, x => x.QuotationId == Context.Item.Id
          ).FirstOrDefault();

        if (Context.Item.HasPackaging && !item.PackagingComplete)
        {
          // If packaging is not complete, return true and go to Waiting for Packaging
          return true;
        }
        return false;
      }
      //Then check if the thresholds are met for Controlling Configuration or Sales Configuration
      [Action<ControllingConfiguration>(TodoList = false, WatchList = false, Order = 3)]
      [Visibility<Admin>]
      public bool SendToControllingConf()
      {
        if (Context.Item.NumProdItems > Context.Item.MaxNumItems
        || Context.Item.EsTotalCost > Context.Item.MaxEstTotalCost)
          return true; // If the threshold is met, send to Controlling Configuration

        return false;
      }

      [Action<SalesConfiguration>(TodoList = false, WatchList = false, Order = 4)]
      [Visibility<Admin>]
      public bool SendToSalesConf()
      {
        if (Context.Item.NumProdItems < Context.Item.MaxNumItems
        || Context.Item.EsTotalCost < Context.Item.MaxEstTotalCost)
          return true; // If the threshold is not met, send to Sales Configuration

        return false;
      }




    }

    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Watch<Industrial, Sales, BU>]
    [Todo<Operation>]
    [Form<FormulaDefView>]
    public class RoutingAndInvestmentValidation : Stage
    {


      [Action<WaitingForPackaging>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class ApproveRAndI : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          Context.Item.RAndICompleted = true;
          base.OnExecute();
        }

      }

      [Action<TechnologySelection>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class RejectRAndI : YAction
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
    [Watch<Operation, BU, Industrial>]
    [Todo<Sales>]
    [Form<FormulaDefView>]
    public class WaitingForPackaging : Stage
    {


      [Action<CheckThreshold>(TodoList = false, WatchList = false, Order = 1)]
      [Visibility<Admin>]
      public bool PackaginComplete()
      {

        var item = Runtime.Instance.GetItems<PackagingWI>(Context, x => x.QuotationId == Context.Item.Id
          ).FirstOrDefault();

        if (item != null && item.PackagingComplete)
        {
          // If the packaging is complete, set the properties and return true
          Context.Item.HasPackaging = true;
          Context.Item.PackagingCompleted = item.PackagingComplete;
          Context.Item.PrimaryPackaging = item.PrimaryPackageObj;
          Context.Item.SecondaryPackaging = item.SecondaryPackageObj;
        }

        if (Context.Item.HasPackaging && Context.Item.PackagingCompleted)
        {
          // If the packaging is complete, return true and go to check thresholds
          return true;
        }
        return false;
      }

      [Action<CheckThreshold>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class SendToCOGSConfiguration : YAction
      {
        public virtual ActionConfirm form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override bool CanView()
        {
          var cont = Context;
          var id = Context.Item.Id;

          //TODO: Not working
          // var item = Runtime.Instance.GetItems<PackagingWI>(Context, x => x.QuotationId == Context.Item.Id)
          // .FirstOrDefault();

          // if (item != null && !item.PackagingComplete && Context.Item.HasPackaging)
          // {
          //   // If the packaging is not complete, return false and do not show go to COGS Configuration
          //   return false;
          // }
          return base.CanView();
        }
      }



      [Action<CheckThreshold>]
      [Schedule(RefProperty = "PropName", Offset = 60, Repeat = 1, NotBefore = 0, NotAfter = 24, FromMinutes = 0, FromHours = 0, FromDays = 0)]
      public void CheckPackaging()
      {

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


    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]

  }

}
