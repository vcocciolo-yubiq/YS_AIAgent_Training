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
  public class SourcingEdit : Form<PackagingWI>
  {

    public virtual TextField Description { get; set; }
    public virtual BoLookupField<Package> PrimaryPackageObj { get; set; }
    public virtual BoLookupField<Package> SecondaryPackageObj { get; set; }

    //------------- Unbound Fields ---------------

    [Unbound]
    public virtual BoLookupField<Vendor> PriEffectiveVendor { get; set; }
    [Unbound]
    public virtual BoLookupField<Vendor> SecEffectiveVendor { get; set; }
    [Unbound]
    public virtual DecimalField PriEffectiveCost { get; set; }
    [Unbound]
    public virtual DecimalField SecEffectiveCost { get; set; }


    public override void ConfigureFields()
    {
      base.ConfigureFields();
      PriEffectiveVendor.Required = true;
      PriEffectiveCost.Required = true;
      SecEffectiveVendor.Required = true;
      SecEffectiveCost.Required = true;
    }

    public override FormPart GetLayout()
    {
      return Flat(
          Row(Col(PriEffectiveVendor)),
          Row(Col(PriEffectiveCost)),
          Row(Col(SecEffectiveVendor)),
          Row(Col(SecEffectiveCost))
      );
    }
        public override void OnLoad()
        {
            base.OnLoad();
            if (Context.Item.PrimaryPackageObj != null)
            {
                PriEffectiveVendor.Value = Context.Item.PrimaryPackageObj.EffectiveVendor;
                PriEffectiveCost.Value = Context.Item.PrimaryPackageObj.EffectiveCost;
            }
            if (Context.Item.SecondaryPackageObj != null)
            {
                SecEffectiveVendor.Value = Context.Item.SecondaryPackageObj.EffectiveVendor;
                SecEffectiveCost.Value = Context.Item.SecondaryPackageObj.EffectiveCost;
            }
    }
  }

}
