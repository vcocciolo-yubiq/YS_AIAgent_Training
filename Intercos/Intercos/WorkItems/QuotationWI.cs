using Intercos.BusinessObjects;
using YubikStudioCore.Attributes;
using YubikStudioCore;
using YubikStudioCore.Documents;

namespace Intercos.WorkItems
{
  public class QuotationWI : WorkItem
  {


    public virtual string Code { get; set; }
    public virtual int NumProdItems { get; set; }
    public virtual DateOnly QuotationDL { get; set; }
    public virtual Technology Technology { get; set; }
    public virtual Plant Site { get; set; }
    public virtual bool PackagingCompleted { get; set; }
    public virtual bool RAndICompleted { get; set; }
    public virtual int MaxNumItems { get; set; }
    public virtual int MaxEstTotalCost { get; set; }
    public virtual bool HasPackaging { get; set; }
    public virtual Package SecondaryPackaging { get; set; }
    public virtual Package PrimaryPackaging { get; set; }
    public virtual Formula Formula { get; set; }

    public virtual DateOnly DeliveryDL { get; set; }
    public virtual decimal EsTotalCost { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual string Description { get; set; }
  }

}
