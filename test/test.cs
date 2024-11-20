using System;
using PlaywrightSharp;
using TechTalk.SpecFlow;

namespace MyNamespace
{
    [Binding]
    public class MyTestClass
    {
        private readonly IPlaywright _playwright;
        private readonly IBrowser _browser;
        private readonly IPage _page;

        public MyTestClass(IPlaywright playwright)
        {
            _playwright = playwright;
            _browser = _playwright.Webkit.LaunchAsync().GetAwaiter().GetResult();
            _page = _browser.NewPageAsync().GetAwaiter().GetResult();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _page.GotoAsync("https://devexpress.github.io/testcafe/example/").GetAwaiter().GetResult();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _browser.CloseAsync().GetAwaiter().GetResult();
            _playwright.Dispose();
        }

        // Tests
    }
}