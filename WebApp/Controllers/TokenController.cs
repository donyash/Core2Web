using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApp.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    //[Route("v1/[controller]")]
    //[Route("[controller]/[action]")]
    [AllowAnonymous]
    public class TokenController : Controller
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration configuration)
        {
            _config = configuration;
        }
        [HttpPost]
        [Route("create")]   //v1/api/token/create
        public IActionResult Create([FromBody]LoginViewModel model)
        {
            //if (model.Email == "donyash@hotmail.com" && model.Password == "Password1!!")
            //{
            //    return new ObjectResult(GenerateToken(model.Email));
            //}
            //return BadRequest();

        
            //return new ObjectResult(GenerateToken("donyash@hotmail.com"));

            //var token = GenerateToken("donyash@hotmail.com");
            var token = GenerateTokenString("donyash@hotmail.com");

            var tokenUser = new TokenUser { access_token = token, userName = "donyash@hotmail.com" };
            return new ObjectResult(tokenUser);
        }

        private string GenerateToken(string email)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecreteforauth"));
            SigningCredentials signingCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtHeader jwtHeader = new JwtHeader(signingCredential);
            JwtPayload jwtPayload = new JwtPayload(claims);
            JwtSecurityToken token = new JwtSecurityToken(jwtHeader, jwtPayload);
            //TokenValidationParameters valid = new TokenValidationParameters()
            //{
            //    ValidIssuer = "meeeeee",
            //    ValidAudience = "meeeeee",
            //    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("thisisasecreteforauthasdfasdfasdf")),
            //    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
            //};

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateTokenString(string email)
        {
            var claims = new[]
                     {
                          new Claim(JwtRegisteredClaimNames.Sub, email),
                          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
              _config["Tokens:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

    public class TokenUser
    {
        public string access_token { get; set; }
        public string userName { get; set; }
    }
}