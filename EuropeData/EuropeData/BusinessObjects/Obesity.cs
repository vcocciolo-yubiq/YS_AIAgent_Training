using EuropeData.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;

namespace EuropeData.BusinessObjects
{
  // [WsBo]
  // [CustomBo<>]
  // [InMemoryBo]
  [DbBo]
  public class Obesity : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual string Country { get; set; }
    public virtual decimal Children { get; set; }
    public virtual decimal Male { get; set; }
    public virtual decimal Female { get; set; }
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
