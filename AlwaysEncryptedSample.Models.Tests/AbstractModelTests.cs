// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation

using System.Data.SqlClient;
using System.Linq;
using Effort;
using NMemory.Linq;
using NUnit.Framework;

namespace AlwaysEncryptedSample.Models
{
    public abstract class AbstractModelTests
    {
        protected AuthDbContext AuthCtx { get; private set; }
        protected ApplicationDbContext AppCtx { get; private set; }

        [OneTimeSetUp]
        public void Setup()
        {
            var connection = DbConnectionFactory.CreateTransient();
            AuthCtx = new AuthDbContext(connection);
            AppCtx = new ApplicationDbContext(connection);
            
        }
    }

    [TestFixture]
    public class ApplicationTests : AbstractModelTests
    {
        [Test]
        public void TestAddCreditCard()
        {
            var cc = AppCtx.CreditCards.Create();
            cc.CardNumber = "xxxx-xxxx-xxxx-xxxx";
            cc.CCV = 128;
            cc.ExpMonth = 4;
            cc.ExpYear = 2022;
            cc.Network = new CreditCardNetwork
            {
                Name = "Visa"
            };
            AppCtx.CreditCards.Add(cc);
            AppCtx.SaveChanges();
            Assert.AreEqual(1,AppCtx.CreditCards.Count(c => c.CardNumber == "xxxx-xxxx-xxxx-xxxx"));
            Assert.AreEqual(1,AppCtx.CreditCardNetworks.Count(n => n.Name== "Visa"));
        }
    }
}
