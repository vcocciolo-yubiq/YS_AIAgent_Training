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
  public class AuditorCalendarRow : SubForm<AuditorCalendar>
  {
    public virtual BoLookupField<AuditType> AuditType { get; set; }
    public virtual Vendor Vendor { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual DateOnly AuditDate { get; set; }

    public override IEnumerable<string> GetColumnNames()
    {
      return base.GetColumnNames();
    }

    public override FormPart GetLayout()
    {
      AuditType.CssClass = "Larghezzamassima10";
      return base.GetLayout();
    }
  }

}
