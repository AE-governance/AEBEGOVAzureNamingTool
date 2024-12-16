using AzureNamingTool.Attributes;
using AzureNamingTool.Components;
using AzureNamingTool.Helpers;
using AzureNamingTool.Models;
using BlazorDownloadFile;
using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Set GLOBAL json parse options
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNameCaseInsensitive = true;
    options.ReferenceHandler = ReferenceHandler.Preserve;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents().AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        options.EnableDetailedErrors = false;
        options.HandshakeTimeout = TimeSpan.FromSeconds(15);
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        options.MaximumParallelInvocationsPerClient = 1;
        options.MaximumReceiveMessageSize = 102400000;
        options.StreamBufferCapacity = 10;
    }).Services.Configure<JsonOptions>(options =>
    {
        // Configure ServerComponentSerializer JSON options
        options.SerializerOptions.PropertyNameCaseInsensitive = true;
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// Configure HttpClient JSON options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.WriteIndented = true;
});

builder.Services.AddHealthChecks();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredModal();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<StateContainer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<CustomHeaderSwaggerAttribute>();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v" + ConfigurationHelper.GetAssemblyVersion(),
        Title = "Azure Naming Tool API",
        Description = "An ASP.NET Core Web API for managing the Azure Naming tool configuration. All API requests require the configured API Keys (found in the site Admin configuration). You can find more details in the <a href=\"https://github.com/mspnp/AzureNamingTool/wiki/Using-the-API\" target=\"_new\">Azure Naming Tool API documentation</a>."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Add services to the container
builder.Services.AddBlazorDownloadFile();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredModal();
builder.Services.AddMemoryCache();
builder.Services.AddMvcCore().AddApiExplorer().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});


// // Json loop fix
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});

var app = builder.Build();

app.MapHealthChecks("/healthcheck/ping");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzureNamingToolAPI"));

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStatusCodePagesWithRedirects("/404");

app.MapControllers();
app.Run();


/// <summary>
/// Exists so can be used as reference for WebApplicationFactory in tests project
/// </summary>
public partial class Program
{
}