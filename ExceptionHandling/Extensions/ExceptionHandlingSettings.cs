using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace ExceptionHandling.Configuration
{
    public class ExceptionHandlingSettings: ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ExceptionErrorViewElementCollection ExceptionErrorViews
        {
            get { return (ExceptionErrorViewElementCollection)this[""]; }
        }

        public static  ExceptionHandlingSettings GetSection()
        {
            return ConfigurationManager.GetSection("artech.exceptionHandling") as ExceptionHandlingSettings;
        }
    }

    public class ExceptionErrorViewElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExceptionErrorViewElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ExceptionErrorViewElement).ExceptionType;
        }
    }

    public class ExceptionErrorViewElement : ConfigurationElement
    {
        [TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
        [ConfigurationProperty("exceptionType", IsRequired = true)]
        public Type ExceptionType
        {
            get { return (Type)this["exceptionType"]; }
            set { this["exceptionType"] = value; }
        }

        [ConfigurationProperty("errorView", IsRequired = true)]
        public string ErrorView
        {
            get { return (string)this["errorView"]; }
            set { this["errorView"] = value; }
        }
    }
}