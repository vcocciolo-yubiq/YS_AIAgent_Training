using YubikStudioCore.Forms.Fields;
using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EuropeData.WorkItems;
using EuropeData.BusinessObjects;

namespace EuropeData.Forms
{
    public class FrmObesity : Form<ChartDataWI>
    {

        [Unbound]
        public virtual BoLookupField<Obesity> Country { get; set; }

        [Unbound]
        public virtual BoLookupField<Obesity> Sex { get; set; }
    

  }

}
