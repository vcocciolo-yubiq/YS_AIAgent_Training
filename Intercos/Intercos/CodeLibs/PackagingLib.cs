using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.CodeLibraries;
using Intercos.WorkItems;

namespace Intercos.CodeLibs
{
  public class PackagingLib : CodeLibrary<PackagingWI>
  {
    public PackagingLib(ActionContext ctx) : base(ctx)
    {
    }

    public static string GetDiffHtml(decimal startPrice, decimal endPrice)
    {
      if (startPrice == 0 && endPrice == 0)
        return string.Empty;

      var diff = endPrice - startPrice;
      var percent = startPrice != 0 ? (diff / startPrice) * 100 : 0;
      var arrow = diff >= 0 ? "fa-arrow-up" : "fa-arrow-down";
      var badgeClass = diff <= 0 ? "badge-light-success" : "badge-light-danger";

      return @$"  <div class=""mb-7"">
                    <div class=""d-flex align-items-center mb-2"">   
                        <span class=""fs-4 fw-semibold text-gray-500 me-1 mt-n1 alignt-self-start"">€</span>
                        <span class=""fs-2x fw-bold text-gray-800 me-2 lh-1 ls-n2"">{diff.ToString("N2")}</span>
                        <span class=""badge {badgeClass} fs-base"">                                
                           <i class=""fa-solid {arrow}""></i>{percent.ToString("N2")}%</span>  

                    </div>
                    <span class=""fs-6 fw-semibold text-gray-500"">Differenza rispetto a costo stimato</span>
                </div>
                ";
    }

    public static string GetSeparatorTitle(string title)
    {
      return $@"<div class=""d-flex flex-column""><div class=""fs-5 fw-bold text-primary"">{title}</div><div class='separator mb-4 '></div></div>";
    }
  }

}
