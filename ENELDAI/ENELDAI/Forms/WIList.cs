using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ENELDAI.WorkItems;

namespace ENELDAI.Forms
{
  public class WIList : Form<TestDAIWI>
  {

    public virtual DocumentField Document { get; set; }
  }

}
