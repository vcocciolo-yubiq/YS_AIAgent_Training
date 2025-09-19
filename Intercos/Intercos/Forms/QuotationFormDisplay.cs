using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Forms.Fields;
using Intercos.WorkItems;

namespace Intercos.Forms
{
  public class QuotationFormWI : Form<QuotationWI>
  {

    public virtual TextField Description { get; set; }
    public override FormPart GetLayout()
    {
      return Flat(Row(Col(Description)));
    }
  }

}
