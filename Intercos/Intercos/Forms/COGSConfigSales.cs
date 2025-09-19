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
    public class COGSConfigSales : FormulaDefView
    {

        public override BoLookupField<Package> PrimaryPackaging { get; set; }
        public override BoLookupField<Package> SecondaryPackaging { get; set; }
        [Unbound]
        public override TextField PrimaryPackageName { get; set; }
        [Unbound]
        public virtual EnumField<PackageType> PrimaryPackageType { get; set; }
        [Unbound]
        public override BoLookupField<Vendor> PrimaryPotentialVendor { get; set; }
        [Unbound]
        public override TextField PrimaryPackageEstCost { get; set; }

        [Unbound]
        public override TextField SecondaryPackageName { get; set; }
        [Unbound]
        public virtual EnumField<PackageType> SecondaryPackageType { get; set; }
        [Unbound]
        public override BoLookupField<Vendor> SecondaryPotentialVendor { get; set; }
        [Unbound]
        public override TextField SecondaryPackageEstCost { get; set; }

        [Unbound]
        public virtual HtmlPart FormulaStat { get; set; }
        public override FormPart GetLayout()
        {

            var layout = base.GetLayout();


            return layout;


            
        }
        public override void OnLoad()
        {
            base.OnLoad();
            PrimaryPackageName.Value = Context.Item.PrimaryPackaging.Name;
            // PrimaryPackageType.Value = Context.Item.PrimaryPackaging.Type;
            PrimaryPotentialVendor.Value = Context.Item.PrimaryPackaging.PotentialVendor;
            PrimaryPackageEstCost.Value = Context.Item.PrimaryPackaging.EstCost.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);

            SecondaryPackageName.Value = Context.Item.SecondaryPackaging.Name;
            // SecondaryPackageType.Value = Context.Item.SecondaryPackaging.Type;
            SecondaryPotentialVendor.Value = Context.Item.SecondaryPackaging.PotentialVendor;
            SecondaryPackageEstCost.Value = Context.Item.SecondaryPackaging.EstCost.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);

            //Calculate total material cost based on ingredients and their quantities
            TotalMaterialCost.Value = Formula.Value?.Ingredients?.Sum(i => i.Quantity * i.Material?.Price ?? 0) ?? 0;

            Stats.RawHTML = QuotationLib.GetStatsHtml(
                PercProdItems.Value ?? 0,
                PercValue.Value ?? 0
            );
            FormulaStat.RawHTML = QuotationLib.GetStatsHtml(
                PercIngredients.Value ?? 0
            );
            FormulaStat.RawHTML += QuotationLib.GetTotalCost(
                TotalMaterialCost.Value ?? 0
            );

        }
    }

}
