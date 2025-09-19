using Newtonsoft.Json;
using QuickRoute.Engine.CommonActions;
using QuickRoute.Engine.Configuration;
using QuickRoute.Engine.ExternalAction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using YubikStudio.Commons;

namespace YubikStudio.QR.Injector
{

 
    public class /*[WORKFLOW]*/Injector : IExecuteAction, QuickRoute.Engine.CommonActions.ICommonAction
    {
        void LogAll(string key, string s)
        {
            var fname=@"C:\\Progetti\\Docflow\\YubikStudio\Out_"+key+".txt";
            File.AppendAllText(fname,"------------------------------------------------------"+Environment.NewLine);
            File.AppendAllText(fname, DateTime.Now.ToString("HH:mm:ss")+ " "+s+Environment.NewLine);
            File.AppendAllText(fname,"------------------------------------------------------"+Environment.NewLine);
        }

        void LogAll(string key, ExternalActionParameter20 par)
        {
            LogAll(key, $"{par.CurrentStageName} {par.ActionName} {par.TargetStageName}" + Environment.NewLine);

            try
            {
                LogAll(key, JsonConvert.SerializeObject(par));
            }
            catch
            {

            }
        }

        private RetExternalAction Exec(ExternalActionParameter20 eap,string action)
        {
            var key = DateTime.Now.ToString("yyyyMMddHHss")+"_"+action+"_"+eap.ActionName;
            LogAll(key,"Form Data Type:"+ eap.Properties["FormData"].ObjectValue?.GetType());
            LogAll(key,eap);
            var data = eap.Properties["FormData"].Value??eap.Properties["FormData"].ObjectValue as string;
            try
            {
                var i = JsonConvert.SerializeObject(
                    new Input()
                    {
                        Id = eap.QuickRouteId,
                        Workflow = eap.Workflow.Name,
                        CurrentStage = eap.CurrentStageName,
                        TargetStage = eap.TargetStageName,
                        Action = eap.ActionName,
                        User = eap.User.Name,
                        Roles = eap.User.Roles.Select(r => r.Name).ToArray(),
                        Properties =string.IsNullOrEmpty(data)?new Dictionary<string,string>(): JsonConvert.DeserializeObject<Dictionary<string,string>>(data)
                    });
                LogAll(key,JsonConvert.SerializeObject(i));

                var content = new StringContent(i, Encoding.UTF8, "application/json");

                HttpClient c = new HttpClient();
                c.Timeout = TimeSpan.FromMinutes(10);
                LogAll(key,$"Calling: http://localhost:5001/YS/{action}");
                var result = c.PostAsync($"http://localhost:5001/YS/{action}", content).Result;
                var jsonreply = result.Content.ReadAsStringAsync().Result;
                LogAll(key,"Risposta: " + jsonreply);


                var reply = JsonConvert.DeserializeObject<Output>(jsonreply);
                if(reply.Properties.ContainsKey("FormData"))
                reply.Properties["FormData"] = "";
                else
                reply.Properties.Add("FormData","");


                RetExternalAction act = new RetExternalAction() { ReturnCode = reply.Result ? ExternalActionReturnType.ExecutionOK : ExternalActionReturnType.ExecutionKO, Properties = new PropertyCollection(reply.Properties.Select(p => new QuickRoute.Engine.Configuration.Property() { Name = p.Key, Value =p.Value }))  };

                return act;
            }
            catch (Exception ex)
            {
                LogAll(key,ex.ToString());
                throw;
            }
        }

        //[CreationAction]
        /*[CREATE]*/
        public RetExternalAction CreateDP(ExternalActionParameter20 eap)
        {
            return Exec(eap, "Create");
        }

        //[ExternalAction("StageName","ActionName")]
        /*[ACTION]*/
        public RetExternalAction ExecDP(ExternalActionParameter20 eap)
        {
            return Exec(eap, "Execute");
        }

        //[OnEntered("StageName",false)]
        /*[ENTER]*/
        public RetExternalAction EnterDP(ExternalActionParameter20 eap)
        {
            return Exec(eap, "Enter");
        }

        //[OnLeft("StageName",false)]
        /*[EXIT]*/
        public RetExternalAction ExitDP(ExternalActionParameter20 eap)
        {
            return Exec(eap, "Exit");
        }

        public void Close()
        {

        }
    }
}
