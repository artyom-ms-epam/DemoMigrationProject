using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;

namespace MigrationTests
{
    [TestFixture]
    public class TestCafeExampleTests : PageTest
    {
        private Page _page;

        [SetUp]
        public async Task SetUp()
        {
            _page = await Browser.NewPageAsync();
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [TearDown]
        public async Task TearDown()
        {
            await _page.CloseAsync();
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
            var featureList = _page.Locator(".column.col-2").Locator("label");
            for (int i = 0; i < await featureList.CountAsync(); i++)
            {
                var label = featureList.Nth(i);
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
            await nameInput.PressAsync("ArrowLeft");
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
            var sliderHandle = _page.Locator("#slider").Locator(".ui-slider-handle");
            var initialOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            await _page.CheckAsync("#tried-test-cafe");
            await sliderHandle.DragToAsync(_page.Locator(".ui-slider-tick").WithText("9"));
            var newOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
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
            await _page.ClickAsync("#populate");
            var dialogHistory = await _page.EvaluateAsync<string[]>("() => window.__testCafeNativeDialogsHistory");
            dialogHistory[0].Should().Be("Reset information before proceeding?");
            await _page.ClickAsync("#submit-button");
            var resultText = await _page.Locator("#article-header").InnerTextAsync();
            resultText.Should().Contain("Peter Parker");
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
            await _page.FillAsync("#developer-name", "Bruce Wayne");
            await _page.CheckAsync("#macos");
            await _page.CheckAsync("#tried-test-cafe");
            var commentsTextArea = _page.Locator("#comments");
            await commentsTextArea.FillAsync("It's...");
            await Task.Delay(500);
            await commentsTextArea.TypeAsync("\ngood");
            await Task.Delay(500);
            await commentsTextArea.SelectTextAsync(1, 0);
            await commentsTextArea.PressAsync("Delete");
            await commentsTextArea.TypeAsync("awesome!!!");
            await Task.Delay(500);
            await _page.ClickAsync("#submit-button");
            var resultText = await _page.Locator("#article-header").InnerTextAsync();
            resultText.Should().Contain("Bruce Wayne");
        }
    }
}