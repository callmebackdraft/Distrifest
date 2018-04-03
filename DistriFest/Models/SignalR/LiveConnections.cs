using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using extmodels = Models;

namespace DistriFest.Models.SignalR
{
    public static class LiveConnections
    {
        public static List<SignalRConnection> liveConnections = new List<SignalRConnection>();
    }
}