using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using DistriFest.Models.SignalR;
using Repositories;



namespace DFUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MailTest()
        {
            Mail mail = new Mail("distrifest@notacorrect.nl", "dennis.aspers@gmail.com", "TEST");
            mail.SendMail();
        }

        [TestMethod]
        public void DictionaryTest()
        {
            
            User x = new UserRepository().GetUserByID(1);
            User y = new UserRepository().GetUserByID(2);
            LiveConnections.liveConnections.Add(new SignalRConnection(x, "constring1"));
            LiveConnections.liveConnections.Add(new SignalRConnection(y, "constring2"));
            bool bol = y.Equals(x);
            int index = LiveConnections.liveConnections.FindIndex(item => item.User.Equals(y));
            Console.WriteLine(x.ID);
            Console.WriteLine(y.ID);
            Console.WriteLine(bol);
            Console.WriteLine(index);
            if (index > 0)
            {
                
            }
            else
            {
                
            }

        }
    }
}
