// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Data.SqlClient;
using NUnit.Framework;

namespace AlwaysEncryptedSample.Models
{
    public abstract class AbstractModelTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            DbInit.CreateDatabase(new SqlConnectionStringBuilder());
            DbInit.CreateAuthContext();
            DbInit.CreatePurchasingContext();
            DbInit.InitLog4NetDb(new SqlConnection());
        }
    }

    [TestFixture]
    public class ApplicationUserTests : AbstractModelTests
    {
        [Test]
        public void TestFail()
        {
            Assert.Fail("It's a good thing I got this far.");
        }
    }
}
