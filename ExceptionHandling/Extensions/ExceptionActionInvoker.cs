using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
namespace ExceptionHandling
{
    public class ExceptionActionInvoker: ControllerActionInvoker
    {
        protected ExceptionHandlingSettings ExceptionHandlingSettings{get; private set;}
        protected virtual Func<string, HandleErrorInfo, ViewResult> GetErrorView { get; private set; }
        public ExceptionPolicyImpl ExceptionPolicy { get; private set; }
        public ExceptionActionInvoker(string exceptionPolicy,Func<string, HandleErrorInfo, ViewResult> getErrorView)
        {
            this.ExceptionPolicy = EnterpriseLibraryContainer.Current.GetInstance<ExceptionPolicyImpl>(exceptionPolicy);
            this.GetErrorView = getErrorView;
            this.ExceptionHandlingSettings = ExceptionHandlingSettings.GetSection();
        }

        public override bool InvokeAction(ControllerContext controllerContext, string handleErrorActionName)
        {
            ExceptionContext exceptionContext = controllerContext as ExceptionContext;
            if (null == exceptionContext)
            {
                throw new ArgumentException("The controllerContext must be ExceptionContext!", "controllerContext");
            }
            try
            {
                exceptionContext.ExceptionHandled = true;
                if (this.ExceptionPolicy.HandleException(exceptionContext.Exception))
                {
                    HandleRethrownException(exceptionContext);
                }
                else
                {
                    if (ExceptionHandlingContext.Current.Errors.Count == 0)
                    {
                        ExceptionHandlingContext.Current.Errors.Add(exceptionContext.Exception.Message);
                    }
                    ControllerDescriptor controllerDescriptor = this.GetControllerDescriptor(exceptionContext);
                    ActionDescriptor handleErrorAction = FindAction(exceptionContext, controllerDescriptor, handleErrorActionName);
                    if (null != handleErrorAction)
                    {
                        IDictionary<string, object> parameters = GetParameterValues(controllerContext, handleErrorAction);
                        exceptionContext.Result = this.InvokeActionMethod(exceptionContext, handleErrorAction, parameters);
                    }
                    else
                    {
                        HandleRethrownException(exceptionContext);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                exceptionContext.Exception = ex;
                HandleRethrownException(exceptionContext);
                return true;
            }
        }
        protected virtual void HandleRethrownException(ExceptionContext exceptionContext)
        {
            string errorViewName = this.GetErrorViewName(exceptionContext.Exception.GetType());
            string controllerName = (string)exceptionContext.RouteData.GetRequiredString("controller");
            string action = (string)exceptionContext.RouteData.GetRequiredString("action");
            HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exceptionContext.Exception, controllerName, action);
            exceptionContext.Result = this.GetErrorView(errorViewName, handleErrorInfo);
        }
        protected string GetErrorViewName(Type exceptionType)
        {
            ExceptionErrorViewElement element = ExceptionHandlingSettings.ExceptionErrorViews
                .Cast<ExceptionErrorViewElement>().FirstOrDefault(el=>el.ExceptionType == exceptionType);
            if(null != element)
            {
                return element.ErrorView;
            }
            if(null== element && null != exceptionType.BaseType!= null)
            {
                return GetErrorViewName(exceptionType.BaseType);
            }
            else
            {
                return "Error";
            }
        }
    }
}