using HelmerDemo.BlazorServer.Application.Interfaces;
using HelmerDemo.BlazorServer.Application.Observables;
using HelmerDemo.BlazorServer.Application.Reactive;
using HelmerDemo.BlazorServer.Application.Services;
using HelmerDemo.BlazorServer.Presentation.JsInterop;
using HelmerDemo.BlazorServer.Presentation.JsInterop.Contracts;
using HelmerDemo.BlazorServer.Shared.Tools.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

AddServices(builder);
ConfigureRequestPipeline(builder.Build());

static void AddServices(WebApplicationBuilder builder)
{
	builder.Services.Configure<HeaderSettings>(options => builder.Configuration.GetSection("HeaderSettings").Bind(options));
	var corsSettings = new CorsSettings();
	builder.Configuration.GetSection("CorsSettings").Bind(corsSettings);

	builder.Services.AddSingleton<IDigitalTimeObservable, DigitalTimeObservable>();
	builder.Services.AddSingleton<IDigitalTimeStream, DigitalTimeStream>();
	builder.Services.AddDataProtection();
	builder.Services.AddSingleton<WeatherForecastService>();
	builder.Services.AddSingleton<IMessageBoxStream, MessageBoxStream>();
	builder.Services.AddScoped<ILocalStorageProvider, LocalStorageProvider>();
	// Add services to the container.
	builder.Services.AddRazorPages();
	builder.Services.AddServerSideBlazor();
	builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
}

// Configure the HTTP request pipeline
static void ConfigureRequestPipeline(WebApplication app)
{
	//ToDo https://learn.microsoft.com/en-us/aspnet/core/blazor/security/content-security-policy?view=aspnetcore-7.0
	#if !DEBUG
	app.UseMiddleware<WebSecurityMiddleware>();
	#endif

	// Configure the HTTP request pipeline.
	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}

	app.UseHttpsRedirection();

	app.UseStaticFiles();

	app.UseRouting();

	app.MapBlazorHub();
	app.MapFallbackToPage("/_Host");

	app.Run();
}