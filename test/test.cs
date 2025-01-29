using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using FluentAssertions;

namespace PlaywrightTests
{
    [TestFixture]
    public class ExampleTests
    {
        private IPage _page;
        private IBrowser _browser;
        private IBrowserContext _context;

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [TearDown]
        public async Task Teardown()
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
            var featureList = _page.Locator(".column.col-2 label");
            for (int i = 0; i < await featureList.CountAsync(); i++)
            {
                var feature = featureList.Nth(i);
                await feature.ClickAsync();
                var checkbox = feature.Locator("input[type=checkbox]");
                (await checkbox.IsCheckedAsync()).Should().BeTrue();
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
            await nameInput.TypeAsync(".");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            value = await nameInput.InputValueAsync();
            value.Should().Be("P. Parker");
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var sliderHandle = _page.Locator(".ui-slider-handle");
            var initialOffset = await sliderHandle.EvaluateAsync<int>("e => e.offsetLeft");
            await _page.Locator("#tried-test-cafe").CheckAsync();
            await sliderHandle.DragToAsync(_page.Locator(".ui-slider-tick:nth-child(10)"));
            var newOffset = await sliderHandle.EvaluateAsync<int>("e => e.offsetLeft");
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
            _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await _page.Locator("#populate").ClickAsync();
            var dialogHistory = await _page.EvaluateAsync<string[]>("window.__nativeDialogs");
            dialogHistory[0].Should().Be("Reset information before proceeding?");
            await _page.Locator("#submit-button").ClickAsync();
            var results = await _page.Locator("#article-header").InnerTextAsync();
            results.Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            var interfaceSelect = _page.Locator("#preferred-interface");
            await interfaceSelect.SelectOptionAsync(new[] { "Both" });
            var value = await interfaceSelect.InputValueAsync();
            value.Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            await _page.Locator("#developer-name").FillAsync("Bruce Wayne");
            await _page.Locator("#macos").CheckAsync();
            await _page.Locator("#tried-test-cafe").CheckAsync();
            var commentsTextArea = _page.Locator("#comments");
            await commentsTextArea.FillAsync("It's...");
            await Task.Delay(500);
            await commentsTextArea.TypeAsync("\ngood");
            await Task.Delay(500);
            await commentsTextArea.SelectTextAsync();
            await commentsTextArea.PressAsync("Delete");
            await commentsTextArea.TypeAsync("awesome!!!");
            await Task.Delay(500);
            await _page.Locator("#submit-button").ClickAsync();
            var results = await _page.Locator("#article-header").InnerTextAsync();
            results.Should().Contain("Bruce Wayne");
        }
    }
}
