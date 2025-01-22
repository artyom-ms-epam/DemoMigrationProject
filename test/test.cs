using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightTests
{
    [TestClass]
    public class ExampleTests
    {
        private IBrowser browser;
        private IBrowserContext context;
        private IPage page;

        [TestInitialize]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            context = await browser.NewContextAsync();
            page = await context.NewPageAsync();
            await page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await page.CloseAsync();
            await context.CloseAsync();
            await browser.CloseAsync();
        }

        [TestMethod]
        public async Task TextTypingBasics()
        {
            var nameInput = page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
            await nameInput.FillAsync("Paker");
            await nameInput.FillAsync("Parker");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        [TestMethod]
        public async Task ClickArrayOfLabelsAndCheckStates()
        {
            var featureList = page.Locator(".column.col-2 label");
            for (var i = 0; i < await featureList.CountAsync(); i++)
            {
                var feature = featureList.Nth(i);
                await feature.ClickAsync();
                var checkbox = feature.Locator("input[type=checkbox]");
                (await checkbox.IsCheckedAsync()).Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task DealingWithTextUsingKeyboard()
        {
            var nameInput = page.Locator("#developer-name");
            await nameInput.FillAsync("Peter Parker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.PressAsync("Backspace");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Pete Parker");
            await nameInput.PressAsync("Home");
            await nameInput.PressAsync("ArrowRight");
            await nameInput.PressAsync(".");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            value = await nameInput.InputValueAsync();
            value.Should().Be("P. Parker");
        }

        [TestMethod]
        public async Task MovingTheSlider()
        {
            var sliderHandle = page.Locator("#slider");
            var initialOffset = await sliderHandle.EvaluateAsync<int>("e => e.offsetLeft");
            await page.CheckAsync("#tried-test-cafe");
            await sliderHandle.DragToAsync(page.Locator("#slider .ui-slider-handle"));
            var newOffset = await sliderHandle.EvaluateAsync<int>("e => e.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [TestMethod]
        public async Task DealingWithTextUsingSelection()
        {
            var nameInput = page.Locator("#developer-name");
            await nameInput.FillAsync("Test Cafe");
            await nameInput.SelectTextAsync(1, 7);
            await nameInput.PressAsync("Delete");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Tfe");
        }

        [TestMethod]
        public async Task HandleNativeConfirmationDialog()
        {
            page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await page.ClickAsync("#populate");
            var dialogHistory = page.Dialogs;
            dialogHistory[0].Message.Should().Be("Reset information before proceeding?");
            await page.ClickAsync("#submit-button");
            var results = await page.InnerTextAsync("#article-header");
            results.Should().Contain("Peter Parker");
        }

        [TestMethod]
        public async Task PickOptionFromSelect()
        {
            await page.ClickAsync("#preferred-interface");
            await page.ClickAsync("option[value='Both']");
            var value = await page.InputValueAsync("#preferred-interface");
            value.Should().Be("Both");
        }

        [TestMethod]
        public async Task FillingForm()
        {
            await page.FillAsync("#developer-name", "Bruce Wayne");
            await page.CheckAsync("#macos");
            await page.CheckAsync("#tried-test-cafe");
            await page.FillAsync("#comments", "It's...");
            await Task.Delay(500);
            await page.FillAsync("#comments", "\ngood");
            await Task.Delay(500);
            await page.FillAsync("#comments", "awesome!!!");
            await Task.Delay(500);
            await page.ClickAsync("#submit-button");
            var results = await page.InnerTextAsync("#article-header");
            results.Should().Contain("Bruce Wayne");
        }
    }
}
