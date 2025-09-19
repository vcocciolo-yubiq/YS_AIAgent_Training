using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Runtime;
using YubikStudioCore.Forms.Attributes;
using VendorAudit.WorkItems;
using VendorAudit.CodeLibs;
using VendorAudit.BusinessObjects;
using YubikStudioCore.Forms.Fields;
using static YubikStudioCore.Forms.MapPart;
using System.Text;

namespace VendorAudit.Forms
{
  public class AuditPlanCreate : Form<PlannerWI>
  {

    //-------------- Tab 1 - Vendors Selection --------------

    [Unbound]
    public virtual BoLookupField<Vendor> Vendor { get; set; }
    public virtual BoLookupField<Brand> Brand { get; set; }

    [Unbound]
    public virtual BoLookupField<Category> Category { get; set; }

    public virtual BoLookupField<Country> Country { get; set; }

    [Unbound]
    public virtual TableField<Plant, PlantAuditRow> PlantVendorAuditList { get; set; }

    [Unbound]
    public virtual BoLookupField<Plant> SelectedPlant { get; set; }

    [Unbound]
    public virtual TableField<PlantAuditHistory, PlantAuditHistoryRow> PlantAuditHistoryTable { get; set; }

    //-------------- Tab 2 - Plant Location --------------
    [Unbound]
    public virtual MapPart PlantMap { get; set; }

    //-------------- Tab 3 - Reputation --------------------

    [Unbound]
    public virtual HtmlPart AIReputationTitleHTML { get; set; }

    [Unbound]
    public virtual HtmlPart AIReputationContentHTML { get; set; }

    [Unbound]
    public virtual ButtonField AIReputationRefresh { get; set; }

    //-------------- Tab 4 - Auditor Selection --------------

    [Unbound]
    public virtual IntLookupField AuditorYear { get; set; }
    [Unbound]
    public virtual TableField<VolatileCalendarRow, VolatileCalendarRowForm> AuditorCalendar { get; set; }
    [Unbound]
    public virtual TableField<AuditorCalendar, AuditorCalendarRow> AuditorCalendarTable { get; set; }
    public virtual UserLookupField AssignedAuditor { get; set; }
    public virtual BoLookupField<Process> Process { get; set; }
    public virtual BoLookupField<AuditCategory> AuditCategory { get; set; }
    public virtual DateField AuditDate { get; set; }

    public override void ConfigureFields()
    {
      base.ConfigureFields();

      Vendor.OnGetOptions = () => [.. Context.BO.All<Vendor>(0, 100).OrderBy(x => x.Name)];

      Brand.Required = true;
      Brand.OnGetOptions = () => [.. Context.BO.All<Brand>(0, 100).OrderBy(x => x.Name)];
      
      Country.Required = true;

      PlantVendorAuditList.DependsOn = [nameof(Vendor), nameof(Brand), nameof(Category), nameof(Country)];
      PlantVendorAuditList.SelectTarget = nameof(SelectedPlant);
      PlantVendorAuditList.CanSort = true;
      PlantVendorAuditList.CssClass = "";
      PlantVendorAuditList.IsPaged = true;
      PlantVendorAuditList.PageSize = 10;

      SelectedPlant.IsVisible = false;

      PlantAuditHistoryTable.ReadOnly = true;
      PlantAuditHistoryTable.DependsOn = [nameof(SelectedPlant)];
      PlantAuditHistoryTable.IsPaged = true;
      PlantAuditHistoryTable.PageSize = 10;

      PlantMap.DependsOn = [nameof(Vendor), nameof(Brand), nameof(Category), nameof(Country)];

      AIReputationTitleHTML.DependsOn = [nameof(AIReputationRefresh), nameof(SelectedPlant)];
      AIReputationContentHTML.DependsOn = [nameof(AIReputationRefresh), nameof(SelectedPlant)];
      AIReputationRefresh.Label = "Refresh Reputation";

      AuditorYear.OnGetOptions = () => [DateTime.Now.Year, DateTime.Now.Year + 1, DateTime.Now.Year + 2];
      AuditorYear.HasDynamicOptions = false;
      AuditorYear.ReadOnly = false;

      AuditorCalendar.DependsOn = [nameof(AuditorYear), nameof(Country)];
      AuditorCalendar.ReadOnly = true;
      AuditorCalendar.CanSort = false;
      AuditorCalendar.CssClass = "FullCalendar";
      AuditorCalendar.DependsOn = [nameof(AuditorYear)];
      AuditorCalendar.IsPaged = true;
      AuditorCalendar.PageSize = 4;

      AssignedAuditor.Required = true;
      AssignedAuditor.DependsOn = [nameof(Country)];
      AssignedAuditor.HasDynamicOptions = true;
      AssignedAuditor.PageSize = 20;

      AuditorCalendarTable.ReadOnly = true;
      AuditorCalendarTable.DependsOn = [nameof(AssignedAuditor), nameof(Country)];
      AuditorCalendarTable.IsPaged = true;
      AuditorCalendarTable.PageSize = 4;

      AuditDate.MinLimit = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
      AuditDate.MaxLimit = DateOnly.FromDateTime(DateTime.Now.AddYears(1));

      PlantAuditHistoryTable.CssClass = "VATable";
    }

    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);

