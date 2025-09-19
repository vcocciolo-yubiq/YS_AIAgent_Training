using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using static YubikStudioCore.Forms.MapPart;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EuropeData.WorkItems;
using EuropeData.BusinessObjects;

namespace EuropeData.Forms
{
    public class WIDetails : Form<ChartDataWI>
    {
        [Unbound]
        public MapPart EuropeMap { get; set; }
        public override void ConfigureFields()
        {
            base.ConfigureFields();

            var places = new List<Place>();

            foreach (var country in Context.BO.All<PopulationDensity>(0, 100))
            {
                var place = new Place
                {
                    Name = $"{country.Capital}",
                    Lat = country.Lat,
                    Lon = country.Lon,
                    Address = country.Capital
                };
                places.Add(place);
            }

            EuropeMap.Places = [.. places];
        }
    }

}
