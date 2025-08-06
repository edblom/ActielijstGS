using NUnit.Framework;

namespace KlantBaseTests
{
    [TestFixture]
    public class SanityCheckTests
    {
        [Test]
        public void ShouldPassSanityCheck()
        {
            Assert.That(true, Is.True);
        }
    }
}
