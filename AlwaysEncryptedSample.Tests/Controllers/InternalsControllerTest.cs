using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlwaysEncryptedSample.Controllers
{
    [TestClass]
    public class InternalsControllerTest
    {
        InternalsController controller = new InternalsController();
        [Ignore]
        [TestMethod]
        public void Index()
        {
            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}