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
  public class Invoice : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual Header Header { get; set; }
    public virtual Footer Footer { get; set; }
    public virtual ICollection<LineItem> LineItem { get; set; }
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
