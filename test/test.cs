using NUnit.Framework;
using Microsoft.Playwright;
using FluentAssertions;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    [TestFixture]
    public class ExampleTests
    {
        private IPage _page;
        private IBrowser _browser;
        private IBrowserContext _context;

        [SetUp]
        public async Task SetUp()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [TearDown]
        public async Task TearDown()
        {
            await _page.CloseAsync();
            await _context.CloseAsync();
            await _browser.CloseAsync();
        }

        [Test]
        public async Task TextTypingBasics()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
            await nameInput.FillAsync("Paker");
            await nameInput.FillAsync("Parker");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayOfLabelsAndCheckStates()
        {
            var featureList = _page.Locator(".column.col-2 label");
            var count = await featureList.CountAsync();
            for (int i = 0; i < count; i++)
            {
                var feature = featureList.Nth(i);
                await feature.ClickAsync();
                var checkbox = feature.Locator("input[type=checkbox]");
                var checkedState = await checkbox.IsCheckedAsync();
                checkedState.Should().BeTrue();
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
        {
            var nameInput = _page.Locator("#developer-name");
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

        [Test]
        public async Task MovingTheSlider()
        {
            var sliderHandle = _page.Locator("#slider");
            var initialOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            await _page.ClickAsync("#tried-test-cafe");
            await sliderHandle.DragToAsync(_page.Locator("#slider .ui-slider-handle"));
            var newOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Test Cafe");
            await nameInput.SelectTextAsync(7, 1);
            await nameInput.PressAsync("Delete");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Tfe");
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await _page.ClickAsync("#populate");
            var dialogHistory = await _page.EvaluateAsync<string[]>("window._dialogHistory");
            dialogHistory[0].Should().Be("Reset information before proceeding?");
            await _page.ClickAsync("#submit-button");
            var results = await _page.InnerTextAsync("#results");
            results.Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            await _page.ClickAsync("#preferred-interface");
            await _page.ClickAsync("#preferred-interface option[value='Both']");
            var value = await _page.InputValueAsync("#preferred-interface");
            value.Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            await _page.FillAsync("#developer-name", "Bruce Wayne");
            await _page.ClickAsync("#macos");
            await _page.ClickAsync("#tried-test-cafe");
            await _page.FillAsync("#comments", "It's...");
            await Task.Delay(500);
            await _page.FillAsync("#comments", "\ngood");
            await Task.Delay(500);
            await _page.SelectTextAsync("#comments");
            await _page.PressAsync("Delete");
            await _page.FillAsync("#comments", "awesome!!!");
            await Task.Delay(500);
            await _page.ClickAsync("#submit-button");
            var results = await _page.InnerTextAsync("#results");
            results.Should().Contain("Bruce Wayne");
        }
    }
}
