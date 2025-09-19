using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using EuropeData.BusinessObjects;
using EuropeData.StaticRoles;
using EuropeData.DynamicRoles;
using EuropeData.Forms;
using EuropeData.WorkItems;
using EuropeData.CodeLibs;

namespace EuropeData.Workflows
{
  [Visibility<Admin>]
  [Workflow("eeb22d08-f2fa-4b91-8a1c-46cc9b9c7e0a", "WI", 0)]
  [Form<WIList>]
  public class ChartData : Workflow<ChartDataWI>
  {
    [Visibility<Admin>]
    [Action<Stage1>]
    public class CreateItem : YAction
    {
      public override void OnExecute()
      {
        base.OnExecute();
      }

    }

    [Form<WIDetails>]
    public class Stage1 : Stage
    {

      [Action<Stage1>]
      [Visibility<Admin>]
      public class PopulationArea : YAction
      {
        public virtual Forms.FrmPopulationArea Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

      }

      [Action<Stage1>]
      [Visibility<Admin>]
      public class Obesity : YAction
      {
        public virtual Forms.FrmObesity Frm { get; set; }
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
