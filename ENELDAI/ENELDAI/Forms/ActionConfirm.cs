using YubikStudioCore.Forms.Fields;
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
  public class ActionConfirm : Form<TestDAIWI>
  {

    [Unbound]
    public virtual TextField ActionMsg { get; set; }

    [Unbound]
    public virtual MemoField ActionNote { get; set; }
    public override FormPart GetLayout()
    {
      return Flat(
        Row(title: ActionMsg.Value),
        Row(Col(ActionNote))
    );
    }
    public override void OnLoad()
    {
      base.OnLoad();
      
    }
  }

}
