using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistriFest.Exceptions
{
    public class QueryException : Exception
    {
        public QueryException(string message)
            :base(message)
        {
        }
    }
}