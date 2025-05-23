using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using RobotControllerApi.Persistence;   
using RobotControllerApi.Services;

//handle basic authentication for the api by validating the USN and PW
//custom authentication handler 
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService; //use this interface to validate user by email
    //constructor with dependency injection
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, //diagnostic 
        UrlEncoder encoder,
        IUserService userService) //injected service to get user's data
        : base(options, logger, encoder)
    {
        _userService = userService;
    }

    //main authentication method 
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        //require authorization header, check if it exist 
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        try
        {
            //get header value which is the basic auth credentials 
            var authHeader = Request.Headers["Authorization"].ToString();
            //use basic auth format to decode 
            var authHeaderVal = authHeader.Substring("Basic ".Length).Trim();

            //decode Base64 string to get email:pw format
            var credentialBytes = Convert.FromBase64String(authHeaderVal);
            //converts the byte array to string, then splits it at the colon
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            //assign 
            var email = credentials[0];
            var password = credentials[1];

            //get user from the data source
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return AuthenticateResult.Fail("Invalid Email or Password");

            //verify password
            var hasher = new PasswordHasher<UserModel>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
                return AuthenticateResult.Fail("Invalid Email or Password");

            //add user claims
            var claims = new[]
            {   
                //standard claim
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                //role claim 
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            //create identity and principle 
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            //create authentication ticket 
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }
    }
}
