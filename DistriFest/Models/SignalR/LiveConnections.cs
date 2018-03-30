using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistriFest.Models.SignalR
{
    public static class LiveConnections
    {
        public static Dictionary<int,string> liveConnections = new Dictionary<int, string>(); 
    }
}