using Bakery.Core.Entities;
using Bakery.Web.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Web.ApiControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Customer> _userManager;

        public AuthController(
            IConfiguration configuration,
            UserManager<Customer> userManager)
        {
            _config = configuration;
            _userManager = userManager;
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] CredentialDto credentials)
        {
            var authUser = await _userManager.FindByNameAsync(credentials.Username);
            if (authUser == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.CheckPasswordAsync(authUser, credentials.Password))
            {
                return Unauthorized();
            }

            var tokenString = GenerateJwtToken(authUser.UserName, await _userManager.GetRolesAsync(authUser));
            return Ok(new
            {
                auth_token = tokenString,
                name = authUser.FullName,
            });
        }


        /// <summary>
        /// JWT erzeugen. Minimale Claim-Infos: Email und Rolle
        /// </summary>
        /// <returns>Token mit Claims</returns>
        private string GenerateJwtToken(string email, IEnumerable<string> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            foreach (string role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              audience: _config["Jwt:Audience"],
              claims: authClaims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        /// <summary>
        /// Neuen Benutzer registrieren. 
        /// </summary>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register(UserDto newUser)
        {
            var user = await _userManager.FindByNameAsync(newUser.Username);

            // gibt es diesen Benutzer bereits?
            if (user != null)
            {
                return BadRequest(new { Status = "Error", Message = "User already exists!" });
            }

            var result = await _userManager.CreateAsync(new Customer()
            {
                UserName = newUser.Username,
                Firstname = newUser.Firstname,
                Lastname = newUser.Lastname,

            }, newUser.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { Status = "Error", Message = string.Join(", ", result.Errors.Select(error => error.Description)) });
            }

            return Ok();
        }
    }

}
