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
  public class VendorCalendar : Form<ExecutionWI>
{
        [Unbound]
        public virtual IntLookupField Year { get; set; }
        [Unbound]
        [DependsOn(nameof(Year))]
        public virtual TableField<VolatileCalendarRow, VolatileCalendarRowForm> Calendar { get; set; }
        public override void ConfigureFields()
        {
            base.ConfigureFields();
            Year.OnGetOptions = () => [DateTime.Now.Year, DateTime.Now.Year + 1, DateTime.Now.Year + 2];
            Year.HasDynamicOptions = false;
            Year.ReadOnly = false;
            Calendar.CanSort = false;
            Calendar.CssClass = "FullCalendar";
            Calendar.DependsOn = [nameof(Year)];
        }
        public override void OnLoad()
        {
            base.OnLoad();
            Year.Value = DateTime.Now.Year;
            OnRefresh(new string[] { nameof(Year) });
        }

        public override void OnRefresh(string[] changedProperties)
        {
            base.OnRefresh(changedProperties);
            Calendar.Value = Context.BO.All<AuditorCalendar>(0, 3000).Where(c => c.AuditDate.Year == Year.Value).GroupBy(c => c.Vendor).Select(c => new VolatileCalendarRow()
            {
                Name = c.Key.Name,
                Jan = c.Count(x => x.AuditDate.Month == 1),
                Feb = c.Count(x => x.AuditDate.Month == 2),
                Mar = c.Count(x => x.AuditDate.Month == 3),
                Apr = c.Count(x => x.AuditDate.Month == 4),
                May = c.Count(x => x.AuditDate.Month == 5),
                Jun = c.Count(x => x.AuditDate.Month == 6),
                Jul = c.Count(x => x.AuditDate.Month == 7),
                Aug = c.Count(x => x.AuditDate.Month == 8),
                Sep = c.Count(x => x.AuditDate.Month == 9),
                Oct = c.Count(x => x.AuditDate.Month == 10),
                Nov = c.Count(x => x.AuditDate.Month == 11),
                Dec = c.Count(x => x.AuditDate.Month == 12)
            }).ToList();
        }
    }


}
