using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Presentation.ViewModel;

internal class CounterViewModel
{
	public int CurrentCount { get; set; }
	
	public string ErrorStyle { get; set; } = "";
	public string ErrorMessage { get; set; } = "";
	
	public bool Disabled => !string.IsNullOrWhiteSpace(ErrorMessage);
	
	public void FromRxo(CounterRxo rxo)
	{
		CurrentCount = rxo.Value;
		ErrorMessage = rxo.ErrorMessage;
	}
}
