using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TicketControllerTests
    {
        [TestMethod]
        public void GetSingleTicket_ValidId_BringTicket()
        {
            Assert.AreEqual("mail", "mail1");
        }
    }
}
