
using Ccms.Entities.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using Server.Services.Jwt;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        public OAuthController(IJwtService jwtService)
        {
            JwtService = jwtService;
        }

        private IJwtService JwtService { get; }
        [HttpGet]
        public IActionResult Authorize(
            string response_type, // authorization flow type 
            string client_id, // client id
            string redirect_uri,
            string scope, // what info I want = email,grandma,tel
            string state) // random string generated to confirm that we are going to back to the same client
        {
            // ?a=foo&b=bar
            var query = new QueryBuilder
            {
                { "redirectUri", redirect_uri },
                { "state", state }
            };

            return View(model: query.ToString());
        }

        [HttpPost]
        public IActionResult Authorize(string username, string redirectUri, string state)
        {



            const string code = "BABAABABABA";

            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);


            return Redirect($"{redirectUri}{query.ToString()}");
        }

        public async Task<IActionResult> Token(
            string grant_type, // flow of access_token request
            string code, // confirmation of the authentication process
            string redirect_uri,
            string client_id,
            string refresh_token)
        {
            // some mechanism for validating the code

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "someid"),
                new Claim("role1", "Admin"),
                new Claim("role2", "User"),
                new Claim("role3", "owner"),
                new Claim("granny", "cookie")
            };
            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Constants.Secret)),
                SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: grant_type == "refresh_token"
                    ? DateTime.Now.AddMinutes(5)
                    : DateTime.Now.AddMilliseconds(1),
                signingCredentials);



            var access_token = JwtService.Generate(new User()
            {
                FullName = "Massoud",
                PersonId = 1,
                UserId = 1
            },
            new Role()
            {
                RoleCaption = "سوپر یوزر",
                RoleId = 1,
                RoleName = "Admin"
            });

            var responseObject = new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(access_token),
                token_type = "Bearer",
                raw_claim = "oauthTutorial",
                refresh_token = "RefreshTokenSampleValueSomething77"
            };
            var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseObject));

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);
        }

        [Authorize]
        public IActionResult Validate()
        {
            if (HttpContext.Request.Query.TryGetValue("access_token", out var accessToken))
            {

                return Ok();
            }
            return BadRequest();
        }
    }
}
