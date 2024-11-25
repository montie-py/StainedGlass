using StainedGlass.Transfer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//add services to the container
builder.Services.AddTransient<InputBoundary, UseCaseInteractor>();

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

//for controllers to work
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
}); 

app.UseAuthorization();

app.MapRazorPages();

app.Run();
