using System;
using NUnit.Framework;

namespace AlwaysEncryptedSample.Models
{
    [TestFixture]
    public class CreditCardTests
    {
        [Test]
        public void TestConstructor()
        {
            var creditCard = new CreditCard();
            Assert.GreaterOrEqual(1, (DateTime.Now  - creditCard.ModifiedDate).Seconds);
            Assert.AreEqual(0,creditCard.CreditCardId);
            Assert.AreEqual(0,creditCard.CCV);
            Assert.AreEqual(0, creditCard.ExpMonth);
            Assert.AreEqual(0, creditCard.ExpYear);
            Assert.IsNull(creditCard.CardNumber);
        }
    }
}