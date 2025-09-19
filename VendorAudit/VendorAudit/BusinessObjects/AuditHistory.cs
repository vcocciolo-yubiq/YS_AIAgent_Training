using VendorAudit.BusinessObjects;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Attributes.BoAttributes;
using YubikStudioCore.BusinessObjects;
using YubikStudioCore.Forms.Fields;

namespace VendorAudit.BusinessObjects
{
  // [WsBo]
  // [CustomBo<>]
  // [InMemoryBo]
  [DbBo(DescriptionProperties = new string[] { "VendorName", "AuditTypeName", "AuditDate" })]
  public class PlantAuditHistory : BusinessObject
  {
    [Key]
    public virtual int Id { get; set; }
    public virtual Plant Plant { get; set; }
    public virtual AuditType AuditType { get; set; }
    public virtual DateOnly AuditDate { get; set; }
    public virtual decimal RespScore { get; set; }
    public virtual decimal RespRating { get; set; }
    public virtual bool RespHNCZT { get; set; }
    public virtual bool RespHNC { get; set; }
    public virtual bool RespZT { get; set; }
    public virtual int RespNC { get; set; }




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
