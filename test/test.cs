using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using TechTalk.SpecFlow;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace TestNamespace
{
    [Binding]
    public class TestSteps
    {
        private IPage _page;

        public TestSteps(IPage page)
        {
            _page = page;
        }

        [Given(@"I navigate to the example page")]
        public async Task GivenINavigateToTheExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type text in the name input")]
        public async Task WhenITypeTextInTheNameInput()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Peter");
            await nameInput.FillAsync("Parker");
            await nameInput.PressAsync("ArrowLeft");
            await nameInput.TypeAsync("r");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Parker");
        }

        [When(@"I click an array of labels and check their states")]
        public async Task WhenIClickAnArrayOfLabelsAndCheckTheirStates()
        {
            var features = await _page.QuerySelectorAllAsync(".feature");
            foreach (var feature in features)
            {
                var label = await feature.QuerySelectorAsync("label");
                await label.ClickAsync();
                var checkbox = await feature.QuerySelectorAsync("input[type=checkbox]");
                var isChecked = await checkbox.IsCheckedAsync();
                isChecked.Should().BeTrue();
            }
        }

        [When(@"I deal with text using keyboard")]
        public async Task WhenIDealWithTextUsingKeyboard()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Peter Parker");
            await nameInput.ClickAsync(new ClickOptions { Position = new Position { X = 5, Y = 5 } });
            await nameInput.PressAsync("Backspace");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Pete Parker");
            await nameInput.PressAsync("Home");
            await nameInput.PressAsync("ArrowRight");
            await nameInput.TypeAsync(".");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            await nameInput.PressAsync("Delete");
            value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("P. Parker");
        }

        [When(@"I move the slider")]
        public async Task WhenIMoveTheSlider()
        {
            var slider = await _page.QuerySelectorAsync("#slider");
            var initialOffset = await slider.EvaluateAsync<int>("el => el.offsetLeft");
            var handle = await slider.QuerySelectorAsync(".ui-slider-handle");
            await handle.DragToAsync(await _page.QuerySelectorAsync(".ui-slider-tick[data-value='9']"));
            var newOffset = await slider.EvaluateAsync<int>("el => el.offsetLeft");
            newOffset.Should().BeGreaterThan(initialOffset);
        }

        [When(@"I deal with text using selection")]
        public async Task WhenIDealWithTextUsingSelection()
        {
            var nameInput = await _page.QuerySelectorAsync("#developer-name");
            await nameInput.TypeAsync("Test Cafe");
            await nameInput.EvaluateAsync("el => el.setSelectionRange(1, 7)");
            await nameInput.PressAsync("Delete");
            var value = await nameInput.GetAttributeAsync("value");
            value.Should().Be("Tfe");
        }

        [When(@"I handle native confirmation dialog")]
        public async Task WhenIHandleNativeConfirmationDialog()
        {
            await _page.RouteAsync("**/*", route => route.ContinueAsync());
            var dialog = await _page.RunAndWaitForDialogAsync(async () =>
            {
                await _page.ClickAsync("#populate");
            });
            dialog.Message.Should().Be("Reset information before proceeding?");
            await dialog.AcceptAsync();
            await _page.ClickAsync("#submit-button");
            var result = await _page.InnerTextAsync("#result");
            result.Should().Contain("Peter Parker");
        }

        [When(@"I pick option from select")]
        public async Task WhenIPickOptionFromSelect()
        {
            var select = await _page.QuerySelectorAsync("#preferred-interface");
            await select.SelectOptionAsync(new SelectOptionValue { Label = "Both" });
            var value = await select.GetAttributeAsync("value");
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
            await _page.EvaluateAsync("el => el.select()", await _page.QuerySelectorAsync("#comments"));
            await _page.PressAsync("#comments", "Delete");
            await _page.TypeAsync("#comments", "awesome!!!");
            await _page.WaitForTimeoutAsync(500);
            await _page.ClickAsync("#submit-button");
            var result = await _page.InnerTextAsync("#result");
            result.Should().Contain("Bruce Wayne");
        }
    }
}
