using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using StainedGlass.Web.Enum;
using StainedGlass.Web.Models;

namespace StainedGlass.Web.Controllers.Account;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly IConfiguration Configuration;

    public AccountController(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    [HttpGet("Login")]
    public IActionResult Login(Error error, string returnUrl = "/Admin/Index")
    {
        ViewBag.Error = error switch
        {
            Error.NotAuthorized => "You need to be authenticated.",
            Error.TokenExpired => "The token has been expired, please try again.",
            _ => null
        };
        
        ViewBag.ReturnUrl = returnUrl;
        
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (
                model.Username == Configuration["User:Username"] 
                && VerifyPasswordHash(model.Password)
                )
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, "Admin") // Assign role as needed
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: Configuration["Jwt:Issuer"],
                    audience: Configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Use 'true' in production for HTTPS
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("jwtToken", tokenString, cookieOptions);
                
                return Redirect(HttpUtility.UrlDecode(model.ReturnUrl));

            }
            
            ModelState.AddModelError("Username", "Invalid login attempt.");
        }

        return View(model);
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwtToken");
        return Redirect("/Account/Login");
    }
    
    public string GenerateSalt()
    {
        // Generate a cryptographic salt
        var saltBytes = new byte[Convert.ToInt16(Configuration["User:Password:SaltByte"])];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }

        // Convert the salt to a Base64 string
        var saltString = Convert.ToBase64String(saltBytes);
        return saltString;
    }
    
    public (string Hash, string Salt) CreatePasswordHash(string password)
    {
        // Generate a salt and convert it to a Base64 string
        var saltString = Configuration["User:Password:Salt"];
        var saltBytes = Convert.FromBase64String(saltString);

        // Hash the password with the salt
        using (var hmac = new HMACSHA512(saltBytes))
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = hmac.ComputeHash(passwordBytes);
            var hashString = Convert.ToBase64String(hashBytes);
            return (Hash: hashString, Salt: saltString);
        }
    }

    public bool VerifyPasswordHash(string password)
    {
        // Convert the stored salt back to a byte array
        var saltBytes = Convert.FromBase64String(Configuration["User:Password:Salt"]);

        // Hash the input password with the stored salt
        using (var hmac = new HMACSHA512(saltBytes))
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var computedHashBytes = hmac.ComputeHash(passwordBytes);
            var computedHashString = Convert.ToBase64String(computedHashBytes);
            return computedHashString == Configuration["User:Password:Hash"];
        }
    }

}