using HelmerDemo.BlazorServer.Application.Reactive;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Components;

public partial class Root : ComponentBase
{
	[Inject]
	public NavigationManager MyNavigationManager { get; set; } = default!;
	
	[Inject]
	public RootActor MyRootActor { get; set; } = default!;
	
	[Parameter]
	public RenderFragment? ChildContent { get; set; }
}

