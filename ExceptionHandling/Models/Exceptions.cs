using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExceptionHandling.Models
{
    [Serializable]
    public class InvalidUserNameException : Exception
    {
        public InvalidUserNameException() { }
        public InvalidUserNameException(string message) : base(message) { }
        public InvalidUserNameException(string message, Exception inner) : base(message, inner) { }
        protected InvalidUserNameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class UserNamePasswordNotMatchException : Exception
    {
        public UserNamePasswordNotMatchException() { }
        public UserNamePasswordNotMatchException(string message) : base(message) { }
        public UserNamePasswordNotMatchException(string message, Exception inner) : base(message, inner) { }
        protected UserNamePasswordNotMatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }  
}