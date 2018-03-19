using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

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
    }
}
