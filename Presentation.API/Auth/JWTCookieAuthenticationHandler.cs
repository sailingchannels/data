using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Presentation.API.Auth
{
    public class JWTCookieAuthenticationHandler
        : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILogger<JWTCookieAuthenticationHandler> _logger;

        /// <summary>
        /// Instantiates an authentication handler the can either use the device GUID, an API token or a JWT
        /// to authenticate requests to the API
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public JWTCookieAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger<JWTCookieAuthenticationHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // read JWT cookie value
            string jwtRawValue = Request.Cookies["token"];
            if (string.IsNullOrWhiteSpace(jwtRawValue))
            {
                _logger.LogWarning("JWT cookie is empty");
                return AuthenticateResult.NoResult();
            }

            // decode JWT
            var payload = Jose.JWT.Decode<JWTPayload>(
                jwtRawValue,
                Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")),
                Jose.JwsAlgorithm.HS256
            );

            // validate the shape of the payload
            if (payload == null)
            {
                _logger.LogWarning("JWT payload is empty");
                return null;
            }

            // prepare identity as an authenticationticket for customer
            var ticket = CustomerAuthenticationTicket.Generate(Scheme, payload);

            return AuthenticateResult.Success(ticket);
        }
    }
}
