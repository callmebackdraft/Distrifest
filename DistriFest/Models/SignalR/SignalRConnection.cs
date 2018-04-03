using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using extmodels = Models;

namespace DistriFest.Models.SignalR
{
    public class SignalRConnection
    {
        public extmodels.User User { get; private set; }
        public string ConnectionID { get; private set; }

        public SignalRConnection(extmodels.User _user, string _connectionID)
        {
            User = _user;
            ConnectionID = _connectionID;
        }

        public SignalRConnection()
        {

        }

        public void SetConnectionID(string _connectionID)
        {
            ConnectionID = _connectionID;
        }
    }
}