using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YubikStudioCore.Forms.Fields;
using EuropeData.WorkItems;

namespace EuropeData.Forms
{
  public class WIList : Form<ChartDataWI>
  {

    public override FormPart GetLayout()
    {
      return Grid(2);
    }
  }

}
