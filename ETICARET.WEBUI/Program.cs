using ETICARET.BLL.Abstract;
using ETICARET.BLL.Concrete;
using ETICARET.DAL.Abstract;
using ETICARET.DAL.Concrete.EFCore;
using ETICARET.DAL.Concrete.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Dependency Injection
builder.Services.AddScoped<IProductDal, MemoryProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();

//MVC Mimarisini Tanımladım.
builder.Services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);

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

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
});

app.Run();
