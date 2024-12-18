using System.ComponentModel.DataAnnotations;

namespace HelmerDemo.BlazorServer.Presentation.ViewModel;

public class TextAreaViewModel
{
	[Required(AllowEmptyStrings = false)]
	public string Content { get; set; } = string.Empty;
}
