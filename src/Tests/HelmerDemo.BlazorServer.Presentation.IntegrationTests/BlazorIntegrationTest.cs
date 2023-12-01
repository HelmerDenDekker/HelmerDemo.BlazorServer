using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HelmerDemo.BlazorServer.Presentation.IntegrationTests;

public class BlazorIntegrationTest
{
	/// <summary>
	/// To prove the fact you can test Blazor pages
	/// </summary>
	[Fact]
	public async Task Get_Request_ShouldReturnPage()
	{
		// --arrange--
        
		var application = new WebApplicationFactory<Program>();
		var httpClient = application.CreateClient();
        
		// --act--
		var response = await httpClient.GetAsync("");
        
		// --assert-- 
		
		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}
}
