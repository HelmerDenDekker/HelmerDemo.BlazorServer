using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

public partial class PersonPetRx : ComponentBase
{
	protected EditContext _editContext;
	
	protected Person _person = new();


	private InputTextArea _inputTextReference;
	
	protected override void OnInitialized()
	{
		// I think it is better to ref and listen fromm there (OnAfterRender)
		_editContext = new EditContext(_person);
		// Note: The OnFieldChanged event is raised for each field in the model
		var obs = Observable.FromEventPattern<EventHandler<FieldChangedEventArgs>, FieldChangedEventArgs>(
			x => _editContext.OnFieldChanged += x,
			x => _editContext.OnFieldChanged -= x);
		
		obs.Do(p => LogEvents($"Changed from editform context {p.EventArgs.FieldIdentifier.FieldName}")).Subscribe();
		obs.Where(e => e.EventArgs.FieldIdentifier.FieldName.Equals("Pet")).Subscribe(p => PersonGetsPetName());

		var obstst = Observable.FromEventPattern<FieldChangedEventArgs>(_editContext, "OnFieldChanged");
		obstst.Do(p => LogEvents($"Changed from edFrm evPattern context {p.EventArgs}")).Subscribe();
		

		base.OnInitialized();
	}
	
	

	private void LogTextEvents(ChangeEventArgs objEventArgs)
	{
		Console.WriteLine("Field changed: " + objEventArgs);
	}

	private void PersonGetsPetName()
	{
		_person.Name = _person.Pet; 
	}

	private void LogEvents(string e)
	{
		Console.WriteLine("Field changed: " + e);
	}

	public class Person
	{
		[StringLength(16, ErrorMessage = "Name too long (16 character limit).")]
		public string Name { get; set; }
		public string Pet { get; set; }
	}

	private void Submit(EditContext obj)
	{
		Console.WriteLine($"Form submitted: {_person.Name} has pet type {_person.Pet}" );
	}

	private void TextAreaInputChange(ChangeEventArgs obj)
	{
		Console.WriteLine("Field changed: From Input " + obj);
	}
}
