using VendorAudit.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;

namespace VendorAudit.BusinessObjects
{
  // [WsBo]
  // [CustomBo<>]
  // [InMemoryBo]
  [DbBo(DescriptionProperties = [nameof(Name)])]
  public class Plant : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual Vendor Vendor { get; set; }
    public virtual decimal Latitude { get; set; }
    public virtual decimal Longitude { get; set; }
    public virtual string Address { get; set; }
    public virtual string Country { get; set; }
    public virtual int Year { get; set; }
    public virtual int SQM { get; set; }
    public virtual int EmployeeNumber { get; set; }
    public virtual int MaxProdCap { get; set; }
    public virtual string Characteristics { get; set; }
    public virtual bool ToAudit { get; set; } = false;


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
