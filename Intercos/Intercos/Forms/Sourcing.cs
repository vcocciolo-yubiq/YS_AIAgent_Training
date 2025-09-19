using YubikStudioCore.Forms.Fields;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Intercos.WorkItems;
using Intercos.BusinessObjects;
using Intercos.CodeLibs;

namespace Intercos.Forms
{
    public class SourcingView : PackagingView
    {

        [Unbound]
        public virtual BoLookupField<Vendor> PriEffectiveVendor { get; set; }
        [Unbound]
        public virtual BoLookupField<Vendor> SecEffectiveVendor { get; set; }
        [Unbound]
        public virtual DecimalField PriEffectiveCost { get; set; }
        [Unbound]
        public virtual DecimalField SecEffectiveCost { get; set; }
        [Unbound]
        public virtual HtmlPart PriDiffPrice { get; set; }
        [Unbound]
        public virtual HtmlPart SecDiffPrice { get; set; }
        public override void OnLoad()
        {
            base.OnLoad();
            if (Context.Item.PrimaryPackageObj != null)
            {
                PriEffectiveVendor.Value = Context.Item.PrimaryPackageObj.EffectiveVendor;
                PriEffectiveCost.Value = Context.Item.PrimaryPackageObj.EffectiveCost;
            }
            if (Context.Item.SecondaryPackageObj != null)
            {
                SecEffectiveVendor.Value = Context.Item.SecondaryPackageObj.EffectiveVendor;
                SecEffectiveCost.Value = Context.Item.SecondaryPackageObj.EffectiveCost;
            }
            if (PriEffectiveCost.Value != null && PriEffectiveCost.Value > 0)
            {
                PriDiffPrice.RawHTML = PackagingLib.GetDiffHtml(Context.Item.PrimaryPackageObj.EstCost, PriEffectiveCost?.Value ?? 0);
            }
            if (SecEffectiveCost.Value != null && SecEffectiveCost.Value > 0)
            {
                SecDiffPrice.RawHTML = PackagingLib.GetDiffHtml(Context.Item.SecondaryPackageObj.EstCost, SecEffectiveCost?.Value ?? 0);
            }
        }
        public override FormPart GetLayout()
        {
            var layout = base.GetLayout();

            var colPriEffectiveVendor = Col(PriEffectiveVendor);
            colPriEffectiveVendor.ColumnWidth = "col-6 mb-5";
            var colPriEffectiveCost = Col(PriEffectiveCost);
            colPriEffectiveCost.ColumnWidth = "col-6 mb-5";
            var colSecEffectiveVendor = Col(SecEffectiveVendor);
            colSecEffectiveVendor.ColumnWidth = "col-6 mb-5";
            var colSecEffectiveCost = Col(SecEffectiveCost);
            colSecEffectiveCost.ColumnWidth = "col-6 mb-5";

            var colPrimary = Row(colPriEffectiveVendor, colPriEffectiveCost, PriDiffPrice);
            var colSecondary = Row(colSecEffectiveVendor, colSecEffectiveCost, SecDiffPrice);

            var cardSourcing =
            Card(title: "Definizione sourcing",
                Row(RawHtml(PackagingLib.GetSeparatorTitle("Imballaggio primario"))),
                colPrimary,
                Row(RawHtml("<div class='mb-7'></div>")),
                Row(RawHtml(PackagingLib.GetSeparatorTitle("Imballaggio secondario"))),
                colSecondary
            );
            return Flat(cardSourcing, layout);
        }
    }

}
