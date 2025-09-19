using ENELDAI.BusinessObjects;
using YubikStudioCore.Attributes;
using YubikStudioCore;
using YubikStudioCore.Documents;

namespace ENELDAI.WorkItems
{
  public class TestDAIWI : WorkItem
  {


    public virtual Document Document { get; set; }
    public virtual Locale Locale { get; set; }
    public virtual Invoice Invoice { get; set; }
  }

}
