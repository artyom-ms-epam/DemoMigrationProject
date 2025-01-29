using NUnit.Framework;
using FluentAssertions;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    [TestFixture]
    public class Tests
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
            await _context.CloseAsync();
            await _browser.CloseAsync();
        }

        [Test]
        public async Task TextTypingBasics()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
            await nameInput.FillAsync("Paker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.TypeAsync("r");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayOfLabelsAndCheckStates()
        {
            var labels = _page.Locator(".feature");
            for (int i = 0; i < await labels.CountAsync(); i++)
            {
                var label = labels.Nth(i);
                await label.ClickAsync();
                var checkbox = label.Locator("input[type=checkbox]");
                (await checkbox.IsCheckedAsync()).Should().BeTrue();
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Peter Parker");
            await nameInput.PressAsync("ArrowLeft", new PressOptions { Delay = 100 });
            await nameInput.PressAsync("Backspace");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Pete Parker");
            await nameInput.PressAsync("Home");
            await nameInput.PressAsync("ArrowRight");
            await nameInput.TypeAsync(". P");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            value = await nameInput.InputValueAsync();
            value.Should().Be("P. Parker");
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var slider = _page.Locator("#slider");
            var initialOffset = await slider.EvaluateAsync<int>("e => e.offsetLeft");
            await _page.Locator("#tried-test-cafe").CheckAsync();
            await slider.DragToAsync(_page.Locator(".slider-value").Nth(9));
            var newOffset = await slider.EvaluateAsync<int>("e => e.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync("Test Cafe");
            await nameInput.SelectTextAsync(1, 7);
            await nameInput.PressAsync("Delete");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Tfe");
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            _page.Dialog += (_, dialog) => dialog.AcceptAsync();
            await _page.Locator("#populate").ClickAsync();
            var dialogHistory = await _page.Locator("#populate").EvaluateAsync<string[]>("window.confirmationHistory");
            dialogHistory[0].Should().Be("Reset information before proceeding?");
            await _page.Locator("#submit-button").ClickAsync();
            var result = await _page.Locator("#result").InnerTextAsync();
            result.Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            await _page.Locator("#preferred-interface").SelectOptionAsync(new[] { "Both" });
            var value = await _page.Locator("#preferred-interface").InputValueAsync();
            value.Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            await _page.Locator("#developer-name").FillAsync("Bruce Wayne");
            await _page.Locator("#macos").CheckAsync();
            await _page.Locator("#tried-test-cafe").CheckAsync();
            var comments = _page.Locator("#comments");
            await comments.FillAsync("It's...");
            await Task.Delay(500);
            await comments.TypeAsync("\ngood");
            await Task.Delay(500);
            await comments.SelectTextAsync(0, 1);
            await comments.PressAsync("Delete");
            await comments.TypeAsync("awesome!!!");
            await Task.Delay(500);
            await _page.Locator("#submit-button").ClickAsync();
            var result = await _page.Locator("#result").InnerTextAsync();
            result.Should().Contain("Bruce Wayne");
        }
    }
}
