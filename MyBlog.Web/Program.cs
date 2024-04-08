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

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default settings password requirements
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6; // at least 6 chars
    options.Password.RequiredUniqueChars = 1; 
});





// Inject BloggieDbContext to ITagInterface & TagResponsitory
builder.Services.AddScoped<ITagRepository, TagRepository>();
// Inject BloggieDbContext to IBlo  gPostInterface & BlogPostResponsitory
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
