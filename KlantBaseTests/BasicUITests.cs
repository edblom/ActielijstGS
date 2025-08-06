using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;

namespace KlantBaseTests
{
    [TestFixture]
    public class BasicUITests : PageTest
    {
        [Test]
        public async Task Homepage_ShouldContainTitle()
        {
            // Ga naar de KlantBaseWASM URL
            await Page.GotoAsync("https://localhost:7002");

            // Controleer of de body bestaat
            var bodyExists = await Page.Locator("body").IsVisibleAsync();

            Assert.That(bodyExists, Is.True, "De homepage is niet correct geladen.");
        }
    }
}

