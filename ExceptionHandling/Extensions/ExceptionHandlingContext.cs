using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExceptionHandling
{
    public class ExceptionHandlingContext
    {
        [ThreadStatic]
        private static ExceptionHandlingContext current;

        public ExceptionContext ExceptionContext { get; private set; }
        public ModelErrorCollection Errors { get; private set; }

        public ExceptionHandlingContext(ExceptionContext exceptionContext)
        {
            this.ExceptionContext = exceptionContext;
            this.Errors = new ModelErrorCollection();
        }
        public static ExceptionHandlingContext Current
        {
            get { return current; }
            set { current = value; }
        }
    }

    public class ExceptionHandlingContextScope : IDisposable
    {
        private ExceptionHandlingContext original = ExceptionHandlingContext.Current;
        public ExceptionHandlingContextScope(ExceptionContext exceptionContext)
        {
            ExceptionHandlingContext.Current = new ExceptionHandlingContext(exceptionContext);
        }

        public void Dispose()
        {
            ExceptionHandlingContext.Current = original;
        }
    }
}