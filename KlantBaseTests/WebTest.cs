using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace KlantBaseTests
{
    [TestFixture]
    public class WebTest : PageTest
    {
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;
        private Process? _frontendProcess;
        private Process? _apiProcess;
        private const string FrontendUrl = "https://localhost:7002";
        private const string ApiUrl = "https://localhost:44361";

        [SetUp]
        public async Task Setup()
        {
            // Controleer of de frontend draait
            bool frontendRunning = await IsServerRunningAsync(FrontendUrl);
            if (!frontendRunning)
            {
                // Start KlantBaseWASM
                var frontendStartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "run --project ../KlantBaseWASM/KlantBaseWASM.csproj --urls https://localhost:7002",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                _frontendProcess = Process.Start(frontendStartInfo);
                await Task.Delay(5000); // Wacht tot frontend opstart
            }

            // Controleer of de API draait
            bool apiRunning = await IsServerRunningAsync(ApiUrl);
            if (!apiRunning)
            {
                // Stop eventuele bestaande API-processen
                KillExistingProcess("ActieLijstApi");

                // Start ActieLijstAPI
                var apiStartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "run --project ../ActieLijstAPI/ActieLijstAPI.csproj --urls https://localhost:44361",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                _apiProcess = Process.Start(apiStartInfo);
                await Task.Delay(5000); // Wacht tot API opstart
            }

            // Start de browser
            using var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 250
            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        // Helper om te controleren of een server draait
        private async Task<bool> IsServerRunningAsync(string url)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(2);
            try
            {
                var response = await client.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Helper om bestaande processen te stoppen
        private void KillExistingProcess(string processName)
        {
            foreach (var process in Process.GetProcessesByName(processName))
            {
                try
                {
                    process.Kill();
                    process.WaitForExit(5000); // Wacht max 5 seconden
                }
                catch
                {
                    // Log eventuele fouten, maar ga door
                }
            }
        }

        [Test]
        public async Task HoofdPaginaLaadtEnNavigeertNaarActies()
        {
            // Navigeer naar de hoofdpagina
            await _page.GotoAsync(FrontendUrl);

            // Controleer of de pagina laadt
            await Expect(_page.Locator("body")).ToBeVisibleAsync(new() { Timeout = 5000 });
            await Expect(_page).ToHaveTitleAsync("KlantBase", new() { Timeout = 5000 }); // Pas titel aan

            // Wacht tot het menu laadt
            await Expect(_page.Locator(".rz-navigation-menu")).ToBeVisibleAsync(new() { Timeout = 5000 });

            // Klik op de menuoptie "Acties"
            await _page.ClickAsync(".rz-menuitem:has-text('Acties')");

            // Controleer of de Acties-pagina laadt
            await Expect(_page).ToHaveURLAsync($"{FrontendUrl}/actielijst", new() { Timeout = 5000 });
            await Expect(_page.Locator(".rz-dropdown:has-text('Voor:')")).ToBeVisibleAsync(new() { Timeout = 5000 });
        }

#pragma warning disable NUnit1032 // Onderdruk waarschuwing voor _frontendProcess en _apiProcess
        [TearDown]
        public async Task Cleanup()
        {
            // Sluit Playwright-objecten
            try
            {
                await _page.CloseAsync();
                await _context.CloseAsync();
                await _browser.CloseAsync();
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Fout bij sluiten van Playwright-objecten: {ex.Message}");
            }

            // Stop servers en dispose processen
            try
            {
                if (_frontendProcess != null)
                {
                    _frontendProcess.Kill();
                    _frontendProcess.WaitForExit(5000);
                    _frontendProcess.Dispose();
                    _frontendProcess = null;
                }
                if (_apiProcess != null)
                {
                    _apiProcess.Kill();
                    _apiProcess.WaitForExit(5000);
                    _apiProcess.Dispose();
                    _apiProcess = null;
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Fout bij stoppen van servers: {ex.Message}");
            }
        }
#pragma warning restore NUnit1032
    }
}