      if (changedProperties.Contains(nameof(SelectedPlant)))
      {
        PlantAuditHistoryTable.Value = SelectedPlant.Value == null ? []
          : Context.BO.Search<PlantAuditHistory>(x => SelectedPlant.Value == x.Plant, 0, 10).ToList();
        RefreshPlantReputation(false);
      }

      if (changedProperties.Contains(nameof(AIReputationRefresh)))
      {
        RefreshPlantReputation(true);
      }

      if (changedProperties.Contains(nameof(Vendor)) || changedProperties.Contains(nameof(Brand)) ||
          changedProperties.Contains(nameof(Category)) || changedProperties.Contains(nameof(Country)))
      {
        RefreshVendorAuditList();
        RefreshPlantMap();
      }

      if (changedProperties.Contains(nameof(Country)) || changedProperties.Contains(nameof(AuditorYear)))
      {
        RefreshAuditorCalendar();
      }

      if (changedProperties.Contains(nameof(Country)))
      {
        RefreshAssignedAuditor();
      }

    }

    public override void OnLoad()
    {
      base.OnLoad();

      AuditorYear.Value = DateTime.Now.Year;
      AuditDate.Value = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
      AIReputationTitleHTML.RawHTML = "<h3>Reputation</h3><p>Select a Vendor to view its reputation.</p>";

      RefreshVendorAuditList();
      RefreshPlantMap();
      RefreshAssignedAuditor();
      RefreshAuditorCalendar();
    }

    public override FormPart GetLayout()
    {
      // Vendors Selection
      var t1r1 = Row(Col(Vendor), Col(Brand), Col(Category), Col(Country));
      var t1r2 = Row(Col(PlantVendorAuditList));
      var t1r3 = Row(Col(SelectedPlant));
      var t1r4 = Row(Col(PlantAuditHistoryTable));
      var t1 = Flat(t1r1, t1r2, t1r3, t1r4);

      // Plant Location
      var t2r1 = Row(Col(PlantMap));
      var t2 = Flat(t2r1);

      // Plant Location
      var t3r1 = Row(Col(AIReputationTitleHTML), Col(AIReputationRefresh));
      var t3r2 = Row(Col(AIReputationContentHTML));
      var t3 = Flat(t3r1, t3r2);

      // Auditor Selection
      var t4r1 = Row(Col(AuditorYear));
      var t4r2 = Row(Col(AuditorCalendar));
      var t4r3 = Row(Col(AssignedAuditor));
      var t4r4 = Row(Col(Process), Col(AuditCategory), Col(AuditDate));
      var t4 = Flat(t4r1, t4r2, t4r3, t4r4);

      var tabs = Tabs(t1, t2, t3, t4);
      tabs.TabHeaders = ["Vendors Selection", "Plant Location", "Reputation", "Auditor Selection"];
      return Flat(tabs);
    }


    public void RefreshPlantReputation(bool forceRefresh)
    {

      if (SelectedPlant.Value.Name != null)
      {
        if (SelectedPlant.Value.Vendor.AIReputationDoc != null && !forceRefresh)
        {
          AIReputationTitleHTML.RawHTML = $"<h3>Reputation of {SelectedPlant.Value.Vendor.Name} </h3>";
          AIReputationContentHTML.RawHTML = Encoding.UTF8.GetString(SelectedPlant.Value.Vendor.AIReputationDoc.Content);
        }
        else
        if (forceRefresh)
        {
          var s1 = "Chi sono, Attività e gamma prodotti, Innovazione tecnologica e sostenibilità, Certificazioni e standard di qualità, Collaborazioni e partnership, Feedback dei clienti, Riconoscimenti e premi, Presenza sul mercato, Responsabilità sociale d'impresa, Audit e conformità, ";
          var s2 = "Infortuni sul lavoro, Denunce,  Procedimenti o Sanzioni, Acquisizioni, Procedure Concorsuali, Protesti ";
          var f = "Il formato deve essere HTML su due colonne, con ogni sezione ben formattata con icone grafiche della sezione. La lingua Inglese britannico";
          var prompt = $"sei un esperto in risk management. Devi fare  una ricerca di dati e informazioni usando fonti il più possibile attendibili sul web, citandole sempre, per la seguente società {SelectedPlant.Value.Vendor.Name} suddividi le informazioni nelle seguenti sezioni: {s1} {s2}, citare le fonti inserendo i link e ritorna il contenuto in {f}";
          var completion = Context.Lib<PlannerLib>().InvokeAI(prompt);

          if (completion != null && completion.Content.Count > 0)
          {

            Document d = new Document();
            d.Creator = Context.User;
            d.Content = Encoding.UTF8.GetBytes(completion.Content[0].Text);
            d.FileName = $"Plant_{SelectedPlant.Value.Id}_AIReputation.html";
            Context.DOCS.Insert(d);
            SelectedPlant.Value.Vendor.AIReputationDoc = d;
            Context.BO.Update<Plant>(SelectedPlant.Value);

            AIReputationTitleHTML.RawHTML = $"<h3>Reputation of {SelectedPlant.Value.Vendor.Name}</h3>";
            AIReputationContentHTML.RawHTML = completion.Content[0].Text;

          }
          else
          {
            AIReputationTitleHTML.RawHTML = $"<h3>Reputation of {SelectedPlant.Value.Vendor.Name}</h3>";
            AIReputationContentHTML.RawHTML = $"<p>No Return from AI</p>";
          }
        }
        else
        {
            AIReputationTitleHTML.RawHTML = $"<h3>Reputation of {SelectedPlant.Value.Vendor.Name}</h3>";
            AIReputationContentHTML.RawHTML = $"<p>No AI data stored. Please hit the Refresh Button and give time to AI to fetch the Vendor data for you!</p>";         
        }
      }
      else
      {
        AIReputationTitleHTML.RawHTML = "<h3>Select a Vendor Plant to view its reputation<h3>";
        AIReputationContentHTML.RawHTML = "";
      }

    }

    public void RefreshVendorAuditList()
    {
      var ls = Context.BO.Search<Plant>(val =>
            (Vendor.Value == null || val.Vendor == Vendor.Value) &&
            (Brand.Value == null || val.Vendor.Brand.Contains(Brand.Value)) &&
            (Category.Value == null || val.Vendor.Category == Category.Value) &&
            (Country.Value == null || val.Country == Country.Value.Name)
            , 0, 100);

      PlantVendorAuditList.Value = ls.ToList();
    }

    public void RefreshPlantMap()
    {

      var ls = Context.BO.Search<Plant>(val =>
            (Vendor.Value == null || val.Vendor == Vendor.Value) &&
            (Brand.Value == null || val.Vendor.Brand.Contains(Brand.Value)) &&
            (Category.Value == null || val.Vendor.Category == Category.Value) &&
            (Country.Value == null || val.Country == Country.Value.ToString())
            , 0, 100);

      var places = new List<Place>();

      foreach (var plant in ls)
      {
        var place = new Place
        {
          Name = $"{plant.Address} ({plant.Name})",
          Lat = plant.Latitude,
          Lon = plant.Longitude,
          Address = plant.Address
        };
        places.Add(place);
      }

      PlantMap.Places = places.ToArray();

    }

    public void RefreshAuditorCalendar()
    {
      AuditorCalendar.Value = Context.BO.All<AuditorCalendar>(0, 3000)
        .Where(c => (c.AuditDate.Year == AuditorYear.Value) &&
                    (Country.Value == null || c.Auditor.Profile["Location"] == Country.Value.Name))
        .GroupBy(c => c.Auditor).Select(c => new VolatileCalendarRow()
        {
          Id = c.Key.UserName,
          Name = c.Key.UserName,
          Country = c.Key.Profile["Location"],
          Jan = c.Count(x => x.AuditDate.Month == 1),
          Feb = c.Count(x => x.AuditDate.Month == 2),
          Mar = c.Count(x => x.AuditDate.Month == 3),
          Apr = c.Count(x => x.AuditDate.Month == 4),
          May = c.Count(x => x.AuditDate.Month == 5),
          Jun = c.Count(x => x.AuditDate.Month == 6),
          Jul = c.Count(x => x.AuditDate.Month == 7),
          Aug = c.Count(x => x.AuditDate.Month == 8),
          Sep = c.Count(x => x.AuditDate.Month == 9),
          Oct = c.Count(x => x.AuditDate.Month == 10),
          Nov = c.Count(x => x.AuditDate.Month == 11),
          Dec = c.Count(x => x.AuditDate.Month == 12)
        }
      ).ToList();

      AuditorCalendarTable.Value = AssignedAuditor.Value == null ? [] :
          Context.BO.Search<AuditorCalendar>(x => x.Auditor.UserName == AssignedAuditor.Value.UserName, 0, 10).ToList();
    }

    public void RefreshAssignedAuditor()
    {
      AssignedAuditor.OnGetOptions = () => Runtime.Instance.GetUsers(AssignedAuditor.SearchTerm, AssignedAuditor.Page, AssignedAuditor.PageSize)
                  .Where(x => x.IsInRole("Auditor") && (Country.Value == null || (x.Profile["Location"] == Country.Value.Name)))
                  .Select(x => new User() { UserName = x.UserName, FullName = $"{x.FullName} - {x.Profile["Location"]} ({x.Profile["Position"]})", UserImg = x.UserImg })
                  .Skip(AssignedAuditor.Page * AssignedAuditor.PageSize)
                  .Take(AssignedAuditor.PageSize);
    }



  }
}
