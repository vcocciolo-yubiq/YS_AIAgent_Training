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
    public class VolatileCalendarRowForm : SubForm<VolatileCalendarRow>
    {
        [Unbound]
        public virtual TextField AuditorName { get; set; }
        [Unbound]
        public virtual TextField AuditorLocation { get; set; }
        public virtual IntField Jan { get; set; }
        public virtual IntField Feb { get; set; }
        public virtual IntField Mar { get; set; }
        public virtual IntField Apr { get; set; }
        public virtual IntField May { get; set; }
        public virtual IntField Jun { get; set; }
        public virtual IntField Jul { get; set; }
        public virtual IntField Aug { get; set; }
        public virtual IntField Sep { get; set; }
        public virtual IntField Oct { get; set; }
        public virtual IntField Nov { get; set; }
        public virtual IntField Dec { get; set; }

        public override void OnLoad()
        {
            base.OnLoad();
            AuditorName.Value = BoundItem.Name;
            AuditorName.ColumnWidth = "10%";

            AuditorLocation.Value = BoundItem.Country;
            AuditorLocation.ColumnWidth = "6%";

            Jan.ColumnWidth = "4%";
            Feb.ColumnWidth = "4%";
            Mar.ColumnWidth = "4%";
            Apr.ColumnWidth = "4%";
            May.ColumnWidth = "4%";
            Jun.ColumnWidth = "4%";
            Jul.ColumnWidth = "4%";
            Aug.ColumnWidth = "4%";
            Sep.ColumnWidth = "4%";
            Oct.ColumnWidth = "4%";
            Nov.ColumnWidth = "4%";
            Dec.ColumnWidth = "4%";

            Jan.CssClass = "qt-ball " + (Jan.Value == 0 ? "qt-0" : Jan.Value < 4 ? "qt-1" : Jan.Value < 9 ? "qt-2" : "qt-3");
            Feb.CssClass = "qt-ball " + (Feb.Value == 0 ? "qt-0" : Feb.Value < 4 ? "qt-1" : Feb.Value < 9 ? "qt-2" : "qt-3");
            Mar.CssClass = "qt-ball " + (Mar.Value == 0 ? "qt-0" : Mar.Value < 4 ? "qt-1" : Mar.Value < 9 ? "qt-2" : "qt-3");
            Apr.CssClass = "qt-ball " + (Apr.Value == 0 ? "qt-0" : Apr.Value < 4 ? "qt-1" : Apr.Value < 9 ? "qt-2" : "qt-3");
            May.CssClass = "qt-ball " + (May.Value == 0 ? "qt-0" : May.Value < 4 ? "qt-1" : May.Value < 9 ? "qt-2" : "qt-3");
            Jun.CssClass = "qt-ball " + (Jun.Value == 0 ? "qt-0" : Jun.Value < 4 ? "qt-1" : Jun.Value < 9 ? "qt-2" : "qt-3");
            Jul.CssClass = "qt-ball " + (Jul.Value == 0 ? "qt-0" : Jul.Value < 4 ? "qt-1" : Jul.Value < 9 ? "qt-2" : "qt-3");
            Aug.CssClass = "qt-ball " + (Aug.Value == 0 ? "qt-0" : Aug.Value < 4 ? "qt-1" : Aug.Value < 9 ? "qt-2" : "qt-3");
            Sep.CssClass = "qt-ball " + (Sep.Value == 0 ? "qt-0" : Sep.Value < 4 ? "qt-1" : Sep.Value < 9 ? "qt-2" : "qt-3");
            Oct.CssClass = "qt-ball " + (Oct.Value == 0 ? "qt-0" : Oct.Value < 4 ? "qt-1" : Oct.Value < 9 ? "qt-2" : "qt-3");
            Nov.CssClass = "qt-ball " + (Nov.Value == 0 ? "qt-0" : Nov.Value < 4 ? "qt-1" : Nov.Value < 9 ? "qt-2" : "qt-3");
            Dec.CssClass = "qt-ball " + (Dec.Value == 0 ? "qt-0" : Dec.Value < 4 ? "qt-1" : Dec.Value < 9 ? "qt-2" : "qt-3");
        }

        public override void OnRefresh(string[] changedProperties)
        {
            AuditorName.Value = BoundItem.Name;
        }
        public override FormPart GetLayout()
        {
            return Flat(AuditorName, AuditorLocation, Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec);
        }
    }

}
