using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.CodeLibraries;
using VendorAudit.WorkItems;
using VendorAudit.BusinessObjects;

namespace VendorAudit.CodeLibs
{
  public class VendorAuditLib : CodeLibrary<ExecutionWI>
  {
    public void GenerateResponse()
    {
      //Create CheckListAnswers Collection based on AuditType
      var q = Context.BO.Search<AuditRequirement>(x => x.AuditType.Id == Context.Item.AuditCategory.Id, 0, 30).ToArray();
      var coll = new List<AuditResponse>();
      foreach (var item in q)
      {
        var newResp = (new AuditResponse
        {
          WIId = Context.Item.Id,
          AuditRequirement = item,
          //RequirementSection = item.Section,
          //RequirementName = item.Name,
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
    }
    public void TestBO(ActionContext ctx)
    {
      
      var item1 = Context.BO.First<Brand>();
      var item2 = Context.BO.GetById<Brand>("4");
      var items1 = Context.BO.Search<Brand>(x => x.Name.StartsWith("B"), 1, 10);
      var numitems1 = items1.Count();
      var vend1 = Context.BO.First<Vendor>();
      var vend1brands1 = vend1.Brand;
      var name1 = vend1brands1.First().Name;
      var brands2 = vend1brands1.OrderBy(x => x.Name);
      var auditor1 = Context.BO.First<AuditorCalendar>();

      var AudQuest1 = Context.BO.First<AuditRequirement>();
      var AudQuest2 = Context.BO.All<AuditRequirement>(0, 30).ToArray();
      var AudQuest3 = Context.BO.GetById<AuditRequirement>("135");
      var AudQuest4 = Context.BO.Search<AuditRequirement>(x => x.AuditType.Id == 1, 0, 30).ToArray();
      var AudQuest5 = Context.BO.Search<AuditRequirement>(x => x.AuditType.Id == 1, 0, 30).ToArray();
      var AudQuest6 = Context.BO.Search<AuditRequirement>(x => x.Name.StartsWith("N"), 0, 30).ToArray();

      var AudCal1 = Context.BO.First<AuditorCalendar>();
      var AudCal2 = Context.BO.All<AuditorCalendar>(0, 30).ToArray();
      var AudCal3 = Context.BO.GetById<AuditorCalendar>("1");
      var AudCal4 = Context.BO.Search<AuditorCalendar>(x => x.Auditor.UserName == "IsabellaNaarro", 0, 30).ToArray();

      var AudResp1 = Context.BO.GetById<AuditResponse>("1");

      var name = item1.Name;
      Context.BO.Update(item1);
    }

    public VendorAuditLib(ActionContext ctx) : base(ctx)
    {
    }
  }

}
