using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Intercos.WorkItems;
using Intercos.BusinessObjects;

namespace Intercos.Forms
{
  public class SendToFormulaDef : Form<QuotationWI>
  {

    protected override IEnumerable<ValErr> ExtendedValidation()
    {
      // if (condition)
      // yield return new ValErr(nameof(PropertyName), L.T("LocalizationKey"));
      return base.ExtendedValidation();
    }
  }

}
