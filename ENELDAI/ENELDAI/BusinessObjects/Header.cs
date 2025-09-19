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
  public class Header : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual string CustomerAddress { get; set; }
    public virtual string CustomerId { get; set; }
    public virtual string CustomerName { get; set; }
    public virtual string CustomerTaxId { get; set; }
    public virtual string CurrencyName { get; set; }
    public virtual string CurrencySymbol { get; set; }
    public virtual DateOnly? InvoiceDate { get; set; }
    public virtual string InvoiceId { get; set; }
    public virtual string VendorName { get; set; }
    public virtual string VendorTaxId { get; set; }
    public virtual string VendorAddress { get; set; }

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
