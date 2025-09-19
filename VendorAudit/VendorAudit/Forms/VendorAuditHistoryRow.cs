using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VendorAudit.WorkItems;
using VendorAudit.BusinessObjects;
using YubikStudioCore.Forms.Fields;

namespace VendorAudit.Forms
{
  public class PlantAuditHistoryRow : SubForm<PlantAuditHistory>
  {
    public virtual BoLookupField<AuditType> AuditType { get; set; }
    public virtual DateField AuditDate { get; set; }
    public virtual IntField RespNC { get; set; }
    [Unbound]
    public virtual HtmlPart MyRespHNC { get; set; }
    [Unbound]
    public virtual HtmlPart MyRespZT { get; set; }
    public virtual DecimalField RespScore { get; set; }
    [Unbound]
    public virtual HtmlPart MyRespRating { get; set; }


    public override void ConfigureFields()
    {
      base.ConfigureFields();
      AuditDate.CssClass = "text-center";
      RespNC.CssClass = "text-center";
      RespScore.CssClass = "text-center";
      MyRespHNC.CssClass = "text-center";
      MyRespZT.CssClass = "text-center";
      MyRespRating.CssClass = "text-center";
    }

    public override void OnLoad()
    {
      base.OnLoad();
      var badge = BoundItem.RespHNC ? "secondary" : "danger";
      MyRespHNC.RawHTML = $"<span class='badge badge-{badge}'>HNC</span>";
      badge = BoundItem.RespZT ? "secondary" : "danger";
      MyRespZT.RawHTML = $"<span class='badge badge-{badge}'>ZT</span>";

      badge = BoundItem.RespRating  <=30 ? "danger" :
              BoundItem.RespRating <= 50 ? "warning":
              "success";
      MyRespRating.RawHTML = $"<span class='badge badge-{badge}'>{BoundItem.RespRating}</span>";
            
    }
  }
}

