using BlazorAuthTemplate.Client;
using BlazorAuthTemplate.Client.Services;
using BlazorAuthTemplate.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddScoped<ICompanyService, CompanyService>();

await builder.Build().RunAsync();
