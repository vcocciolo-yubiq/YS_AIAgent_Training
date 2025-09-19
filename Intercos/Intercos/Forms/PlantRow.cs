using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Intercos.WorkItems;
using Intercos.BusinessObjects;

namespace Intercos.Forms
{
  public class PlantRow : SubForm<Plant>
  {

    public virtual TextField Name { get; set; }
    public virtual BoLookupField<Vendor> Vendor { get; set; }
    public virtual TextField Address { get; set; }
    public virtual TextField Country { get; set; }
    public virtual IntField EmployeeNumber { get; set; }
    public virtual IntField MaxProdCap { get; set; }
    public virtual TextField Characteristics { get; set; }
    public override void ConfigureFields()
    {
      base.ConfigureFields();
      MaxProdCap.ColumnWidth = "10%";
      EmployeeNumber.ColumnWidth = "10%";
      Country.ColumnWidth = "10%";
      Vendor.ColumnWidth = "15%";

      // MaxProdCap.ReadOnly = true;
      // EmployeeNumber.ReadOnly = true;
      // Country.ReadOnly = true;
      // Vendor.ReadOnly = true;

    }
  }

}
