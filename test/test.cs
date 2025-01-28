using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace TestNamespace
{
    [Binding]
    public class TestSteps
    {
        private readonly IPage _page;

        public TestSteps(IPage page)
        {
            _page = page;
        }

        [Given(@"I navigate to the example page")]
        public async Task GivenINavigateToTheExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type text into the name input")]
        public async Task WhenITypeTextIntoTheNameInput()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Peter");
            await nameInput.FillAsync("Parker");
            await nameInput.TypeAsync("r", new LocatorTypeOptions { Position = 2 });
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Parker");
        }

        [When(@"I click an array of labels and check their states")]
        public async Task WhenIClickAnArrayOfLabelsAndCheckTheirStates()
        {
            var featureList = await _page.QuerySelectorAllAsync(".feature");
            foreach (var feature in featureList)
            {
                var label = await feature.QuerySelectorAsync("label");
                await label.ClickAsync();
                var checkbox = await feature.QuerySelectorAsync("input[type='checkbox']");
                var isChecked = await checkbox.IsCheckedAsync();
                isChecked.Should().BeTrue();
            }
        }

        [When(@"I deal with text using keyboard")]
        public async Task WhenIDealWithTextUsingKeyboard()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Peter Parker");
            await nameInput.ClickAsync(new LocatorClickOptions { Position = 5 });
            await _page.Keyboard.PressAsync("Backspace");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Pete Parker");
            await _page.Keyboard.PressAsync("Home Right . Delete Delete Delete");
            value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("P. Parker");
        }

        [When(@"I move the slider")]
        public async Task WhenIMoveTheSlider()
        {
            var sliderHandle = await _page.QuerySelectorAsync("#slider");
            var initialOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            await _page.ClickAsync("#tried-test-cafe");
            await sliderHandle.DragToAsync(await _page.QuerySelectorAsync(".slider-tick:nth-child(9)"));
            var newOffset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [When(@"I deal with text using selection")]
        public async Task WhenIDealWithTextUsingSelection()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Test Cafe");
            await nameInput.SelectTextAsync(7, 1);
            await _page.Keyboard.PressAsync("Delete");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Tfe");
        }

        [When(@"I handle native confirmation dialog")]
        public async Task WhenIHandleNativeConfirmationDialog()
        {
            await _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await _page.ClickAsync("#populate");
            var dialogs = await _page.GetDialogsAsync();
            dialogs[0].Message.Should().Be("Reset information before proceeding?");
            await _page.ClickAsync("#submit-button");
            var resultText = await _page.InnerTextAsync("#result");
            resultText.Should().Contain("Peter Parker");
        }

        [When(@"I pick option from select")]
        public async Task WhenIPickOptionFromSelect()
        {
            await _page.ClickAsync("#preferred-interface");
            await _page.ClickAsync("option[value='Both']");
            var value = await _page.GetAttributeAsync("#preferred-interface", "value");
            value.Should().Be("Both");
        }

        [When(@"I fill a form")]
        public async Task WhenIFillAForm()
        {
            await _page.TypeAsync("#developer-name", "Bruce Wayne");
            await _page.ClickAsync("#macos");
            await _page.ClickAsync("#tried-test-cafe");
            await _page.TypeAsync("#comments", "It's...");
            await _page.WaitForTimeoutAsync(500);
            await _page.TypeAsync("#comments", "\ngood");
            await _page.WaitForTimeoutAsync(500);
            await _page.SelectTextAreaContentAsync("#comments", 1, 0);
            await _page.Keyboard.PressAsync("Delete");
            await _page.TypeAsync("#comments", "awesome!!!");
            await _page.WaitForTimeoutAsync(500);
            await _page.ClickAsync("#submit-button");
            var resultText = await _page.InnerTextAsync("#result");
            resultText.Should().Contain("Bruce Wayne");
        }
    }
}
