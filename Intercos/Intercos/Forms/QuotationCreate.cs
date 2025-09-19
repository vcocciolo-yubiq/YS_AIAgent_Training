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
  public class PackagingCreate : Form<PackagingWI>
  {

    public virtual IntField QuotationId { get; set; }
    public virtual TextField Title { get; set; }

    public virtual BoLookupField<Package> PrimaryPackageObj { get; set; }
    public virtual BoLookupField<Package> SecondaryPackageObj { get; set; }
    public override void ConfigureFields()
    {
      base.ConfigureFields();
      Title.IsVisible = false;
      QuotationId.IsVisible = false;
    }
  }

}
