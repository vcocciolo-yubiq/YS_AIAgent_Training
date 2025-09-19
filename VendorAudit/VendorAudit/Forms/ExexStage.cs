using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VendorAudit.WorkItems;
using VendorAudit.BusinessObjects;
using YubikStudioCore.Forms.Fields;

namespace VendorAudit.Forms
{
  public class ExecStage : Form<ExecutionWI>
  {

    public virtual DateField ExecStart { get; set; }
    public virtual DateField ExecEdit { get; set; }
    public virtual DateField ExecFinish { get; set; }
    public virtual IntField RespTotRequirements { get; set; }
    public virtual IntField RespRequirements { get; set; }
    public virtual IntField RespNC { get; set; }
    [Unbound]
    public virtual TextField RespHNCZT2 { get; set; }
    [Unbound]
    public virtual TextField RespRating2 { get; set; }

    public override void OnLoad()
    {
      base.OnLoad();
      RespHNCZT2.Value = Context.Item.RespHNCZT ? "Yes" : "No";
      RespRating2.Value = Context.Item.RespRating !=null ? Context.Item.RespRating.ToString() + "/100" : "--";
    }


    public override FormPart GetLayout()
    {
      RespTotRequirements.CssClass = "text-start";
      RespRequirements.CssClass = "text-start";
      RespNC.CssClass = "text-start";
      RespHNCZT2.CssClass = $" text-start " + (Context.Item.RespHNCZT ? "text-danger" : "");

      var r1 = Row(Col(ExecStart), Col(ExecEdit), Col(ExecFinish));
      var r2 = Row(Col(RespRequirements), Col(RespTotRequirements), Col(RespNC), Col(RespHNCZT2), Col(RespRating2));


      return Flat(r1, r2);
    }
    public override void ConfigureFields()
    {
      base.ConfigureFields();
      ExecStart.ReadOnly = true;
      ExecEdit.ReadOnly = true;
      ExecFinish.ReadOnly = true;
    }
  }

}
