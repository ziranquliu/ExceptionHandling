using System;
using System.Web.Mvc;
namespace ExceptionHandling
{
    public abstract class BaseController : Controller
    {
        public BaseController(string exceptionPolicy)
        {
            Func<string, HandleErrorInfo, ViewResult> getErrorView = (viewName, handleErrorInfo) => this.View(viewName, handleErrorInfo);
            this.ExceptionActionInvoker = new ExceptionActionInvoker(exceptionPolicy,getErrorView);
        }
        public BaseController(ExceptionActionInvoker actionInvoker)
        {
            this.ExceptionActionInvoker = actionInvoker;
        }

        public virtual ExceptionActionInvoker ExceptionActionInvoker { get; private set; }

        protected virtual string GetHandleErrorActionName(string actionName)
        {
            return string.Format("On{0}Error", actionName);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            using (ExceptionHandlingContextScope contextScope = new ExceptionHandlingContextScope(filterContext))
            {
                string actionName = RouteData.GetRequiredString("action");
                string handleErrorActionName = this.GetHandleErrorActionName(actionName);
                this.ExceptionActionInvoker.InvokeAction(filterContext, handleErrorActionName);
                foreach (var error in ExceptionHandlingContext.Current.Errors)
                {
                    ModelState.AddModelError(Guid.NewGuid().ToString() ,error.ErrorMessage);
                }
            }
        }
    }
}