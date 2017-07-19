using System;
using ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace ExceptionHandling
{
    [ConfigurationElementType(typeof(ErrorMessageSettingHandlerData))]
    public class ErrorMessageSettingHandler : IExceptionHandler
    {
        public string ErrorMessage { get; private set; }
        public ErrorMessageSettingHandler(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            if (null == ExceptionHandlingContext.Current)
            {
                throw new InvalidOperationException("...");
            }

            if (string.IsNullOrEmpty(this.ErrorMessage))
            {
                ExceptionHandlingContext.Current.Errors.Add(exception.Message);
            }
            else
            {
                ExceptionHandlingContext.Current.Errors.Add(this.ErrorMessage);
            }
            return exception;
        }
    }    
}