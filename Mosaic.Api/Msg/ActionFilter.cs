//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Mosaic.Api.Msg
//{
//    public class ActionFilter : ActionFilterAttribute
//    {
//        private const string Key = "action";
//        private bool _IsDebugLog = true;
//        public override void OnActionExecuting(ActionExecutingContext actionContext)
//        {
//            if (_IsDebugLog)
//            {
//                Stopwatch stopWatch = new Stopwatch();

//                actionContext.Request.Properties[Key] = stopWatch;

//                string actionName = actionContext.ActionDescriptor.ActionName;

//                Debug.Print(Newtonsoft.Json.JsonConvert.SerializeObject(actionContext.ActionArguments));

//                stopWatch.Start();
//            }

//        }
//        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
//        {
//            if (_IsDebugLog)
//            {
//                Stopwatch stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;

//                if (stopWatch != null)
//                {

//                    stopWatch.Stop();

//                    string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

//                    string controllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;

//                    Debug.Print(actionExecutedContext.Response.Content.ReadAsStringAsync().Result);

//                    Debug.Print(string.Format(@"[{0}/{1} 用时 {2}ms]", controllerName, actionName, stopWatch.Elapsed.TotalMilliseconds));
//                }
//            }
//        }

//    }
//}
