using NUnit.Framework;
using System.Web.Mvc;

namespace AlwaysEncryptedSample.Controllers
{
    [TestFixture]
    public class InternalsControllerTest
    {
        InternalsController controller = new InternalsController();
        [Ignore("Because")]
        [Test]
        public void Index()
        {
            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}