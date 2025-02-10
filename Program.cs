using BitzArt.Blazor.Cookies;
using StartupWing_BlazorServer.Components;
using StartupWing_BlazorServer.Components.Manager;
using StartupWing_BlazorServer.Components.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddBlazorCookies();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<DataManager>();
builder.Services.AddTransient<APIService>();
builder.Services.AddTransient<SwingServerApiService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<GoogleSheetsService>();
builder.Services.AddHttpClient<APIService>(c => c.BaseAddress = new Uri("http://ec2-3-34-192-160.ap-northeast-2.compute.amazonaws.com:7676"));
builder.Services.AddHttpClient<SwingServerApiService>(c => c.BaseAddress = new Uri("https://dev-api.startupwing.net"))
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = true,
        Credentials = System.Net.CredentialCache.DefaultCredentials
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.Run();
app.Run("http://0.0.0.0:6676");
