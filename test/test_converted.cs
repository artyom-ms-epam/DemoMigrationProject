using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using FluentAssertions;

namespace PlaywrightTests
{
    [TestFixture]
    public class ExampleTests : PageTest
    {
        [SetUp]
        public async Task SetUp()
        {
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [Test]
        public async Task TextTypingBasics()
        {
            var nameInput = Page.Locator("#developer-name");
            await nameInput.FillAsync("Peter");
            await nameInput.FillAsync("Paker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.TypeAsync("r");
            (await nameInput.InputValueAsync()).Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayLabels()
        {
            var featureList = Page.Locator(".column.col-2 input[type=checkbox]");
            var count = await featureList.CountAsync();
            for (int i = 0; i < count; i++)
            {
                var feature = featureList.Nth(i);
                await feature.CheckAsync();
                (await feature.IsCheckedAsync()).Should().BeTrue();
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
        {
            var nameInput = Page.Locator("#developer-name");
            await nameInput.FillAsync("Peter Parker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.PressAsync("Backspace");
            (await nameInput.InputValueAsync()).Should().Be("Pete Parker");
            await nameInput.PressAsync("Home");
            await nameInput.PressAsync("ArrowRight");
            await nameInput.TypeAsync(". ");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            (await nameInput.InputValueAsync()).Should().Be("P. Parker");
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var slider = Page.Locator("#slider");
            var initialOffset = await slider.EvaluateAsync<int>("el => el.offsetLeft");
            await Page.CheckAsync("#tried-test-cafe");
            await slider.DragToAsync(Page.Locator(".slider-value").Nth(9));
            (await slider.EvaluateAsync<int>("el => el.offsetLeft")).Should().BeGreaterThan(initialOffset);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            var nameInput = Page.Locator("#developer-name");
            await nameInput.FillAsync("Test Cafe");
            await nameInput.SelectTextAsync(1, 7);
            await nameInput.PressAsync("Delete");
            (await nameInput.InputValueAsync()).Should().Be("Tfe");
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            Page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await Page.ClickAsync("#populate");
            var dialogHistory = await Page.EvaluateAsync<Dialog[]>("window.dialogHistory");
            dialogHistory[0].Message.Should().Be("Reset information before proceeding?");
            await Page.ClickAsync("#submit-button");
            (await Page.InnerTextAsync("#article-header")).Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            await Page.SelectOptionAsync("#preferred-interface", new[] { "Both" });
            (await Page.InputValueAsync("#preferred-interface")).Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            await Page.FillAsync("#developer-name", "Bruce Wayne");
            await Page.CheckAsync("#macos");
            await Page.CheckAsync("#tried-test-cafe");
            await Page.FillAsync("#comments", "It's...");
            await Page.WaitForTimeoutAsync(500);
            await Page.FillAsync("#comments", "It's...
good");
            await Page.WaitForTimeoutAsync(500);
            await Page.FillAsync("#comments", "awesome!!!");
            await Page.WaitForTimeoutAsync(500);
            await Page.ClickAsync("#submit-button");
            (await Page.InnerTextAsync("#article-header")).Should().Contain("Bruce Wayne");
        }
    }
}
