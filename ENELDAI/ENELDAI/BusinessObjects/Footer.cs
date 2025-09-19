using ENELDAI.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;

namespace ENELDAI.BusinessObjects
{
  // [WsBo]
  // [CustomBo<>]
  // [InMemoryBo]
  [DbBo]
  public class Footer : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual decimal? SubTotal { get; set; }
    public virtual decimal? TotalTax { get; set; }
    public virtual decimal? InvoiceTotal { get; set; }
    public override string GetId()
    {
      return Id.ToString();
    }

    public override void SetId(string id)
    {
      Id = int.Parse(id);
    }

  }

}
