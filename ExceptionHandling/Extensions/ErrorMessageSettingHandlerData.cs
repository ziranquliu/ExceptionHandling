using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace ExceptionHandling.Configuration
{
    public class ErrorMessageSettingHandlerData : ExceptionHandlerData
    {
        [ConfigurationProperty("errorMessage")]
        public string ErrorMessage
        {
            get { return (string)this["errorMessage"]; }
            set { this["errorMessage"] = value; }
        }

        public override IEnumerable<TypeRegistration> GetRegistrations(string namePrefix)
        {
            yield return
                new TypeRegistration<IExceptionHandler>(
                    () => new ErrorMessageSettingHandler(this.ErrorMessage))
                {
                    Name = this.BuildName(namePrefix),
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}