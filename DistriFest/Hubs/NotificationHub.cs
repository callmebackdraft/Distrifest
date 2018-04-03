using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DistriFest.Models.SignalR;
using Microsoft.AspNet.SignalR;
using Repositories;
using extmodels = Models;

namespace DistriFest
{
    public class NotificationHub : Hub
    {
        public void Send(int userID, string message)
        {
            extmodels.User recipient = new UserRepository().GetUserByID(userID);
            if (LiveConnections.liveConnections.Exists(x => x.User == recipient))
            {
                Clients.Client(LiveConnections.liveConnections.FirstOrDefault(p => p.User.Equals(recipient)).ConnectionID).AddMessageToPage(message);
            }
        }

        public void Chat(int userID, string message)
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            extmodels.User sender = new UserRepository().GetUserByID(Convert.ToInt16(identity.Claims.Last().Value));
            extmodels.User recipient = new UserRepository().GetUserByID(userID);
            if (LiveConnections.liveConnections.Exists(x => x.User.Equals(recipient)))
            {
                Clients.Client(LiveConnections.liveConnections.FirstOrDefault(p => p.User.Equals(recipient)).ConnectionID).AddMessageToChat(sender.Name,message);
                Clients.Client(LiveConnections.liveConnections.FirstOrDefault(p => p.User.Equals(sender)).ConnectionID).AddMessageToChat(sender.Name, message);
                ChatHistory.chatHistory.Add(new ChatMessage(sender,new UserRepository().GetUserByID(userID),message));
            }
            else
            {
                Clients.Client(LiveConnections.liveConnections.FirstOrDefault(p => p.User.Equals(sender)).ConnectionID).AddMessageToChat("DistriFest","Gebruiker is momenteel niet ingelogd, bericht niet aangekomen");
            }
        }

        public override Task OnConnected()
        {
            string userName = Context.User.Identity.Name;
            var identity = (ClaimsIdentity)Context.User.Identity;
            string connectionID = Context.ConnectionId;
            extmodels.User connectedUser = new UserRepository().GetUserByID(Convert.ToInt16(identity.Claims.Last().Value));
            if (LiveConnections.liveConnections.Exists(x => x.User.Equals(connectedUser)))
            {
                LiveConnections.liveConnections.FirstOrDefault(p => p.User.Equals(connectedUser)).SetConnectionID(connectionID);
            }
            else
            {
                LiveConnections.liveConnections.Add(new SignalRConnection(connectedUser, connectionID));
            }
            //Send(Convert.ToInt16(identity.Claims.Last().Value), "Hello " + userName + " you are connected with id: " + connectionID + " and your userID is: " + identity.Claims.Last().Value);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string connectionID = Context.ConnectionId;
            var identity = (ClaimsIdentity)Context.User.Identity;
            extmodels.User disconnectedUser = new UserRepository().GetUserByID(Convert.ToInt16(identity.Claims.Last().Value));
            LiveConnections.liveConnections.RemoveAll(x => x.User.Equals(disconnectedUser));
            return base.OnDisconnected(stopCalled);
        }

    }
}