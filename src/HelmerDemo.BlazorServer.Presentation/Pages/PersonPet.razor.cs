using System.Reactive.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

public partial class PersonPet : ComponentBase
{
	private EditContext EditContext;
	private Person person = new();

	protected override void OnInitialized()
	{
		EditContext = new EditContext(person);
		EditContext.OnFieldChanged += EditContext_OnFieldChanged;

		base.OnInitialized();
	}

	// Note: The OnFieldChanged event is raised for each field in the model
	private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
	{
		if (e.FieldIdentifier.FieldName == "Pet")
		{
			person.Name = person.Pet;
		}
	}

	public class Person
	{
		public string Name { get; set; }
		public string Pet { get; set; }
	}
}
