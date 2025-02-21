using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using StainedGlass.Transfer;
using StainedGlass.Web.Enum;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//add services to the container
builder.Services.AddTransient<InputBoundary, UseCaseInteractor>();

// Add JWT authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        // Optional: Handle custom token validation events
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                
                if (context.Request.Cookies.ContainsKey("jwtToken"))
                {
                    context.Token = context.Request.Cookies["jwtToken"];
                }
                return Task.CompletedTask;
            },
            
            OnChallenge = context =>
            {
                var endpoint = context.HttpContext.GetEndpoint();
                var returnUrl = HttpUtility.UrlEncode(context.Request.Path + context.Request.QueryString);
                //rule just for pages, that require authorization
                var requiresAuthorization = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() != null;
                if (requiresAuthorization)
                {
                    var error = Error.NotAuthorized;
                    if (context.AuthenticateFailure?.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        error = Error.TokenExpired;
                    }
                    context.Response.Redirect($"/Account/Login?returnUrl={returnUrl}&error={error}");
                    
                    //cancelling all the further default logic
                    context.HandleResponse();
                }
                return Task.CompletedTask;
            },
            
            // OnAuthenticationFailed = context =>
            // {
            //     
            // },
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", 
        policy => policy.RequireClaim(ClaimTypes.Role, "Admin")
        );
});

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

app.UseAuthentication();
app.UseAuthorization();

//for controllers to work
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
}); 

app.MapRazorPages();

app.Run();
