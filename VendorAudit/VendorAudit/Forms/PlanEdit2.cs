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
  public class FrmAuditorPlanEdit : Form<ExecutionWI>
  {


    public virtual BoLookupField<Plant> Plant { get; set; }
    public virtual BoLookupField<Vendor> Vendor { get; set; }
    public virtual BoLookupField<Category> Category { get; set; }
    public virtual BoLookupField<Brand> Brand { get; set; }
    public virtual BoLookupField<AuditType> AuditType { get; set; }
    public virtual DateField AuditDate { get; set; }
    [Unbound]
    public virtual TextField ContactName { get; set; }
    [Unbound]
    public virtual TextField ContactEmail { get; set; }
    [Unbound]
    public virtual TextField PlantAddress { get; set; }
    [Unbound]
    public virtual TextField WebSite { get; set; }
    [Unbound]
    public virtual TextField PlantCountry { get; set; }
    [Unbound]
    public virtual IntField PlantYear { get; set; }
    [Unbound]
    public virtual IntField PlantSQM { get; set; }
    [Unbound]
    public virtual IntField PlantEmployeeNumber { get; set; }
    [Unbound]
    public virtual IntField PlantMaxProdCap { get; set; }
    [Unbound]
    public virtual MemoField PlantCharacteristics { get; set; }
    public virtual BoLookupField<Process> Process { get; set; }
    public virtual BoLookupField<AuditCategory> AuditCategory { get; set; }
    public override void ConfigureFields()
    {

      base.ConfigureFields();
      Vendor.ReadOnly = true;
      Brand.ReadOnly = true;
      Category.ReadOnly = true;
      ContactName.ReadOnly = true;
      ContactEmail.ReadOnly = false;
      ContactEmail.Required = true;
      Plant.ReadOnly = true;
      PlantCountry.ReadOnly = true;
      PlantAddress.ReadOnly = true;
      WebSite.ReadOnly = true;
      PlantYear.ReadOnly = true;
      PlantSQM.ReadOnly = true;
      PlantEmployeeNumber.ReadOnly = true;
      AuditType.Required = true;
      AuditDate.Required = true;
      PlantMaxProdCap.ReadOnly = true;
      PlantCharacteristics.ReadOnly = true;
      //Process.ReadOnly = true;
      //AuditCategory.ReadOnly = true;
    }

    public override void OnLoad()
    {
      base.OnLoad();
      ContactName.Value = Context.Item.Vendor.ContactUser.FullName;
      ContactEmail.Value = Context.Item.Vendor.ContactUser.Email;
      PlantAddress.Value = Context.Item.Plant.Address;
      PlantCountry.Value = Context.Item.Plant.Country;
      WebSite.Value = Context.Item.Vendor.WebSite;
      PlantYear.Value = Context.Item.Plant.Year;
      PlantSQM.Value = Context.Item.Plant.SQM;
      PlantEmployeeNumber.Value = Context.Item.Plant.EmployeeNumber;
      PlantMaxProdCap.Value = Context.Item.Plant.MaxProdCap;
      PlantCharacteristics.Value = Context.Item.Plant.Characteristics;
    }

    public override FormPart GetLayout()
    {
      var r1 = Row(title: "Vendor Information", cssClass: "FirstSeparator");
      var r2 = Row(Col(Vendor), Col(Brand), Col(Category));
      var r3 = Row(Col(WebSite), Col(ContactName), Col(ContactEmail));
      var r4 = Row(title: "Plant Information", cssClass: "Separator");
      var r5 = Row(Col(Plant), Col(PlantAddress), Col(PlantCountry));
      var r6 = Row(Col(PlantYear), Col(PlantSQM), Col(PlantEmployeeNumber));
      var r7 = Row(Col(PlantMaxProdCap), Col(PlantCharacteristics));
      var r8 = Row(title: "Audit Information", cssClass: "Separator");
      var r9 = Row(Col(Process), Col(AuditCategory));
      var ra = Row(Col(AuditType), Col(AuditDate));

      return Flat(r1, r2, r3, r4, r5, r6, r7, r8, r9, ra);
    }
  }

}
