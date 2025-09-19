using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.CodeLibraries;
using Intercos.WorkItems;

namespace Intercos.CodeLibs
{
  public class QuotationLib : CodeLibrary<QuotationWI>
  {
    public QuotationLib(ActionContext ctx) : base(ctx)
    {
    }

    public static string GetStatsHtml(decimal percProdItems, decimal percValue)
    {
      var prodColor = "gray-700";
      var costColor = "gray-700";
      if (percValue > 100)
      {
        costColor = "danger";
      }
      if (percValue < 100 && percValue >= 90)
      {
        costColor = "warning";
      }


      if (percProdItems > 100)
      {
        prodColor = "danger";
      }
      if (percProdItems < 100 && percProdItems >= 90)
      {
        prodColor = "warning";
      }
      // else
      // {
      //   prodColor = "success";
      // }

      return $@"    
            <div class=""bg-secondary-subtle rounded p-3 mx-5 mb-5"">
              <div class=""d-flex align-items-center justify-content-center "">
                <span class=""text-center h5"">Soglie</span>
              </div>
              <div class=""d-flex align-items-center justify-content-start px-6 py-5"">
                  <div class=""symbol symbol-20px me-5"">
                      <span class=""symbol-label"">  
                          <i class=""fa-solid fa-euro-sign fa-2x text-gray-400""></i>                     
                      </span>                
                  </div>
                  <div class=""m-0"">
                      <span class=""text-{costColor} fw-bolder d-block fs-2x lh-1 ls-n1 mb-1"">{Math.Round(percValue, 0)}%</span>
                      <span class=""text-gray-500 fw-semibold fs-6"">Soglia costo stimato</span>
                  </div>
              </div>
                <div class=""d-flex align-items-center justify-content-start px-6 py-5"">
                  <div class=""symbol symbol-20px me-5"">
                      <span class=""symbol-label"">  
                          <i class=""fa-solid fa-boxes-packing text-gray-400 fa-2x""></i>                       
                      </span>                
                  </div>
                  <div class=""m-0"">
                      <span class=""text-{prodColor} fw-bolder d-block fs-2x lh-1 ls-n1 mb-1"">{Math.Round(percProdItems, 0)}%</span>
                      <span class=""text-gray-500 fw-semibold fs-6"">Soglia numero prodotti</span>
                  </div>
              </div>
            </div>";
    }

    public static string GetStatsHtml(decimal percProdItems, decimal percValue, decimal percIngredients)
    {
      return GetStatsHtml(percProdItems, percValue) + GetStatsHtml(percIngredients);
    }

    public static string GetStatsHtml(decimal percIngredients)
    {
      var percColor = "gray-700";
      if (percIngredients != 100)
      {
        percColor = "danger";
      }
      else
      {
        percColor = "success";
      }
      return $@"
                        <div class=""d-flex align-items-center justify-content-start px-6 py-5"">
                            <div class=""symbol symbol-20px me-5"">
                                <span class=""symbol-label"">  
                                    <i class=""fa-solid fa-flask fa-2x text-gray-400""></i>                        
                                </span>                
                            </div>
                            <div class=""m-0"">
                                <span class=""text-{percColor} fw-bolder d-block fs-2x lh-1 ls-n1 mb-1"">{percIngredients}%</span>
                                <span class=""text-gray-500 fw-semibold fs-6"">Completezza formula</span>
                            </div>
                        </div>";
    }

    public static string GetTotalCost(decimal cost)
    {
      return $@"
                        <div class=""d-flex align-items-center justify-content-start px-6 py-5"">
                            <div class=""symbol symbol-20px me-5"">
                                <span class=""symbol-label"">  
                                    <i class=""fa-solid fa-coins fa-2x text-gray-400""></i>                        
                                </span>                
                            </div>
                            <div class=""m-0"">
                                <span class="" fw-bolder d-block fs-2x lh-1 ls-n1 mb-1"">{cost} €</span>
                                <span class=""text-gray-500 fw-semibold fs-6"">Costo Totale Materiali</span>
                            </div>
                        </div>";
    }

