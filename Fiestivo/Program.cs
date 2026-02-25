using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fiestivo.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Fiestivo.Core.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    .EnableSensitiveDataLogging());

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
    });
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

builder.Services.AddAuthorization();

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        // Page configurations
    })
    .AddMvcOptions(options =>
    {
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
            _ => "This field is required.");
    });

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

// API Endpoints
app.MapGet("/api/events/{id}", async (int id, ApplicationDbContext context) =>
{
    var eventItem = await context.Events
        .Include(e => e.Attends)
        .FirstOrDefaultAsync(e => e.Event_ID == id);

    if (eventItem == null) return Results.NotFound();

    return Results.Ok(new
    {
        eventItem.Event_ID,
        eventItem.Event_Title,
        attendees_Number = eventItem.Attendees_Number,
        currentAttendees = eventItem.Attends.Count,
        eventItem.Event_Date,
        eventItem.Event_time,
        eventItem.Event_Duration,
        eventItem.Event_Location,
        eventItem.Event_Location_Details,
        eventItem.Event_Discription,
        eventItem.IsPublic,
        eventItem.Category_ID,
        event_Picture = eventItem.Event_Picture != null ?
            Convert.ToBase64String(eventItem.Event_Picture) : null
    });
});

app.MapGet("/api/events/search", async (string term, ApplicationDbContext context) =>
{
    var events = await context.Events
        .Where(e => e.Event_Title.Contains(term) || e.Event_Discription.Contains(term))
        .Take(5)
        .Select(e => new
        {
            e.Event_ID,
            e.Event_Title,
            e.Event_Date,
            Category = e.Category.Category_Name
        })
        .ToListAsync();

    return Results.Ok(events);
});

app.MapGet("/api/events/{eventId}/guests", async (int eventId, ApplicationDbContext context) =>
{
    var guests = await context.Attends
        .Where(a => a.Event_ID == eventId)
        .Include(a => a.User)
        .Select(a => new
        {
            userId = a._User_ID,
            fullName = a.User.Full_Name,
            userName = a.User._User_Name
        })
        .ToListAsync();

    return Results.Ok(guests);
});

app.MapPost("/api/events/{eventId}/remove-guest/{userId}", async (int eventId, int userId, ApplicationDbContext context) =>
{
    var attendee = await context.Attends
        .FirstOrDefaultAsync(a => a.Event_ID == eventId && a._User_ID == userId);

    if (attendee != null)
    {
        context.Attends.Remove(attendee);
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound();
});

// Global error handling
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Request.Path = "/Error";
        await next();
    }
});

app.Run();