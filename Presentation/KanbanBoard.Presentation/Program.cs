using KanbanBoard.Application.Interfaces.Services;
using KanbanBoard.Infrastructure.Services;
using KanbanBoard.Presentation.Components;
using KanbanBoard.Presentation.Configuration;
using KanbanBoard.Presentation.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>()
                  ?? throw new InvalidOperationException("ApiSettings section is missing.");

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddHttpClient("KanbanAPI", client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapBlazorHub();

app.Run();
