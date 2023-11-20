using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

public partial class PersonPetRx : ComponentBase
{
	private EditContext EditContext;
	private Person person = new();

	protected override void OnInitialized()
	{
		EditContext = new EditContext(person);
		// Note: The OnFieldChanged event is raised for each field in the model
		var obs = Observable.FromEventPattern<EventHandler<FieldChangedEventArgs>, FieldChangedEventArgs>(
			x => EditContext.OnFieldChanged += x,
			x => EditContext.OnFieldChanged -= x);
		obs.Where(e => e.EventArgs.FieldIdentifier.FieldName.Equals("Pet")).Subscribe(p => PersonGetsPetName());

		base.OnInitialized();
	}
	
	private void PersonGetsPetName()
	{
		person.Name = person.Pet; 
	}

	public class Person
	{
		public string Name { get; set; }
		public string Pet { get; set; }
	}
}
