using FluentAssertions;
using NUnit.Framework;
using Microsoft.Playwright.NUnit;

namespace TestNamespace
{
    [TestFixture]
    public class TestClass : PageTest
    {
        [Test]
        public async Task TextTypingBasics()
        {
            var page = new Page();
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            await Page.FillAsync(page.NameInput, "Peter");
            await Page.FillAsync(page.NameInput, "Paker", new PageFillOptions { Replace = true });
            await Page.FillAsync(page.NameInput, "r", new PageFillOptions { CaretPosition = 2 });
            (await Page.InputValueAsync(page.NameInput)).Should().Be("Parker");
        }

        [Test]
        public async Task ClickArrayOfLabelsAndCheckStates()
        {
            var page = new Page();
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            foreach (var feature in page.FeatureList)
            {
                await Page.ClickAsync(feature.Label);
                (await Page.IsCheckedAsync(feature.Checkbox)).Should().BeTrue();
            }
        }

        [Test]
        public async Task DealingWithTextUsingKeyboard()
        {
            var page = new Page();
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            await Page.FillAsync(page.NameInput, "Peter Parker");
            await Page.ClickAsync(page.NameInput, new PageClickOptions { Position = new Position { X = 5, Y = 0 } });
            await Page.PressAsync(page.NameInput, "Backspace");
            (await Page.InputValueAsync(page.NameInput)).Should().Be("Pete Parker");
            await Page.PressAsync(page.NameInput, "Home Right . Delete Delete Delete");
            (await Page.InputValueAsync(page.NameInput)).Should().Be("P. Parker");
        }

        [Test]
        public async Task MovingTheSlider()
        {
            var page = new Page();
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            var initialOffset = await Page.EvalOnSelectorAsync<int>(page.Slider.Handle, "el => el.offsetLeft");
            await Page.ClickAsync(page.TriedTestCafeCheckbox);
            await Page.DragToAsync(page.Slider.Handle, page.Slider.Tick.WithText("9"));
            (await Page.EvalOnSelectorAsync<int>(page.Slider.Handle, "el => el.offsetLeft")).Should().BeGreaterThan(initialOffset);
        }

        [Test]
        public async Task DealingWithTextUsingSelection()
        {
            var page = new Page();
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            await Page.FillAsync(page.NameInput, "Test Cafe");
            await Page.SelectTextAsync(page.NameInput, 7, 1);
            await Page.PressAsync(page.NameInput, "Delete");
            (await Page.InputValueAsync(page.NameInput)).Should().Be("Tfe");
        }

        [Test]
        public async Task HandleNativeConfirmationDialog()
        {
            var page = new Page();
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            Page.Dialog += (_, dialog) => dialog.Accept();
            await Page.ClickAsync(page.PopulateButton);
            var dialogHistory = await Page.GetDialogHistoryAsync();
            dialogHistory[0].Message.Should().Be("Reset information before proceeding?");
            await Page.ClickAsync(page.SubmitButton);
            (await Page.InnerTextAsync(page.Results)).Should().Contain("Peter Parker");
        }

        [Test]
        public async Task PickOptionFromSelect()
        {
            var page = new Page();
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            await Page.ClickAsync(page.InterfaceSelect);
            await Page.ClickAsync(page.InterfaceSelectOption.WithText("Both"));
            (await Page.InputValueAsync(page.InterfaceSelect)).Should().Be("Both");
        }

        [Test]
        public async Task FillingAForm()
        {
            var page = new Page();
            await Page.GotoAsync("https://devexpress.github.io/testcafe/example/");
            await Page.FillAsync(page.NameInput, "Bruce Wayne");
            await Page.ClickAsync(page.MacOSRadioButton);
            await Page.ClickAsync(page.TriedTestCafeCheckbox);
            await Page.FillAsync(page.CommentsTextArea, "It's...");
            await Page.WaitForTimeoutAsync(500);
            await Page.FillAsync(page.CommentsTextArea, "\ngood");
            await Page.WaitForTimeoutAsync(500);
            await Page.SelectTextAreaContentAsync(page.CommentsTextArea, 1, 0);
            await Page.PressAsync(page.CommentsTextArea, "Delete");
            await Page.FillAsync(page.CommentsTextArea, "awesome!!!");
            await Page.WaitForTimeoutAsync(500);
            await Page.ClickAsync(page.SubmitButton);
            (await Page.InnerTextAsync(page.Results)).Should().Contain("Bruce Wayne");
        }
    }
}