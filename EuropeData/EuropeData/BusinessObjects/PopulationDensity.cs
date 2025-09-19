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
  public class PopulationDensity : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual string Country { get; set; }
    public virtual int Population { get; set; }
    public virtual int Density { get; set; }
    public virtual decimal AreaKm2 { get; set; }
    public virtual string Capital { get; set; }
    public virtual decimal Lat { get; set; }
    public virtual decimal Lon { get; set; }
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
