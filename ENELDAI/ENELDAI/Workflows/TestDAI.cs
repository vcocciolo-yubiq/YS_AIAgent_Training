using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using ENELDAI.BusinessObjects;
using ENELDAI.StaticRoles;
using ENELDAI.DynamicRoles;
using ENELDAI.Forms;
using ENELDAI.WorkItems;
using ENELDAI.CodeLibs;

namespace ENELDAI.Workflows
{
  [Visibility<Admin>]
  [Workflow("082f77e4-9918-42cf-9fa8-2f6d94f05956", "WI", 0)]
  [Form<WIList>]
  public class APInvoice : Workflow<TestDAIWI>
  {
    [Visibility<Admin>]
    [Action<TestBed>(Modal = ModalMode.XtraLarge)]
    public class NewInvoice : YAction
    {
      public virtual FrmInvoice Frm { get; set; }
      public override void OnExecute()
      {
        base.OnExecute();
      }

    }
    // [Todo<Admin>]
    // [Watch<Admin>]
    // [ThirdState<Admin>]
    // [Form<Form1>]
    [Form<WIStage>]
    [Form<FrmInvoice>]

    public class TestBed : Stage
    {


      [Action<Archive>(Modal = ModalMode.Medium)]
      [Visibility<Admin>]
      public class Delete : YAction
      {
        public virtual ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

      }


    }
    // [Todo<Admin>]
    // [Watch<Admin>]
    [Archive]
    public class Archive : Stage
    {

    }

  }

}
