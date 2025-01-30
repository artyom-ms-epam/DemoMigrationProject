using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using Microsoft.Playwright;

namespace SpecFlowProjectConverted.Features
{
    [Binding]
    public class TestFileSteps
    {
        private readonly IPage _page;

        public TestFileSteps(IPage page)
        {
            _page = page;
        }

        [Given(@"I navigate to the TestCafe example page")]
        public async Task GivenINavigateToTheTestCafeExamplePage()
        {
            await _page.GotoAsync("https://devexpress.github.io/testcafe/example/");
        }

        [When(@"I type name '([^']*)' into the name input")]
        public async Task WhenITypeNameIntoTheNameInput(string name)
        {
            var nameInput = _page.Locator("#developer-name");
            await nameInput.FillAsync(name);
        }

        [Then(@"the name input should have value '([^']*)'")]
        public async Task ThenTheNameInputShouldHaveValue(string expectedValue)
        {
            var nameInput = _page.Locator("#developer-name");
            var actualValue = await nameInput.InputValueAsync();
            actualValue.Should().Be(expectedValue);
        }

        [When(@"I click the '([^']*)' checkbox")]
        public async Task WhenIClickTheCheckbox(string checkboxLabel)
        {
            var checkbox = _page.Locator($"label:has-text('{checkboxLabel}')").Locator("input[type='checkbox']");
            await checkbox.CheckAsync();
        }

        [Then(@"the '([^']*)' checkbox should be checked")]
        public async Task ThenTheCheckboxShouldBeChecked(string checkboxLabel)
        {
            var checkbox = _page.Locator($"label:has-text('{checkboxLabel}')").Locator("input[type='checkbox']");
            var isChecked = await checkbox.IsCheckedAsync();
            isChecked.Should().BeTrue();
        }

        [When(@"I move the slider to '([^']*)'")]
        public async Task WhenIMoveTheSliderTo(string value)
        {
            var sliderHandle = _page.Locator("#slider").Locator(".ui-slider-handle");
            await sliderHandle.DragToAsync(_page.Locator($".ui-slider-tick:has-text('{value}')"));
        }

        [Then(@"the slider value should be greater than '([^']*)'")]
        public async Task ThenTheSliderValueShouldBeGreaterThan(string initialValue)
        {
            var sliderHandle = _page.Locator("#slider").Locator(".ui-slider-handle");
            var offset = await sliderHandle.EvaluateAsync<int>("el => el.offsetLeft");
            offset.Should().BeGreaterThan(int.Parse(initialValue));
        }

        [When(@"I select '([^']*)' from the interface select")]
        public async Task WhenISelectFromTheInterfaceSelect(string option)
        {
            var select = _page.Locator("#preferred-interface");
            await select.SelectOptionAsync(new SelectOptionValue { Label = option });
        }

        [Then(@"the interface select should have value '([^']*)'")]
        public async Task ThenTheInterfaceSelectShouldHaveValue(string expectedValue)
        {
            var select = _page.Locator("#preferred-interface");
            var actualValue = await select.InputValueAsync();
            actualValue.Should().Be(expectedValue);
        }

        [When(@"I fill the form with name '([^']*)' and OS '([^']*)' and comment '([^']*)'")]
        public async Task WhenIFillTheFormWithNameAndOSAndComment(string name, string os, string comment)
        {
            await WhenITypeNameIntoTheNameInput(name);
            await WhenIClickTheCheckbox(os);
            var commentTextArea = _page.Locator("#comments");
            await commentTextArea.FillAsync(comment);
        }

        [Then(@"the form should be submitted with name '([^']*)'")]
        public async Task ThenTheFormShouldBeSubmittedWithName(string expectedName)
        {
            var results = _page.Locator("#article-header");
            var resultText = await results.InnerTextAsync();
            resultText.Should().Contain(expectedName);
        }
    }
}