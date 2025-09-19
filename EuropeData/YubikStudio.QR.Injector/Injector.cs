using Newtonsoft.Json;
using QuickRoute.Engine.CommonActions;
using QuickRoute.Engine.Configuration;
using QuickRoute.Engine.ExternalAction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using YubikStudio.Commons;

namespace YubikStudio.QR.Injector
{

 
    public class /*[WORKFLOW]*/Injector : IExecuteAction, QuickRoute.Engine.CommonActions.ICommonAction
    {
        #region IExecuteActionLog
        private static object logObject = null;
        private static Dictionary<string, System.Reflection.MethodInfo> logMethods = new Dictionary<string, System.Reflection.MethodInfo>();
        public static void Log(object txt) { Log(txt, "debug"); }
        public static void LogDebug(object txt) { Log(txt, "debug"); }
        public static void LogInfo(object txt) { Log(txt, "debug"); }
        public static void LogErro(object txt) { Log(txt, "error"); }
        public static void LogFatal(object txt) { Log(txt, "fatal"); }
        public static void LogWarn(object txt) { Log(txt, "warning"); }

        private static void Log(object txt, string type)
        {
            var mapName = "VeFa"; //???????????????????????????????????????????
            Log(txt, type, "QuickRoute.Logic." + mapName); //log specific, in log4net.config qrengine
            Log(txt, type, "QuickRoute.Logic.Y5STUDIO"); //log general, in log4net.config qrengine
        }
        private static void Log(object txt, string type, string logger)
        {
            try
            {
                type = type.ToLower();
                //logger = logger; //nolower case
                if (!logMethods.ContainsKey(type + logger))
                {
                    string name = "log4net.LogManager, log4net";
                    Type t = Type.GetType(name);
                    if (t == null) throw new Exception("dll not found in bin " + name);
                    var theMethod = t.GetMethods().Where(m => m.Name == "GetLogger").FirstOrDefault();
                    if (theMethod == null) throw new Exception("GetLogger method is null");
                    var rest = theMethod.Invoke(null, new object[] { logger });
                    if (rest == null) throw new Exception("result GetLogger is null");
                    var ltype = rest.GetType();
                    var theLogMethod = ltype.GetMethods().Where(m => m.Name.ToLower() == type.ToLower() && m.GetParameters().Count() == 1).FirstOrDefault();
                    if (theMethod == null) throw new Exception(type + " method is null");
                    logObject = rest;
                    logMethods.Add(type + logger, theLogMethod);
                }
                //log4net
                logMethods[type + logger].Invoke(logObject, new object[] { txt });
            }
            catch (Exception err)
            {
                //TEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEMP!!!!!!!!!!!!!!!!!!!! //???????????????????????????????????????????
                File.AppendAllText("c:/temp/externalactionerrorlog.log", "###### log4net error: " + Environment.NewLine + err.ToString() + "" + Environment.NewLine + "#####" + Environment.NewLine);
                File.AppendAllText("c:/temp/externalactionerrorlog.log", txt + "" + Environment.NewLine + Environment.NewLine);
            }
        }

        static void LogAll(string key, ExternalActionParameter20 par)
        {
            LogDebug($"{key} - {par.CurrentStageName} {par.ActionName} {par.TargetStageName}" + Environment.NewLine);
            try
            {
                LogDebug($"{key} - {JsonConvert.SerializeObject(par)}");
            }
            catch
            {
            }
        }
        static void LogAll(string key, string s)
        {
            LogDebug($"{key} - {s}");
        }

        #endregion

        private RetExternalAction Exec(ExternalActionParameter20 eap,string action)
        {
            var key = DateTime.Now.ToString("yyyyMMddHHss")+"_"+action+"_"+eap.ActionName;

            var odata = eap.User.Context?.FirstOrDefault(cx => cx.Name == "FormData")?.Value;

            LogAll(key, "Form Data Type:" + odata?.GetType());
            LogAll(key, "Form Data:" + odata);
            
            string data = (string)odata;

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
                c.Timeout = TimeSpan.FromMinutes(30);
                
                LogAll(key,$"Calling: {ConfigurationManager.AppSettings["DataProvider_BASEURL"]}/YS/{action}");
                var result = c.PostAsync($"{ConfigurationManager.AppSettings["DataProvider_BASEURL"]}/YS/{action}", content).Result;
                var jsonreply = result.Content.ReadAsStringAsync().Result;
                LogAll(key,"Risposta: " + jsonreply);


                var reply = JsonConvert.DeserializeObject<Output>(jsonreply);


                RetExternalAction act = new RetExternalAction() { ReturnCode = reply.Result ? ExternalActionReturnType.ExecutionOK : ExternalActionReturnType.ExecutionKO, Properties = new PropertyCollection(reply.Properties.Select(p => new QuickRoute.Engine.Configuration.Property() { Name = p.Key, Value =p.Value })) , ErrorMessage = reply.ErrorMessage };

                return act;
            }
            catch (Exception ex)
            {
                LogAll(key,ex.ToString());
                RetExternalAction act = new RetExternalAction() { ReturnCode = ExternalActionReturnType.ExecutionKO, Properties = new PropertyCollection(), ErrorMessage=ex.Message };

                return act;
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