    public static string GetTechnologyStatsHtml(int technologyId)
    {
      // var technology = Context.BO.Get<Technology>(technologyId);
      var text = $@"<span class=""text-success fw-bolder d-block fs-2x lh-1 ls-n1 mb-1"">OK</span>";


      if (technologyId == 1)
      {
        text = $@"<span class=""text-danger fw-bolder d-block fs-2x lh-1 ls-n1 mb-1"">Nuova</span>";
      }
      else if (technologyId == 0)
      {
        text = $@"<span class=""text-warning fw-bolder d-block fs-2x lh-1 ls-n1 mb-1"">Non Selezionata</span>";
      }

      return $@"
              <div class=""d-flex align-items-center justify-content-start px-6 py-5"">
                  <div class=""symbol symbol-20px me-5"">
                      <span class=""symbol-label"">  
                          <i class=""fa-solid fa-cogs fa-2x text-gray-400""></i>                        
                      </span>                
                  </div>
                  <div class=""m-0"">
                      {text}
                      <span class=""text-gray-500 fw-semibold fs-6"">Tecnologia</span>
                  </div>
              </div>";
    }

    public static string GetTimeline(DateOnly quotationDl, DateOnly deliveryDl)
    {
      var daysToQuotation = quotationDl.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;
      var daysToDelivery = deliveryDl.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;
      var textColorQuot = "";
      var textColorDel = "";

      if (daysToQuotation < 30)
      {
        textColorQuot = "text-warning";
      }
      else if (daysToQuotation < 15)
      {
        textColorQuot = "text-danger";
      }

      if (daysToDelivery < 60)
      {
        textColorDel = "text-warning";
      }
      else if (daysToDelivery < 30)
      {
        textColorDel = "text-danger";
      }

      return $@"
            <div class=""bg-secondary-subtle rounded p-3 mx-5 mb-5"">
              <div class=""d-flex align-items-center justify-content-center "">
                <span class=""text-start h5"">Tempi</span>
              </div>
              <div class=""d-flex align-items-center justify-content-start px-6 py-5"">
                  <div class=""symbol symbol-20px me-5"">
                      <span class=""symbol-label"">  
                          <i class=""fa-solid fa-coins fa-2x text-gray-400""></i>                        
                      </span>                
                  </div>
                  <div class=""m-0"">
                      <span class="" fw-bolder d-block fs-2x lh-1 ls-n1 mb-1 {textColorQuot}"">{daysToQuotation}</span>
                      <span class=""text-gray-500 fw-semibold fs-6"">Giorni alla Quotazione</span>
                  </div>
              </div>
              <div class=""d-flex align-items-center justify-content-start px-6 py-5"">
                  <div class=""symbol symbol-20px me-5"">
                      <span class=""symbol-label"">  
                          <i class=""fa-solid fa-calendar fa-2x text-gray-400""></i>                        
                      </span>                
                  </div>
                  <div class=""m-0"">
                      <span class="" fw-bolder d-block fs-2x lh-1 ls-n1 mb-1 {textColorDel}"">{daysToDelivery}</span>
                      <span class=""text-gray-500 fw-semibold fs-6"">Giorni alla Delivery</span>
                  </div>
              </div>
            </div>";
    }

