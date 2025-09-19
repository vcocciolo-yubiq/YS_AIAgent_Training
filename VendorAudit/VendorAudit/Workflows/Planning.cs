using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using VendorAudit.Forms;
using VendorAudit.WorkItems;
using VendorAudit.BusinessObjects;
using VendorAudit.StaticRoles;
using VendorAudit.CodeLibs;
using System.Reflection.Metadata;
using System.Diagnostics.Contracts;
using VendorAudit.DynamicRoles;
using YubikStudioCore.Runtime;
using System.ComponentModel;

namespace VendorAudit.Workflows
{
  [Visibility<
    Admin, AuditorCoordinator, Auditor, StaticRoles.Vendor,
    AuditAnalyst, StaticRoles.AuditCommiteeOwner, StaticRoles.Kering
  >]

  [Workflow("2abcd93b-e0ef-4f23-a1aa-4902554c5976", "WI", Order = 2)]

  [Form<PlanList>]
  public class Execution : Workflow<ExecutionWI>
  {
    //------------------------------ Creation Item ------------------------------

    [Action<StgAuditorInvite>(Modal = ModalMode.None, TodoList = false)]
    [Visibility<
      Admin,
      StaticRoles.AuditorCoordinator
    >]
    public class AuditCreate : YAction
    {
      public Forms.VendorAuditCreate Frm { get; set; }
      public override void OnExecute()
      {
        Context.Item.Title = $"Audit for : {Frm.Vendor.Value.Name} ({Frm.Brand.Value.Name}) by {Frm.AssignedAuditor.Value.FullName}";
        Context.Item.AssignedAuditorCoordinator = Frm.AssignedAuditorCoordinator.Value;
        Context.Item.IsExternalAuditor = Context.Item.AssignedAuditor.Profile["Position"] == "Extern";
      }

    }

