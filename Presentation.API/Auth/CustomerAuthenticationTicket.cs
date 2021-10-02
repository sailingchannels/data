using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Presentation.API.Auth
{
    public class CustomerAuthenticationTicket
    {
        public static AuthenticationTicket Generate(AuthenticationScheme scheme, JWTPayload payload)
        {
            // prepare identity via claimsprincipal
            var claims = new[] {
                new Claim(ClaimTypes.UserId, payload._id)
            };

            var identity = new ClaimsIdentity(claims, scheme.Name);
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationTicket(principal, scheme.Name);
        }
    }
}