    public static string GetPackagingWFStatusHtml(bool isCompleted, string status, string user, string lastModifiedDate)
    {
      var statusColor = "gray-700";
      var statusMessage = "Processo di packaging ";
      if (isCompleted)
      {
        statusColor = "success";
        statusMessage += "completato!";
      }
      else
      {
        statusColor = "warning";
        statusMessage += "in corso...";
      }

      return $@"
            <div class=""bg-secondary-subtle rounded p-3 mx-5 mb-5 mt-n3"">
              <div class=""d-flex align-items-center justify-content-center mb-5"">
                <span class=""text-start text-{statusColor} fs-4 fw-bold"">{statusMessage}</span>
              </div>
              <div class=""d-flex flex-column align-items-start gap-1"">
                <div class=""d-flex align-items-center justify-content-start px-6 py-3"">
                    <div class=""symbol symbol-20px me-5"">
                        <span class=""symbol-label"">  
                            <i class=""fa-solid fa-boxes-packing fa-2x text-gray-400""></i>                        
                        </span>                
                    </div>
                    <div class=""d-flex flex-column"">
                        <span class=""text-{statusColor} fw-bolder d-block fs-5 lh-1 ls-n1 mb-1"">{L.T(status)}</span>
                        <span class=""text-gray-500 fw-semibold fs-6"">Stato</span>
                    </div>
                </div>
                <div class=""d-flex align-items-center justify-content-start px-6 py-3"">
                    <div class=""symbol symbol-20px me-5"">
                        <span class=""symbol-label"">  
                            <i class=""fa-solid fa-user fa-2x text-gray-400""></i>                        
                        </span>                
                    </div>
                    <div>
                        <span class=""text-gray-700 fw-bolder d-block fs-5 lh-1 ls-n1 mb-1"">{user}</span>
                        <span class=""text-gray-500 fw-semibold fs-6"">In carico a</span>
                    </div>
                </div>
                <div class=""d-flex align-items-center justify-content-start px-6 py-3"">
                    <div class=""symbol symbol-20px me-5"">
                        <span class=""symbol-label"">  
                            <i class=""fa-solid fa-cog fa-2x text-gray-400""></i>                        
                        </span>                
                    </div>
                    <div>
                        <span class=""text-gray-700 fw-bolder d-block fs-5 lh-1 ls-n1 mb-1"">{lastModifiedDate}</span>
                        <span class=""text-gray-500 fw-semibold fs-6"">Ultima azione</span>
                    </div>
                </div>
              </div>
            </div>";
    }

    public static string GetDiffHtml(decimal startPrice, decimal endPrice)
    {
      if (startPrice == 0 && endPrice == 0)
        return string.Empty;

      var diff = endPrice - startPrice;
      var percent = startPrice != 0 ? (diff / startPrice) * 100 : 0;
      var arrow = diff >= 0 ? "fa-arrow-up" : "fa-arrow-down";
      var badgeClass = diff <= 0 ? "badge-light-success" : "badge-light-danger";

      return @$"  <div class=""mb-7"">
                    <div class=""d-flex align-items-center mb-2"">   
                        <span class=""fs-4 fw-semibold text-gray-500 me-1 mt-n1 alignt-self-start"">€</span>
                        <span class=""fs-2x fw-bold text-gray-800 me-2 lh-1 ls-n2"">{diff.ToString("N2")}</span>
                        <span class=""badge {badgeClass} fs-base"">                                
                           <i class=""fa-solid {arrow}""></i>{percent.ToString("N2")}%</span>  

                    </div>
                    <span class=""fs-6 fw-semibold text-gray-500"">Differenza rispetto a costo stimato</span>
                </div>
                ";
    }

    public static string GetUploadFromExcel()
    {
      // return html with an icon and text Carica from excel

      return @"
        <a class=""btn d-flex align-items-center me-5 py-3"">
          <i class=""fa-solid fa-upload fa-lg text-success me-3"" title=""Carica da file""></i>
          <span class=""fw-semibold"">Carica da file</span>
        </a>
        ";
    }
    public static string GetDownloadToExcel()
    {
      // return html with an icon and text Scarica da excel

      return @"
        <a class="" d-flex align-items-center me-5 py-3"">
          <i class=""fa-solid fa-file-excel text-success me-3 fa-2x"" title=""Download in Excel""></i>
          <!--<span class=""fw-semibold"">Download in Excel</span>-->
        </a>
        ";
    }
  }
}