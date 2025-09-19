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
using static YubikStudioCore.Forms.MapPart;
using System.Security.Cryptography.X509Certificates;

namespace Intercos.Forms
{
  public class TechAndSiteSelection : FormulaDefView
  {

    //public virtual BoLookupField<Plant> Site { get; set; }

    //--------------------------- Unbound Fields ---------------------------
    // [Unbound]
    // public override MapPart Map { get; set; }
    // [Unbound]
    // public override TextField TechCategory { get; set; }
    [Unbound]
    public virtual BoLookupField<Plant> Plant { get; set; }
    [Unbound]
    public virtual BoLookupField<Country> Country { get; set; }
    [Unbound]
    public virtual TableField<Plant, PlantRow> PlantTable { get; set; }


    public override FormPart GetLayout()
    {
      var layout = base.GetLayout();

      var colCountry = Col(Country);
      colCountry.CssClass = "col-8";

      var colPlant = Col(Plant);
      colPlant.CssClass = "col-8";

      var colMap = Col(Map);
      colMap.CssClass = "col-5";

      var col1 = Col(//Row(Col(Code)),
                      Row(RawHtml("<div class='form-label mb-3'>Seleziona la tecnologia</div>")),
                      Row(Col(Technology)),
                      Row(Col(TechCategory)),
                      Row(RawHtml("<div class='form-label mt-5 mb-3'>Seleziona il sito usando il Paese</div>")),
                      Row(colCountry),
                      Row(colPlant));
      col1.CssClass = "col-7";

      return Flat(
                  Row(col1, colMap),
                  Row(PlantTable)
                  );
    }
    public override void OnLoad()
    {
      base.OnLoad();
      RefreshMap();
    }
    public override void ConfigureFields()
    {
      base.ConfigureFields();
      Code.ReadOnly = true;
      Customer.ReadOnly = true;
      Description.ReadOnly = true;
      TechCategory.DependsOn = [nameof(Technology)];

      Map.DependsOn = [nameof(Country), nameof(Plant)];
      Plant.DependsOn = [nameof(Country)];
      PlantTable.DependsOn = [nameof(Plant)];

      PlantTable.CanFilter = false;
      PlantTable.CanSort = false;
      PlantTable.ReadOnly = true;

    }
    public override void OnRefresh(string[] changedProperties)
    {
      base.OnRefresh(changedProperties);
      if (changedProperties.Contains(nameof(Technology)))
      {
        TechCategory.Value = Technology.Value.Category;
      }
      if (changedProperties.Contains(nameof(Country)))
      {
        RefreshMap(Country.Value?.Name, null);

        var ls = Context.BO.All<Plant>(0, int.MaxValue).
        Where(x => x.Country == Country.Value.Name)
        .ToList();

        Plant.OnGetOptions = null; // Clear previous options;
        Plant.OnGetOptions = () => ls;

      }
      if (changedProperties.Contains(nameof(Plant)))
      {
        RefreshMap(Country.Value?.Name, Plant.Value?.Name);


        var plants = Context.BO.All<Plant>(0, int.MaxValue)
          .Where(x => (Country == null || x.Country == Country.Value.Name)
                     && (Plant.Value == null || x.Name == Plant.Value.Name))
          .ToList();
        //reset table values
        PlantTable.Value = plants;
        if (plants.Count > 0)
        {
          //save the site in the context item
          Context.Item.Site = Plant.Value;
        }
      }


    }

    public void RefreshMap(string country = null, string plantName = null)
    {
      var ls = Context.BO.All<Plant>(0, int.MaxValue).
        Where(x =>
        (country == null || x.Country == country)
        && (plantName == null || x.Name == plantName))
        .ToList();

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

      Map.Places = places.ToArray();
    }
  }

}
