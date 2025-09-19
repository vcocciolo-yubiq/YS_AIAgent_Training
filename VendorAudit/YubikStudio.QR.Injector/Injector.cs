using Newtonsoft.Json;
using QuickRoute.Engine.CommonActions;
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

 
    public class Injector : IExecuteAction, QuickRoute.Engine.CommonActions.ICommonAction
    {
        void LogAll(string s)
        {
            File.AppendAllText(@"C:\\Progetti\\Docflow\\YubikStudio\Out.txt", s);
        }

        void LogAll(ExternalActionParameter20 par)
        {
            LogAll($"{DateTime.Now.ToString("HH:mm:ss")} {par.CurrentStageName} {par.ActionName} {par.TargetStageName}" + Environment.NewLine);

            try
            {
                LogAll("E aedsso veramente tutto");
                LogAll(JsonConvert.SerializeObject(par));
            }
            catch
            {

            }
        }

        private RetExternalAction Exec(ExternalActionParameter20 eap,string action)
        {
            LogAll(eap);
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
                        Properties = eap.Properties.ToDictionary(p => p.Name, p => p.Value)
                    });
                LogAll(JsonConvert.SerializeObject(i));

                var content = new StringContent(i, Encoding.UTF8, "application/json");

                HttpClient c = new HttpClient();
                LogAll($"Calling: http://localhost:5001/YS/{action}");
                var result = c.PostAsync($"http://localhost:5001/YS/{action}", content).Result;
                var jsonreply = result.Content.ReadAsStringAsync().Result;
                LogAll("Risposta: " + jsonreply);


                var reply = JsonConvert.DeserializeObject<Output>(jsonreply);

                RetExternalAction act = new RetExternalAction() { ReturnCode = reply.Result ? ExternalActionReturnType.ExecutionOK : ExternalActionReturnType.ExecutionKO };

                return act;
            }
            catch (Exception ex)
            {
                LogAll(ex.ToString());
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
