using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Web.Data;
using MyBlog.Web.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); // allow to use Contorller with Views

// Inject DbConext => (telling which server we're using)
builder.Services.AddDbContext<BloggieDbContext>(options =>
    // paste connection string name inside GetConnectionString()
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

// Authentication DbContext
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

// Inject BloggieDbContext to ITagInterface & TagResponsitory
builder.Services.AddScoped<ITagRepository, TagRepository>();
// Inject BloggieDbContext to IBlogPostInterface & BlogPostResponsitory
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
// handle dependency injection
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();


var app = builder.Build();

// this is the middleware already been added to ours project
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see     .
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Allow to use Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    // controler = 'AdminTags' (AdminTagsController) and action = 'List' (List method in AdminTagsController)
    pattern: "{controller=Home}/{action=Index}/{id?}");
    
app.Run();
