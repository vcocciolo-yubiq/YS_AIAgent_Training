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
using Microsoft.VisualBasic;

namespace VendorAudit.Forms
{
    public class PlantAuditRow : SubForm<Plant>
    {
        [Hidden]
        public virtual IntField Id { get; set; }
        [Unbound]
        public virtual TextField VendorName { get; set; }
        public virtual TextField Name { get; set; }
        [Unbound]
        public virtual TextField CategoryName { get; set; }
        public virtual TextField Country { get; set; }
        [Unbound]
        public virtual IntField NumAudits { get; set; }
        [Unbound]
        public virtual DateField LastAuditDate { get; set; }
        [Unbound]
        public virtual IntField AverageRating { get; set; }
        [Unbound]
        public virtual IntField AverageNC { get; set; }
        [Unbound]
        public virtual TextField HasHNCZT { get; set; }

        public virtual ToggleField ToAudit { get; set; }
        public override void ConfigureFields()
        {
            base.ConfigureFields();

            VendorName.ReadOnly = true;
            VendorName.ColumnWidth = "30%";

            CategoryName.ReadOnly = true;
            CategoryName.ColumnWidth = "10%";

            Name.ReadOnly = true;
            Name.ColumnWidth = "10%";

            Country.ReadOnly = true;
            Country.ColumnWidth = "10%";
            Country.CssClass = "text-center";

            NumAudits.ReadOnly = true;
            NumAudits.ColumnWidth = "5%";
            NumAudits.CssClass = "text-center";

            LastAuditDate.ReadOnly = true;
            LastAuditDate.ColumnWidth = "10%";
            LastAuditDate.CssClass = "text-center";

            AverageRating.ReadOnly = true;
            AverageRating.ColumnWidth = "5%";
            AverageRating.CssClass = "text-center";

            AverageNC.ReadOnly = true;
            AverageNC.ColumnWidth = "5%";
            AverageNC.CssClass = "text-center";

            HasHNCZT.ReadOnly = true;
            HasHNCZT.ColumnWidth = "5%";
            HasHNCZT.CssClass = "text-center";

            ToAudit.ReadOnly = false;
            ToAudit.ColumnWidth = "10%";
            HasHNCZT.CssClass = "text-center";

        }
        public override void OnLoad()
        {
            base.OnLoad();

            var vahs = Context.BO.All<PlantAuditHistory>(0, 3000)
                .Where(vah => vah.Plant.Vendor != null && vah.Plant.Vendor.Id == BoundItem.Id)
                .ToList();

            var vn = $"{BoundItem.Vendor.Name} ({string.Join(", ", BoundItem.Vendor.Brand.Select(b => b.Name))})";

            VendorName.Value = vn;
            CategoryName.Value = BoundItem.Vendor.Category.Name;
            NumAudits.Value = vahs.Count;
            LastAuditDate.Value = vahs.Count > 0 ? vahs.Max(vah => vah.AuditDate) : null;
            AverageRating.Value = vahs.Count > 0 ? (int)vahs.Average(vah => vah.RespRating) : null;
            AverageNC.Value = vahs.Count > 0 ? (int)vahs.Average(vah => vah.RespNC) : null;
            HasHNCZT.Value = vahs.Any(vah => vah.RespHNCZT && vah.AuditDate.Year == DateTime.Now.Year) ? "Yes" : "No";
        }
    }
}
