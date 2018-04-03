using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using extmodels = Models;

namespace DistriFest.Models.SignalR
{
    public class ChatMessage
    {

        public extmodels.User Sender { get; private set; }
        public extmodels.User Recipient { get; private set; }
        public string Message { get; private set; }
        public DateTime Date { get; private set; }

        public ChatMessage(extmodels.User _sender, extmodels.User _recipient, string _message)
        {
            Sender = _sender;
            Recipient = _recipient;
            Message = _message;
            Date = DateTime.Now;
        }
        public ChatMessage()
        {

        }
    }
}