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
  public class PackagingFormWI : Form<PackagingWI>
  {

    public virtual TextField Title { get; set; }
    public override FormPart GetLayout()
    {
      return Flat(Title);
    }
  }

}
