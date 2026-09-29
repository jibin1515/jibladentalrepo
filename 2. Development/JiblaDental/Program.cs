using Application;
using Identity;
using Infrastructure;
using Persistence;
using Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigureIdentityServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Configuration);


builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/account/login");

builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
builder.Services.AddHsts(options => options.MaxAge = TimeSpan.FromDays(365));

builder.Services.AddControllersWithViews(options => { options.EnableEndpointRouting = false; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithReExecute("/handle-error/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    //app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithReExecute("/handle-error/{0}");
    await DbInitializer.Seed(app);
}

app.Use(async (context, next) =>
{
    context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin-allow-popups";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    await next();
});

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Assets referenced with ?v=<hash> (asp-append-version) are safe to cache for a year;
        // everything else (images, fonts, uploads) is cached for a week.
        var versioned = ctx.Context.Request.Query.ContainsKey("v");
        ctx.Context.Response.Headers["Cache-Control"] = versioned
            ? "public,max-age=31536000,immutable"
            : "public,max-age=604800";
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "area",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
