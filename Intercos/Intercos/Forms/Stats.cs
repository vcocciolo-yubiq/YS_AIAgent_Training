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
  public class Stats : Form<QuotationWI>
  {

    public virtual BoLookupField<Customer> Customer { get; set; }
    public virtual DateField DeliveryDL { get; set; }
    public virtual MemoField Description { get; set; }
    public virtual DecimalField EsTotalCost { get; set; }
    public virtual BoLookupField<Formula> Formula { get; set; }
    public virtual IntField MaxEstTotalCost { get; set; }
    public virtual IntField MaxNumItems { get; set; }
    public virtual IntField NumProdItems { get; set; }
    public virtual BoLookupField<Package> PrimaryPackaging { get; set; }
    public virtual DateField QuotationDL { get; set; }
    public virtual BoLookupField<Package> SecondaryPackaging { get; set; }

        //------------------ Unbound Fields -------------------
    [Unbound]
    public virtual ChartPart PieCosts { get; set; }
    [Unbound]
    public virtual ChartPart BarNumItems { get; set; }
    [Unbound]
    public virtual ChartPart BarEstCosts { get; set; }
    public override FormPart GetLayout()
        {

            
            return Grid(2);
        }
  }

}
