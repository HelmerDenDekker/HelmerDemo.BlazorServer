using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace HelmerDemo.BlazorServer.Presentation.Benchmark;

//[MemoryDiagnoser]
public class BlazorCounterBenchmark
{
	private HttpClient _httpClient;

	[GlobalSetup]
	public void GlobalSetup()
	{
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(configuration =>
			{
				configuration.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
				}); 
			});

		_httpClient = factory.CreateClient();
	}

	[Benchmark]
	public async Task Counter() => await _httpClient.GetAsync("/counter");

	[Benchmark]
	public async Task CounterEv() => await _httpClient.GetAsync("/counterev");

	[Benchmark]
	public async Task CounterRx() => await _httpClient.GetAsync("/counterrx");
}
