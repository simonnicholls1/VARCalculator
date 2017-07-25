using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    [Serializable()]
    public class DAOException : System.Exception
    {
        public static int FILE_NA = 1;
        public static int FILE_PARSE_ERROR = 2;
        public static int UNKNOWN_ERROR = 3;

        private int ErrorCode;

        public DAOException(int errorCode)
            : base()
        {
            this.ErrorCode = errorCode;
        }

        public DAOException(int errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public DAOException(int errorCode, string message, System.Exception inner)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        public DAOException(int errorCode, string message, System.Exception inner, System.Diagnostics.StackTrace stackTrace)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected DAOException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        {

        }

        public int GetErrorCode()
        {
            return ErrorCode;
        }

    }
}