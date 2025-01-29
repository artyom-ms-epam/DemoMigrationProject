using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using FluentAssertions;

namespace PlaywrightTests
{
    [TestFixture]
    public class TestClass
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
            await nameInput.FillAsync("Parker");
            var value = await nameInput.InputValueAsync();
            value.Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayOfLabels()
        {
            var featureList = _page.Locator(".column.col-2").Locator("label");
            var count = await featureList.CountAsync();
            for (int i = 0; i < count; i++)
            {
                var feature = featureList.Nth(i);
                await feature.ClickAsync();
                var checkbox = feature.Locator("input[type=checkbox]");
                var isChecked = await checkbox.IsCheckedAsync();
                isChecked.Should().BeTrue();
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
            var initialOffset = await sliderHandle.BoundingBoxAsync();
            await _page.Locator("#tried-test-cafe").ClickAsync();
            await sliderHandle.DragToAsync(_page.Locator("#slider .ui-slider-handle"));
            var finalOffset = await sliderHandle.BoundingBoxAsync();
            finalOffset.X.Should().BeGreaterThan(initialOffset.X);
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
            _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await _page.Locator("#populate").ClickAsync();
            var dialogHistory = await _page.Locator("#populate").EvaluateAsync<string[]>("window.confirmationHistory");
            dialogHistory[0].Should().Be("Reset information before proceeding?");
            await _page.Locator("#submit-button").ClickAsync();
            var results = await _page.Locator("#results").InnerTextAsync();
            results.Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            var interfaceSelect = _page.Locator("#preferred-interface");
            await interfaceSelect.SelectOptionAsync(new SelectOptionValue { Label = "Both" });
            var value = await interfaceSelect.InputValueAsync();
            value.Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            await _page.Locator("#developer-name").FillAsync("Bruce Wayne");
            await _page.Locator("#macos").ClickAsync();
            await _page.Locator("#tried-test-cafe").ClickAsync();
            var commentsTextArea = _page.Locator("#comments");
            await commentsTextArea.FillAsync("It's...");
            await Task.Delay(500);
            await commentsTextArea.TypeAsync("\ngood");
            await Task.Delay(500);
            await commentsTextArea.SelectTextAsync(1, 0);
            await commentsTextArea.PressAsync("Delete");
            await commentsTextArea.TypeAsync("awesome!!!");
            await Task.Delay(500);
            await _page.Locator("#submit-button").ClickAsync();
            var results = await _page.Locator("#results").InnerTextAsync();
            results.Should().Contain("Bruce Wayne");
        }
    }
}