using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DistriFest.Models.SignalR;
using Microsoft.AspNet.SignalR;
using Repositories;

namespace DistriFest
{
    public class NotificationHub : Hub
    {
        public void Send(int userID, string message)
        {
            if (LiveConnections.liveConnections.ContainsKey(userID))
            {
                Clients.Client(LiveConnections.liveConnections[userID]).AddMessageToPage(message);
            }
        }

        public void Chat(int userID, string message)
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            if (LiveConnections.liveConnections.ContainsKey(userID))
            {
                Clients.Client(LiveConnections.liveConnections[userID]).AddMessageToChat(new UserRepository().GetUserByID(Convert.ToInt16(identity.Claims.Last().Value)).Name,message);
                Clients.Client(LiveConnections.liveConnections[Convert.ToInt16(identity.Claims.Last().Value)]).AddMessageToChat(new UserRepository().GetUserByID(Convert.ToInt16(identity.Claims.Last().Value)).Name, message);
            }
            else
            {
                Clients.Client(LiveConnections.liveConnections[Convert.ToInt16(identity.Claims.Last().Value)]).AddMessageToChat("DistriFest","Gebruiker is momenteel niet ingelogd, bericht niet aangekomen");
            }
        }

        public override Task OnConnected()
        {
            string userName = Context.User.Identity.Name;
            var identity = (ClaimsIdentity)Context.User.Identity;
            string connectionID = Context.ConnectionId;
            if (LiveConnections.liveConnections.ContainsKey(Convert.ToInt16(identity.Claims.Last().Value)))
            {
                LiveConnections.liveConnections[Convert.ToInt16(identity.Claims.Last().Value)] = connectionID;
            }
            else
            {
                LiveConnections.liveConnections.Add(Convert.ToInt16(identity.Claims.Last().Value), connectionID);
            }
            //Send(Convert.ToInt16(identity.Claims.Last().Value), "Hello " + userName + " you are connected with id: " + connectionID + " and your userID is: " + identity.Claims.Last().Value);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string connectionID = Context.ConnectionId;
            var identity = (ClaimsIdentity)Context.User.Identity;
            LiveConnections.liveConnections.Remove(Convert.ToInt16(identity.Claims.Last().Value));
            return base.OnDisconnected(stopCalled);
        }

    }
}