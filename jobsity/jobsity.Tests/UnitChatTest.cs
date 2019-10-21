using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jobsity.Web.Controllers;
using System.Web.Mvc;
using jobsity.Repository;

namespace jobsity.Tests.Controllers
{
    [TestClass]
    public class UnitChatTest
    {
        ChatController chatController = new ChatController();

        [TestMethod]
        public void ViewChats()
        {
            ViewResult result = chatController.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateChat()
        {
            try
            {
                chatController.CreateMessage(new Message { IdUser = 1, Message1 = "Test" });
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.IsTrue(false);
            }
        }
    }
}
