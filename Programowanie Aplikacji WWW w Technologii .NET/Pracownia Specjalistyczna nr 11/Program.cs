using Infrastructure.ImageMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

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

app.UseAuthorization();

app.UseImageMiddleware();

app.MapRazorPages();

/*app.Use((context, next) =>
 {
     context.Response.WriteAsync("Modul 1");
     return next();
 }
 );
app.Run((context) =>
{
    return context.Response.WriteAsync("Modul 2");
});*/

app.Run();