    //------------------------------ Stage 1) Planning ------------------------------
    [Todo<
      Admin,
      DynamicRoles.AssignedAuditorCoordinator
    >]

    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    public class StgPlanning : Stage
    {
      public override void OnEnter()
      {
        Context.Item.AssignedVendorContact = Context.Item.Vendor.ContactUser;
        Context.Item.Category = Context.Item.Vendor.Category;

        //Lib<VendorAuditLib>().AddField();
      }

      //- - - - - - - - - - - - - - - Action 2) AuditorInvite - - - - - - - - - - - - - -

      [Action<StgAuditorInvite>(Modal = ModalMode.Medium, Order = 1, TodoList = false)]
      [Visibility<Admin, StaticRoles.AuditorCoordinator>]
      public class AuditorInvite : YAction
      {
        public virtual FrmAuditorInvite Frm { get; set; }

        public override void OnPrepare()
        {
          base.OnPrepare();
        }
        public override void OnExecute()
        {
          base.OnExecute();
        }

      }

      //- - - - - - - - - - - - - - - Action 3) PutOnHold - - - - - - - - - - - - - -

      [Action<StgOnHold>(Modal = ModalMode.Medium, Order = 2, TodoList = false)]
      [Visibility<Admin, StaticRoles.AuditorCoordinator>]
      public class PutOnHold : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Put the Audit on Hold?";
          Frm.ActionNote.Required = true;
        }
      }

    }

    //------------------------------ Stage 2) OnHold ------------------------------
    [Todo<
      Admin,
      DynamicRoles.AssignedAuditorCoordinator
    >]

    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    public class StgOnHold : Stage
    {
      //- - - - - - - - - - - - - - - Action 1) Recover - - - - - - - - - - - - - -

      [Action<StgPlanning>(Modal = ModalMode.Medium, Order = 1, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditorCoordinator
      >]
      public class Recover : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Recover the Audit?";
          Frm.ActionNote.Required = true;
        }
      }
    }

    //------------------------------ Stage 3) AuditorInvite ------------------------------
    [Todo<
      Admin,
      DynamicRoles.AssignedAuditorCoordinator,
      DynamicRoles.AssignedAuditor
    >]
    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    public class StgAuditorInvite : Stage
    {

      //- - - - - - - - - - - - - - - - - - Action 1) AuditPlan - - - - - - - - - - - -

      [Action<StgAuditorInvite>(Modal = ModalMode.None, Order = 1, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditor
      >]
      public class AuditPlan : YAction
      {
        public virtual FrmAuditorPlanEdit Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();

          /*
          Context.Item.AssignedVendorContact.Email = Frm.ContactEmail.Value;
          Context.Item.Vendor.WebSite = Frm.WebSite.Value;
          Context.Item.Vendor.SiteYear = (int)Frm.PlantYear.Value;
          Context.Item.Vendor.SiteSQM = (int)Frm.PlantSQM.Value;
          Context.Item.Vendor.EmployeeNumber = (int)Frm.PlantEmployeeNumber.Value;
          Context.Item.Vendor.MaxProdCap = (int)Frm.PlantMaxProdCap.Value;
          Context.Item.Vendor.SiteCharacteristics = Frm.PlantCharacteristics.Value;
          Context.Item.Vendor.SiteAddress = Frm.PlantAddress.Value;
          */

          //Context.Item.Title = $"{Context.Item.AuditCategory.Name} Audit for : {Context.Item.Vendor.Name} ({Context.Item.Brand.Name}) on {Context.Item.Process.Name} by {Context.Item.AssignedAuditor.FullName}";

        }

        public override void OnPrepare()
        {
          base.OnPrepare();
        }
      }
      //- - - - - - - - - - - - - - - - - - Action 2) Refuse - - - - - - - - - - - -

      [Action<StgPlanning>(Modal = ModalMode.Medium, Order = 2, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditor
      >]
      public class Refuse : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Refuse the Audit? Please explain the reason of refusal";
          Frm.ActionNote.Required = true;
        }
      }

      //- - - - - - - - - - - - - - - - - - Action 2) Recall - - - - - - - - - - - -

      [Action<StgPlanning>(Modal = ModalMode.Medium, Order = 3, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditorCoordinator
      >]
      public class Recall : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Refuse the Audit? Please explain the reason of the Audit Recall";
          Frm.ActionNote.Required = true;
        }
      }

      //- - - - - - - - - - - - - - - - - - Action 4) Invite Vendor - - - - - - - - - - - -

      [Action<StgVendorInvite>(Modal = ModalMode.Medium, Order = 4, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditor
      >]
      public class VendorInvite : YAction
      {
        public virtual Forms.VendorInvite Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
          Context.Item.AssignedVendorContact = Runtime.Instance.GetUser("MarkDonovan");
        }

        public override bool CanView()
        {
          return Context.Item.AuditDate != null && Context.Item.AuditType != null;


        }
      }
    }

    //------------------------------ Stage 4) VendorInvite ------------------------------
    [Todo<
      Admin,
      DynamicRoles.AssignedVendorContact,
      DynamicRoles.AssignedAuditor
    >]

    [Watch<
      Admin,
      DynamicRoles.AssignedAuditorCoordinator
    >]

    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    public class StgVendorInvite : Stage
    {

      //- - - - - - - - - - - - - - - - - - Action 1) Accept - - - - - - - - - - - -

      [Action<StgExecuting>(Modal = ModalMode.Medium, Order = 1, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedVendorContact
      >]
      public class Accept : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();

          //Create CheckListAnswers Collection based on AuditType
          var q = Context.BO.Search<AuditRequirement>(x => x.AuditType.Id == Context.Item.AuditType.Id, 0, 30).ToArray();

          var coll = new List<AuditResponse>();
          foreach (var item in q)
          {
            var newResp = (new AuditResponse
            {
              WIId = Context.Item.Id,
              AuditRequirement = item,
              HNC = false,
              ZT = false,
              HNCZT = false,
              AuditResponseStatus = Context.BO.GetById<AuditResponseStatus>("1"),
              Observation = "",
              CorrectiveAction = "",
              DocumentEvidence = null,
              Responsible = "",
              DaysExpire = 0,
              Verified = false
            });
            //Context.BO.Insert<AuditResponse>(newAnswer);
            coll.Add(newResp);
          }
          Context.Item.AuditResponse = coll;

          Context.Item.VendorReport = Context.DOCS.GetById("1");
          Context.Item.ExcelChecklistDL = Context.DOCS.GetById("2");

          Context.Item.Attachments.Attach(Context.Item.VendorReport);
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Thanks to accept the audit invitation";
          Frm.ActionNote.Required = false;
        }
      }
      //- - - - - - - - - - - - - - - - - - Action 2) Refuse - - - - - - - - - - - -

      [Action<StgAuditorInvite>(Modal = ModalMode.Medium, Order = 2, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedVendorContact
      >]
      public class Refuse : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Please explain the reason of refusal";
          Frm.ActionNote.Required = true;
        }
      }
      //- - - - - - - - - - - - - - - - - - Action 3) Recall - - - - - - - - - - - -

      [Action<StgAuditorInvite>(Modal = ModalMode.Medium, Order = 3, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditor
      >]
      public class Recall : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Please explain the reason of the Audit Recall";
          Frm.ActionNote.Required = true;
        }
      }
    }

    //------------------------------ Stage 4) Executing ------------------------------

    [Todo<
      Admin,
      DynamicRoles.AssignedAuditor
    >]

    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    [Form<Forms.AuditStage>(Order = 3)]
    public class StgExecuting : Stage
    {

      //- - - - - - - - - - - - - - - - - - Action 1) CheckList Edit - - - - - - - - - - - -

      [Action<StgExecuting>(Modal = ModalMode.None, Order = 1, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditor
      >]
      public class CheckListEdit : YAction
      {
        public virtual Forms.ExecEdit Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();

          // Execution Date
          if (Context.Item.ExecStart == null)
            Context.Item.ExecStart = DateOnly.FromDateTime(DateTime.Now);
          Context.Item.ExecEdit = DateOnly.FromDateTime(DateTime.Now);

          //Init WI Resp fields
          Context.Item.RespHNC = false;
          Context.Item.RespZT = false;
          Context.Item.RespHNCZT = false;
          Context.Item.RespNC = 0;
          Context.Item.RespRequirements = 0;
          Context.Item.RespScore = 0;
          Context.Item.RespRating = 0;
          Context.Item.RespTotRequirements = Context.Item.AuditResponse.Count;
          bool HasHNC = false;
          bool HasZT = false;


          int i = 0;
          foreach (var ar in Context.Item.AuditResponse)
          {

            switch (ar.AuditResponseStatus.Id)
            {
              case 1: //No Answer
                ar.CorrectiveAction = "";
                ar.Responsible = "";
                ar.DaysExpire = 0;
                ar.HNC = false;
                ar.ZT = false;
                break;

              case 2: //Partially Compliant
                Context.Item.RespRequirements++;
                Context.Item.RespNC++;
                Context.Item.RespScore = Context.Item.RespScore + (decimal)0.5;
                if (ar.HNC) HasHNC = true;
                if (ar.ZT) HasZT = true;
                break;

              case 3: //Non Compliant
                Context.Item.RespRequirements++;
                Context.Item.RespNC++;
                if (ar.HNC) HasHNC = true;
                if (ar.ZT) HasZT = true;
                break;

              case 4: //Compliant
                ar.CorrectiveAction = "";
                ar.Responsible = "";
                ar.DaysExpire = 0;
                Context.Item.RespRequirements++;
                Context.Item.RespScore++;
                break;

            }
            i++;

          }

          // Check if all requirements are answered
          if (Context.Item.RespRequirements == Context.Item.RespTotRequirements)
          {
            Context.Item.AuditComplete = true;
            Context.Item.ExecFinish = DateOnly.FromDateTime(DateTime.Now);
          }
          else
          {
            Context.Item.AuditComplete = false;
            Context.Item.ExecFinish = null;
          }

          // Calculate final rating
          Context.Item.RespHNC = HasHNC;
          Context.Item.RespZT = HasZT;
          Context.Item.RespHNCZT = HasHNC || HasZT;
          Context.Item.RespRating = Context.Item.RespRequirements != 0 ? (int)(Context.Item.RespScore / Context.Item.RespRequirements * 100) : null;
          if (Context.Item.RespZT)
            Context.Item.RespRating = (int)(Context.Item.RespRating * 0.3m);
          else if (Context.Item.RespHNC)
            Context.Item.RespRating = (int)(Context.Item.RespRating * 0.5m);

        }
      }

      //- - - - - - - - - - - - - - - - - - Action 2) Send To Approve - - - - - - - - - - - -

      [Action<StgApproving>(Modal = ModalMode.Medium, Order = 2, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditor
      >]
      public class SendToApprove : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Send the Audit for Approval?";
          Frm.ActionNote.Required = false;
        }
        public override bool CanView()
        {
          return Context.Item.IsExternalAuditor == true &&
                 Context.Item.AuditComplete == true;
          //return true;
        }
      }

      //- - - - - - - - - - - - - - - - - - Action 3) Send To Finalize - - - - - - - - - - - -

      [Action<StgCheckNC>(Modal = ModalMode.Medium, Order = 4, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditor
      >]
      public class SendToFinalize : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Send the Audit for Finalization?";
          Frm.ActionNote.Required = false;
        }
        public override bool CanView()
        {
          base.CanView();

          return Context.Item.IsExternalAuditor == false &&
                 Context.Item.AuditComplete == true;
          //return false;
        }
      }

      [Action<StgExecuting>(Modal = ModalMode.Medium, Order = 3, TodoList = false)]
      [Visibility<
        Admin,
        DynamicRoles.AssignedAuditor
      >]
      public class ExcelEdit : YAction
      {
        public virtual Forms.ExcelEdit form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

      }
      //- - - - - - - - - - - - - - - - - - Action 5) Admin Restarrt - - - - - - - - - - - -

      [Action<StgPlanning>(Modal = ModalMode.Medium, Order = 5, TodoList = false)]
      [Visibility<Admin>]
      public class AdminRestart : YAction
      {
        public virtual ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Restart the Audit?";
          Frm.ActionNote.Required = false;
        }
      }
    }

    //------------------------------ Stage 5) Approving ------------------------------

    [Todo<
      Admin,
      StaticRoles.Kering
    >]

    [Watch<
      Admin,
      DynamicRoles.AssignedAuditorCoordinator,
      DynamicRoles.AssignedAuditor
    >]

    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    public class StgApproving : Stage
    {

      //- - - - - - - - - - - - - - - - - - Action 1) Approve - - - - - - - - - - - -

      [Action<StgCheckNC>(Modal = ModalMode.Medium, Order = 1, TodoList = false)]

      [Visibility<
        Admin,
        StaticRoles.Kering
      >]
      public class Approve : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Do you approve the audit?";
          Frm.ActionNote.Required = false;
        }
      }
      //- - - - - - - - - - - - - - - - - - Action 2) Refuse - - - - - - - - - - - -

      [Action<StgExecuting>(Modal = ModalMode.Medium, Order = 2, TodoList = false)]
      [Visibility<
        Admin,
        StaticRoles.Kering
      >]
      public class Refuse : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Plese, explain the reason of refusal";
          Frm.ActionNote.Required = true;
        }
      }
    }

    //------------------------------ Stage 6) CheckNC ------------------------------
    [Todo<Admin>]
    public class StgCheckNC : Stage
    {

      [Action<StgPrepCommittee>(TodoList = false, WatchList = false, Order = 1)]
      [Visibility<Admin>]
      public bool HNCZTYes()
      {
        return Context.Item.RespHNCZT == true;
      }

      [Action<StgFinalizing>(TodoList = false, WatchList = false, Order = 2)]
      [Visibility<Admin>]
      public bool HTCZTNo()
      {
        return Context.Item.RespHNCZT == false;
      }
    }

    //------------------------------ Stage 7) Finalizing ------------------------------
    [Todo<
      Admin,
      DynamicRoles.AssignedAuditorCoordinator,
      DynamicRoles.AssignedAuditor
    >]

    [Watch<
      Admin,
      DynamicRoles.AssignedAuditorCoordinator,
      DynamicRoles.AssignedAuditor
    >]

    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    [Form<Forms.AuditStage>(Order = 3)]
    public class StgFinalizing : Stage
    {

      //- - - - - - - - - - - - - - - - - - Action 1) SendReport - - - - - - - - - - - -

      [Action<StgFinalizing>(Modal = ModalMode.Large, TodoList = true, WatchList = false, Order = 1)]
      public class SendReport : YAction
      {
        public virtual VendorResult form { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }
      }

      //- - - - - - - - - - - - - - - - - - Action 2) ToBeContinue - - - - - - - - - - - -

      [Action<StgArchive>(Modal = ModalMode.Medium, TodoList = true, WatchList = false, Order = 2)]
      public class ToBeContinue : YAction
      {
        public virtual Forms.ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Thats all for now! Please, send the Audit to be archived";
          Frm.ActionNote.Required = false;
        }
      }

    }


    //------------------------------ Stage 8) PrepCommittee ------------------------------
    [Todo<
      Admin,
      StaticRoles.AuditCommiteeOwner
    >]
    [Watch<
      DynamicRoles.AssignedAuditorCoordinator,
      DynamicRoles.AssignedAuditor
    >]

    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    public class StgPrepCommittee : Stage
    {


      [Action<StgArchive>(Modal = ModalMode.Medium, TodoList = true, WatchList = false, Order = 1)]
      [Visibility<Admin>]
      public class ToBeContinue : YAction
      {
        public virtual ActionConfirm Frm { get; set; }
        public override void OnExecute()
        {
          base.OnExecute();
        }

        public override void OnPrepare()
        {
          base.OnPrepare();
          Frm.ActionMsg.Value = "Thats all for now!Please, send the Audit to be archived";

        }
      }
    }

    //------------------------------ Stage 9) Archive ------------------------------
    [Todo<Admin>]
    [Archive]
    [Form<Forms.PlanStage>(Order = 1)]
    [Form<Forms.ExecStage>(Order = 2)]
    public class StgArchive : Stage
    {

    }

  }

}
