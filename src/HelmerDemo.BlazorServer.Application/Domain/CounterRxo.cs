namespace HelmerDemo.BlazorServer.Application.Domain;

public class CounterRxo
{
	public int Value { get; set; }
	
	public string ErrorMessage { get; set; } = "";

	public CounterRxo(int value)
	{
		Value = value;
	}
	
	public void Increment()
	{
		Value++;
	}
}
