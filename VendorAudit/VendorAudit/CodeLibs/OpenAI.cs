using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Runtime;
using YubikStudioCore.CodeLibraries;
using VendorAudit.WorkItems;
using OpenAI.Chat;

namespace VendorAudit.CodeLibs
{
  public class OpenAI : CodeLibrary<ExecutionWI>
  {
    
public ChatCompletion InvokeAI(string prompt)
    {
      ChatClient client = new(
              model: Runtime.Instance.Params["OpenAIModel"],
              apiKey: Runtime.Instance.Params["OpenAIKey"]
            );

      ChatCompletion completion = client.CompleteChat(prompt);
      return completion;
    }


    public OpenAI(ActionContext ctx) : base(ctx)
        {
        }
  }

}
