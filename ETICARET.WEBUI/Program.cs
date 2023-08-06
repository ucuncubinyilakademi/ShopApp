using ETICARET.BLL.Abstract;
using ETICARET.BLL.Concrete;
using ETICARET.DAL.Abstract;
using ETICARET.DAL.Concrete.EFCore;
using ETICARET.DAL.Concrete.Memory;
using ETICARET.WEBUI.Identity;
using ETICARET.WEBUI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();


builder.Services.Configure<IdentityOptions>(options =>
{
    //password
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;


    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

    //user
    options.User.RequireUniqueEmail = true;

    //SignIn
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = true;

});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".ShopApp.Security.Cookie",
        SameSite=SameSiteMode.Strict
    };
});

// Dependency Injection
builder.Services.AddScoped<IProductDal, EfCoreProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICommentDal, EfCoreCommentDal>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<ICartDal, EfCoreCartDal>();
builder.Services.AddScoped<ICartService, CartManager>();

//MVC Mimarisini Tanýmladým.
builder.Services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

SeedDatabase.Seed();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.CustomStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
    endpoints.MapControllerRoute(
        name: "products",
        pattern: "products/{category?}",
        defaults: new { controller = "Shop", action = "List" });


    endpoints.MapControllerRoute(
        name: "adminProducts",
        pattern: "admin/products",
        defaults: new { controller = "Admin", action = "ProductList" });

    endpoints.MapControllerRoute(
      name: "adminProducts",
      pattern: "admin/products/{id?}",
      defaults: new { controller = "Admin", action = "EditProduct" });

    endpoints.MapControllerRoute(
       name: "adminCategories",
       pattern: "admin/categories",
       defaults: new { controller = "Admin", action = "CategoryList" });

    endpoints.MapControllerRoute(
      name: "adminCategories",
      pattern: "admin/categories/{id?}",
      defaults: new { controller = "Admin", action = "EditCategory" });

    endpoints.MapControllerRoute(
     name: "cart",
     pattern: "cart",
     defaults: new { controller = "Cart", action = "Index" });
});

SeedIdentity.Seed(userManager, roleManager, app.Configuration).Wait();

app.Run();
