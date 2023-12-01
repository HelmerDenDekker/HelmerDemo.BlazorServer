using Bunit;
using HelmerDemo.BlazorServer.Presentation.Pages;

namespace HelmerDemo.BlazorServer.Presentation.bunitTests;

// https://learn.microsoft.com/en-us/aspnet/core/blazor/test?view=aspnetcore-6.0

public class CounterTest : TestContext
{
	[Fact]
	public void Counter_Should_RenderCorrectly()
	{
		// Arrange
		var cut = RenderComponent<Counter>();
		
		//Measures time to load (250ms)
	}
	
	[Fact]
	public void Counter_Click_ShouldReturnOne() {
		// Arrange
		var cut = RenderComponent<Counter>();
		var paraElm = cut.Find("p"); //ToDo, other selector

		// Act
		cut.Find("button").Click();

		// Assert
		var paraElmText = paraElm.TextContent;
		paraElmText.MarkupMatches("Current count: 1");
		
		// https://learn.microsoft.com/en-us/aspnet/core/blazor/test?view=aspnetcore-6.0
	}
}
