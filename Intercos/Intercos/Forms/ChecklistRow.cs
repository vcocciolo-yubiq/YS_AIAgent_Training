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

namespace Intercos.Forms
{
  public class ChecklistRow : SubForm<Checklist>
  {

    public virtual TextField NomeFile { get; set; }
    public virtual DocumentField Documento { get; set; }
    public virtual TextField TipoRisorsa { get; set; }
    [Unbound]
    public virtual ButtonField Approva { get; set; }
  }

}
