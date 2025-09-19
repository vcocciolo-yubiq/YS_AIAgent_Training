using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.CodeLibraries;
using ENELDAI.WorkItems;
using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Core;

namespace ENELDAI.CodeLibs
{
  public class APInvoice : CodeLibrary<TestDAIWI>
  {
    private const string endpoint = "https://temptestrossi.cognitiveservices.azure.com/";
    private const string apiKey = "bbe060f7ff18402d9fa2aa05664e47a2";
    public AnalyzeResult DAIInvoke(byte[] data, string Locale)
    {

      var client = new DocumentIntelligenceClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

      BinaryData bdata = BinaryData.FromBytes(data);

      var options = new AnalyzeDocumentOptions("prebuilt-invoice", bdata)
      {
        Locale = Locale,
        Pages = null // null for all pages otherwhise: "1-5"
      };

      var operation = client.AnalyzeDocument(WaitUntil.Completed, options);
      var result = operation.Value;

      return result;

    }

    public AnalyzeResult DAIInvoke2(byte[] data, string Locale)
    {

      var client = new DocumentIntelligenceClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

      BinaryData bdata = BinaryData.FromBytes(data);

      var options = new AnalyzeDocumentOptions("prebuilt-read", bdata)
      {
        Locale = Locale,
        Pages = null // null for all pages otherwhise: "1-5"
      };

      var operation = client.AnalyzeDocument(WaitUntil.Completed, options);
      var result = operation.Value;

      return result;

    }

    public string SetDAIStringField(string fieldName, AnalyzeResult result)
    {
      if (result.Documents.Count > 0 && result.Documents[0].Fields.TryGetValue(fieldName, out var field))
        return field.Content?.Replace("\n", "");
      else
        return null;
    }

    public DateOnly? SetDAIDateField(string fieldName, AnalyzeResult result)
    {
      if (result.Documents.Count > 0 && result.Documents[0].Fields.TryGetValue(fieldName, out var field))
        return DateOnly.FromDateTime(field.ValueDate.Value.Date);
      else
        return null;
    }

    public decimal? SetDAICurrencyField(string fieldName, AnalyzeResult result)
    {
      if (result.Documents.Count > 0 && result.Documents[0].Fields.TryGetValue(fieldName, out var field))
        return (decimal)field.ValueCurrency.Amount;
      else
        return null;
    }

    public string SetDAICurrencyCodeField(string fieldName, AnalyzeResult result)
    {
      if (result.Documents.Count > 0 && result.Documents[0].Fields.TryGetValue(fieldName, out var field))
        return field.ValueCurrency.CurrencyCode;
      else
        return null;
    }

    public string SetDAICurrencySymbolField(string fieldName, AnalyzeResult result)
    {
      if (result.Documents.Count > 0 && result.Documents[0].Fields.TryGetValue(fieldName, out var field))
        return field.ValueCurrency.CurrencySymbol;
      else
        return null;
    }

    public APInvoice(ActionContext ctx) : base(ctx)
    {
    }

  }



}
