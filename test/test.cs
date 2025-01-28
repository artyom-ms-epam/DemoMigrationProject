using NUnit.Framework;
using FluentAssertions;
using Microsoft.Playwright;
using TechTalk.SpecFlow;

namespace TestProject.StepDefinitions
{
    [Binding]
    public class TestSteps
    {
        private readonly IPage _page;

        public TestSteps(IPage page)
        {
            _page = page;
        }

        [Given(@"I navigate to the test page")]
        public async Task GivenINavigateToTheTestPage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type the name 'Peter' into the input field")]
        public async Task WhenITypeTheNamePeterIntoTheInputField()
        {
            await _page.FillAsync("#developer-name", "Peter");
        }

        [When(@"I replace the name with 'Parker'")]
        public async Task WhenIReplaceTheNameWithParker()
        {
            await _page.FillAsync("#developer-name", "Parker");
        }

        [Then(@"the input field should contain 'Parker'")]
        public async Task ThenTheInputFieldShouldContainParker()
        {
            var value = await _page.InputValueAsync("#developer-name");
            value.Should().Be("Parker");
        }

        [When(@"I type the name 'Peter Parker' into the input field")]
        public async Task WhenITypeTheNamePeterParkerIntoTheInputField()
        {
            await _page.FillAsync("#developer-name", "Peter Parker");
        }

        [When(@"I move the caret position to 5 and press backspace")]
        public async Task WhenIMoveTheCaretPositionTo5AndPressBackspace()
        {
            await _page.PressAsync("#developer-name", "ArrowRight");
            await _page.PressAsync("#developer-name", "Backspace");
        }

        [Then(@"the input field should contain 'Pete Parker'")]
        public async Task ThenTheInputFieldShouldContainPeteParker()
        {
            var value = await _page.InputValueAsync("#developer-name");
            value.Should().Be("Pete Parker");
        }

        [When(@"I click the slider handle and drag it to 9")]
        public async Task WhenIClickTheSliderHandleAndDragItTo9()
        {
            var sliderHandle = _page.Locator(".ui-slider-handle");
            await sliderHandle.DragToAsync(_page.Locator(".ui-slider-tick").WithText("9"));
        }

        [Then(@"the slider handle should move to the right")]
        public async Task ThenTheSliderHandleShouldMoveToTheRight()
        {
            var sliderHandle = _page.Locator(".ui-slider-handle");
            var offset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            offset.Should().BeGreaterThan(0);
        }
    }
}
