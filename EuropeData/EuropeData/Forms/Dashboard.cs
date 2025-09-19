using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EuropeData.WorkItems;
using System.Security.Cryptography.X509Certificates;

namespace EuropeData.Forms
{
  public class FrmPopulationArea : Form<ChartDataWI>
  {
    [Unbound]
    public ChartPart PopulationBar { get; set; }
    [Unbound]
    public ChartPart AreaBar { get; set; }
    [Unbound]
    public ChartPart PopulationPie { get; set; }
    [Unbound]
    public ChartPart AreaPie { get; set; }
    [Unbound]
    public ChartPart PopAreaBubble { get; set; }


    // --------------------Chart.js-------------------------
    // https://www.chartjs.org/docs/latest/charts/bar.html |
    // -----------------------------------------------------

    public override void ConfigureFields()
    {
      base.ConfigureFields();
      PopulationBar.Config = new ChartJsConfig
      {
        Type = "bar",
        Data = new ChartJsData
        {
          Labels = [.. Context.BO.All<PopulationDensity>(0, 100).OrderByDescending(x => x.Population).Select(x => x.Country)],
          Datasets = [new ChartJsDataset<decimal>() {
            Data =[.. Context.BO.All<PopulationDensity>(0,100).OrderByDescending(x => x.Population).Select(x => x.Population)]
            }]
        },
        Options = new ChartJsOptions
        {
          Responsive = true,
          MaintainAspectRatio = false,

          Scales = new Dictionary<string, ChartJsScale>
          {
            ["x"] = new ChartJsScale
            {
              BeginAtZero = true,
              Title = new ChartJsScaleTitle
              {
                Display = true,
                Text = "Country"
              }
            },
            ["y"] = new ChartJsScale
            {
              BeginAtZero = true,
              Title = new ChartJsScaleTitle
              {
                Display = true,
                Text = "Population"
              }
            }

          },
          Plugins = new ChartJsOptionsPlugins
          {
            Title = new ChartJsPluginTitle
            {
              Display = true,
              Text = "European Population"
            },
            Legend = new ChartJsPluginLegend
            {
              Display = false,
              Position = "top"
            }
          }
        }
      };
      AreaBar.Config = new ChartJsConfig
      {
        Type = "bar",
        Data = new ChartJsData
        {
          Labels = [.. Context.BO.All<PopulationDensity>(0, 100).OrderByDescending(x => x.AreaKm2).Select(x => x.Country)],
          Datasets = [new ChartJsDataset<decimal>() {
            Data =[.. Context.BO.All<PopulationDensity>(0,100).OrderByDescending(x => x.AreaKm2).Select(x => x.AreaKm2)]
          }]
        },
        Options = new ChartJsOptions
        {
          Responsive = true,
          MaintainAspectRatio = false,
          Scales = new Dictionary<string, ChartJsScale>
          {
            ["x"] = new ChartJsScale
            {
              BeginAtZero = true,
              Title = new ChartJsScaleTitle
              {
                Display = true,
                Text = "Country"
              }
            },
            ["y"] = new ChartJsScale
            {
              BeginAtZero = true,
              Title = new ChartJsScaleTitle
              {
                Display = true,
                Text = "Area"
              }
            }

          },
          Plugins = new ChartJsOptionsPlugins
          {
            Title = new ChartJsPluginTitle
            {
              Display = true,
              Text = "European Area"
            },
            Legend = new ChartJsPluginLegend
            {
              Display = false,
              Position = "top"
            }
          }
        }
      };
      PopulationPie.Config = new ChartJsConfig
      {
        Type = "pie",
        Data = new ChartJsData
        {
          Labels = [.. Context.BO.All<PopulationDensity>(0, 100).OrderByDescending(x => x.Population).Select(x => x.Country)],
          Datasets = [new ChartJsDataset<decimal>() {
            Data =[.. Context.BO.All<PopulationDensity>(0,100).OrderByDescending(x => x.Population).Select(x => x.Population)]
            }]
        },
        Options = new ChartJsOptions
        {
          Responsive = true,
          MaintainAspectRatio = false,
          Plugins = new ChartJsOptionsPlugins
          {
            Title = new ChartJsPluginTitle
            {
              Display = true,
              Text = "European Population"
            },
            Legend = new ChartJsPluginLegend
            {
              Display = false,
              Position = "top"
            }
          }
        }
      };
      AreaPie.Config = new ChartJsConfig
      {
        Type = "pie",
        Data = new ChartJsData
        {
          Labels = [.. Context.BO.All<PopulationDensity>(0, 100).OrderByDescending(x => x.AreaKm2).Select(x => x.Country)],
          Datasets = [new ChartJsDataset<decimal>() {
            Data =[.. Context.BO.All<PopulationDensity>(0,100).OrderByDescending(x => x.AreaKm2).Select(x => x.AreaKm2)]
            }]
        },
        Options = new ChartJsOptions
        {
          Responsive = true,
          MaintainAspectRatio = false,
          Plugins = new ChartJsOptionsPlugins
          {
            Title = new ChartJsPluginTitle
            {
              Display = true,
              Text = "European Area"
            },
            Legend = new ChartJsPluginLegend
            {
              Display = false,
              Position = "top"
            }
          }
        }
      };

      PopAreaBubble.Config = new ChartJsConfig
      {
        Type = "bubble",
        Data = new ChartJsData
        {
          Labels = [.. Context.BO.All<PopulationDensity>(0, 100).OrderByDescending(x => x.Population).Select(x => x.Country)],
          Datasets = [new ChartJsDataset<ChartJsDataPoint>() {
            Data = [.. Context.BO.All<PopulationDensity>(0,100).OrderByDescending(x => x.AreaKm2).Select(x => new ChartJsDataPoint
            {
              X = x.Population,
              Y = x.AreaKm2,
              R = (int)Math.Sqrt(x.Population / 10000) // Radius based on population
            })]
          }]
        },
        Options = new ChartJsOptions
        {
          Responsive = true,
          MaintainAspectRatio = false,
          Scales = new Dictionary<string, ChartJsScale>
          {
            ["x"] = new ChartJsScale
            {
              BeginAtZero = true,
              Title = new ChartJsScaleTitle
              {
                Display = true,
                Text = "Population"
              }
            },
            ["y"] = new ChartJsScale
            {
              BeginAtZero = true,
              Title = new ChartJsScaleTitle
              {
                Display = true,
                Text = "Area"
              }
            }

          },
          Plugins = new ChartJsOptionsPlugins
          {
            Title = new ChartJsPluginTitle
            {
              Display = true,
              Text = "Area Vs Population"
            },
            Legend = new ChartJsPluginLegend
            {
              Display = false,
              Position = "top"
            }
          }
        }
      };

    }
    public override FormPart GetLayout()
    {
      var t1r1c1 = Col(PopulationBar); t1r1c1.CssClass = "chart-container";
      var t1r1c2 = Col(PopulationPie); t1r1c2.CssClass = "chart-container";
      var t1r1 = Row(t1r1c1, t1r1c2);
      var t1r2c1 = Col(AreaBar); t1r2c1.CssClass = "chart-container";
      var t1r2c2 = Col(AreaPie); t1r2c2.CssClass = "chart-container";
      var t1r2 = Row(t1r2c1, t1r2c2);
      var t1 = Flat(t1r1, t1r2);

      var t2r1c1 = Col(PopAreaBubble); t1r1c1.CssClass = "chart-container";
      var t2r1 = Row(t2r1c1);
      var t2 = Flat(t2r1);

      var t3 = Flat(title: "...");

      var tabs = Tabs(t1, t2, t3);
      tabs.TabHeaders = ["Population Density", "Population & Area", "..."];

      return Flat(tabs);
    }
  }

}
