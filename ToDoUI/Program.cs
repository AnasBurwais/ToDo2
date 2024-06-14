using Microsoft.EntityFrameworkCore;
using ToDo.Persistance;
using ToDoUI.Components;
using ToDo.ServiceInterface;
using ToDo.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<ToDoContext>
    (
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            name: "DBConnection")
        )
    );
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
