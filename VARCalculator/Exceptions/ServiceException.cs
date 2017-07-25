using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    [Serializable()]
    public class ServiceException : System.Exception
    {
        public static int DATA_ACCESS_ERROR = 1;
        public static int UNKNOWN_ERROR = 2;

        private int ErrorCode;

        public ServiceException(int errorCode)
            : base()
        {
            this.ErrorCode = errorCode;
        }

        public ServiceException(int errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public ServiceException(int errorCode, string message, System.Exception inner)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        public ServiceException(int errorCode, string message, System.Exception inner, System.Diagnostics.StackTrace stackTrace)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected ServiceException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        {

        }

        public int GetErrorCode()
        {
            return ErrorCode;
        }

    }
}