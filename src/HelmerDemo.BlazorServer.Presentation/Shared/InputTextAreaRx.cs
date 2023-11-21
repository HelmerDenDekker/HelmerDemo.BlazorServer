using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HelmerDemo.BlazorServer.Presentation.Shared;

public class InputTextAreaRx : InputTextArea
{
	public event EventHandler<ChangeEventArgs>? OnInput;
	
	
}
