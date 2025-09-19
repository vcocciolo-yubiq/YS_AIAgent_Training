using VendorAudit.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;

namespace VendorAudit.BusinessObjects
{
  [DbBo]
  public class VolatileCalendarRow : BusinessObject
  {
    [Key]
    public virtual string Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Country { get; set; }
    public virtual int Jan { get; set; }
    public virtual int Feb { get; set; }
    public virtual int Mar { get; set; }
    public virtual int Apr { get; set; }
    public virtual int May { get; set; }
    public virtual int Jun { get; set; }
    public virtual int Jul { get; set; }
    public virtual int Aug { get; set; }
    public virtual int Sep { get; set; }
    public virtual int Oct { get; set; }
    public virtual int Nov { get; set; }
    public virtual int Dec { get; set; }

    public override string GetId()
    {
      return Id;
    }

    public override void SetId(string id)
    {
      Id = id;
    }

  }

}
