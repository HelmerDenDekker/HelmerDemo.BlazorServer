using System.ComponentModel.DataAnnotations;

namespace HelmerDemo.BlazorServer.Presentation.ViewModel;

public class Person
{
	[StringLength(16, ErrorMessage = "Name too long (16 character limit).")]
	public string Name { get; set; }
	public string Pet { get; set; }
}
