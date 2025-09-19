using Intercos.BusinessObjects;
using YubikStudioCore.Attributes;
using YubikStudioCore;
using YubikStudioCore.Documents;

namespace Intercos.WorkItems
{
  public class PackagingWI : WorkItem
  {


    public virtual string Description { get; set; }


    public virtual int QuotationId { get; set; }


    public virtual bool PackagingComplete { get; set; }
    public virtual Package SecondaryPackageObj { get; set; }
    public virtual DateTime LastModifiedDate { get; set; }
    public virtual User AssOwner { get; set; }
    public virtual Package PrimaryPackageObj { get; set; }


  }

}
