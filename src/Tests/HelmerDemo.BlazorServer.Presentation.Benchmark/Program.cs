using BenchmarkDotNet.Running;
using HelmerDemo.BlazorServer.Presentation.Benchmark;

public class Program
{
	//public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
	public static void Main(string[] args)
	{
		var summary = BenchmarkRunner.Run<BlazorCounterBenchmark>();
	}
}
