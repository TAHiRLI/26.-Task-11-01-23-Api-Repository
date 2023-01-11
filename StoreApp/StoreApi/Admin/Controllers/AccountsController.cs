using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Entities;
using StoreApi.Admin.Dtos.AdminDtos;
using StoreApi.Services.Implementations;
using StoreApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StoreApi.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public AccountsController(
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IJwtService jwtService)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this._jwtService = jwtService;
        }

        //[HttpGet("{roles}")]
        //public async Task<IActionResult>  Create()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    return Ok();
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null || !user.IsAdmin)
                return BadRequest();

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return BadRequest();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new {token = _jwtService.GenerateJwtToken(user,roles, _configuration )});

        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(new { username = User.Identity.Name });
        }

        //[HttpPost("createAdmin")]
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser
        //    {

        //        Fullname = "Samir Tahirli",
        //        UserName = "SamirAdmin",
        //        Email = "Samir@gmail.com",
        //        IsAdmin = true,
               
        //    };
        //   await _userManager.CreateAsync(admin,"admin123");
        //   await _userManager.AddToRoleAsync(admin, "Admin");

        //    AppUser SuperAdmin = new AppUser
        //    {
        //        Fullname = "Tahir Tahirli",
        //        UserName = "TahirAdmin",
        //        Email = "Tahir@gmail.com",
        //        IsAdmin = true,

        //    };
        //    await _userManager.CreateAsync(SuperAdmin, "admin123");
        //    await _userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");


        //    return Ok();
        //}
    }
}
