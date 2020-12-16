using Microsoft.AspNetCore.Http;  
using Microsoft.AspNetCore.Identity;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.Extensions.Configuration;  
using Microsoft.IdentityModel.Tokens;  
using System;  
using System.Collections.Generic;  
using System.IdentityModel.Tokens.Jwt;  
using System.Security.Claims;  
using System.Text;  
using System.Threading.Tasks;
using Movies.Models.Authentication;

namespace Movies.Controllers  
{  
    [Route("api/[controller]")]  
    [ApiController]  
    public class AuthController : ControllerBase  
    {  
        private readonly UserManager<ApplicationUser> _userManager;  
        private readonly RoleManager<IdentityRole> _roleManager;  
        private readonly IConfiguration _configuration;  
  
        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)  
        {  
            _userManager = userManager;  
            _roleManager = roleManager;  
            _configuration = configuration;  
        }  
  
        [HttpPost]  
        [Route("login")]  
        public async Task<IActionResult> Login([FromBody] LoginModel request)  
        {  
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Unauthorized();
            }

            var userRoles = await _userManager.GetRolesAsync(user);  
  
            var authClaims = new List<Claim>  
            {  
                new Claim(ClaimTypes.Name, user.UserName),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
            };  
  
            foreach (var userRole in userRoles)  
            {  
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));  
            }  
  
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));  
  
            var token = new JwtSecurityToken(  
                issuer: _configuration["JWT:ValidIssuer"],  
                audience: _configuration["JWT:ValidIssuer"],  
                expires: DateTime.Now.AddHours(3),  
                claims: authClaims,  
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)  
            );  
  
            return Ok(new  
            {  
                id = user.Id,
                token = new JwtSecurityTokenHandler().WriteToken(token),  
                expiration = token.ValidTo  
            });
        }  
  
        [HttpPost]  
        [Route("register")]  
        public async Task<IActionResult> Register([FromBody] RegisterModel request)  
        {  
            var userExists = await _userManager.FindByNameAsync(request.Username);  
            if (userExists != null)  
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });  
  
            ApplicationUser user = new ApplicationUser()  
            {  
                Email = request.Email,  
                SecurityStamp = Guid.NewGuid().ToString(),  
                UserName = request.Username  
            };  
            var result = await _userManager.CreateAsync(user, request.Password);  
            if (!result.Succeeded)  
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });  
  
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });  
        }  
  
        [HttpPost]  
        [Route("register-admin")]  
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel request)  
        {  
            var userExists = await _userManager.FindByNameAsync(request.Username);  
            if (userExists != null)  
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });  
  
            ApplicationUser user = new ApplicationUser()  
            {  
                Email = request.Email,  
                SecurityStamp = Guid.NewGuid().ToString(),  
                UserName = request.Username  
            };  
            var result = await _userManager.CreateAsync(user, request.Password);  
            if (!result.Succeeded)  
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });  
  
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))  
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));  
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))  
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));  
  
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))  
            {  
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);  
            }  
  
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });  
        }  
    }  
}  