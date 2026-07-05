using Library.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityDemoController : ControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityDemoController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    #region aspNetUsers
    [HttpPost("Users")]
    public async Task<IActionResult> CreateUser()
    {
        var user = new ApplicationUser
        {
            UserName="Ali",
            Email="Ali@gmail.com",
            FullName="Dash Ali"
        };


        var result = await _userManager.CreateAsync(user, "Ali12345#2%@");
        return Ok(result);
    }

    [HttpGet("Users")]
    public async Task<IActionResult> GetAllUser()
    {
        var result = await _userManager.Users.ToListAsync();
        return Ok(result);  
    }



    [HttpGet("Users/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _userManager.FindByIdAsync(id);
        return Ok(result);
    }


    [HttpGet("Users/by-Email")]
    public async Task<IActionResult> GetByIdEmail(string email)
    {
        var result = await _userManager.FindByEmailAsync(email);
        return Ok(result);
    }


    [HttpDelete("Users/{id}")]
    public async Task<IActionResult> DeleteById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        var result = await _userManager.DeleteAsync(user);
        
        return Ok(result);
    }

    [HttpPost("Users/Check-Password")]
    public async Task<IActionResult> CheckPassword(string userNameOrPhoneNumberOrEmail= "Ali@gmail.com", string password= "Ali12345#2%@")
    {
        var user = await _userManager.Users.Where(x=>x.Email == userNameOrPhoneNumberOrEmail 
                || x.PhoneNumber == userNameOrPhoneNumberOrEmail
                || x.UserName ==  userNameOrPhoneNumberOrEmail).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();


        var result = await _userManager.CheckPasswordAsync(user,password );
        return Ok(result);
    }


    [HttpPost("Users/change-Password")]
    public async Task<IActionResult> ChangePassword(string userNameOrPhoneNumberOrEmail = "Ali@gmail.com", string oldpassword = "Ali12345#2%@", string newpassword = "Ali12345#2%@asdasdas")
    {
        var user = await _userManager.Users.Where(x => x.Email == userNameOrPhoneNumberOrEmail
                || x.PhoneNumber == userNameOrPhoneNumberOrEmail
                || x.UserName ==  userNameOrPhoneNumberOrEmail).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();


        var result = await _userManager.ChangePasswordAsync(user,oldpassword , newpassword);
        return Ok(result);
    }

    [HttpPost("Users/reset-Password")]
    public async Task<IActionResult> ResetPassword(string userNameOrPhoneNumberOrEmail = "Ali@gmail.com")
    {
        var user = await _userManager.Users.Where(x => x.Email == userNameOrPhoneNumberOrEmail
                || x.PhoneNumber == userNameOrPhoneNumberOrEmail
                || x.UserName ==  userNameOrPhoneNumberOrEmail).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();


        var result = await _userManager.GeneratePasswordResetTokenAsync(user);
        return Ok(result);
    }


    [HttpPost("Users/email-confirm")]
    public async Task<IActionResult> ConfirmEmail(string userNameOrPhoneNumberOrEmail = "Ali@gmail.com")
    {
        var user = await _userManager.Users.Where(x => x.Email == userNameOrPhoneNumberOrEmail
                || x.PhoneNumber == userNameOrPhoneNumberOrEmail
                || x.UserName ==  userNameOrPhoneNumberOrEmail).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();


        var result = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return Ok(result);
    }


    [HttpPost("Users/Lock")]
    public async Task<IActionResult> LockUser(string userNameOrPhoneNumberOrEmail = "Ali@gmail.com")
    {
        var user = await _userManager.Users.Where(x => x.Email == userNameOrPhoneNumberOrEmail
                || x.PhoneNumber == userNameOrPhoneNumberOrEmail
                || x.UserName ==  userNameOrPhoneNumberOrEmail).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();


        var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
        //var result2 = await _userManager.SetLockoutEnabledAsync(user,true);
        //var result3 = await _userManager.SetLockoutEndDateAsync(user, null);
        return Ok(result);
    }


    #endregion



    #region Role
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllroles()
    {
        var result = await _roleManager.Roles.ToListAsync();
        return Ok(result);
    }


    [HttpGet("roles/{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        var result = await _roleManager.FindByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("role")]
    public async Task<IActionResult> CreateRole(string roleName = "Admin")
    {
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));


        return Ok(result);
    }

    [HttpPost("Users-role")]
    public async Task<IActionResult> AddUserToRole(string userNameOrPhoneNumberOrEmail = "Ali@gmail.com" , string roleName= "Admin")
    {
        var user = await _userManager.Users.Where(x => x.Email == userNameOrPhoneNumberOrEmail
                || x.PhoneNumber == userNameOrPhoneNumberOrEmail
                || x.UserName ==  userNameOrPhoneNumberOrEmail).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

         var roleExists =await _roleManager.RoleExistsAsync(roleName);

        if(!roleExists)
        {
            return NotFound("Role Nadarim ");
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        return Ok(result);
    }


    [HttpDelete("Users-role")]
    public async Task<IActionResult> DeletUserToRole(string userNameOrPhoneNumberOrEmail = "Ali@gmail.com", string roleName = "Admin")
    {
        var user = await _userManager.Users.Where(x => x.Email == userNameOrPhoneNumberOrEmail
                || x.PhoneNumber == userNameOrPhoneNumberOrEmail
                || x.UserName ==  userNameOrPhoneNumberOrEmail).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            return NotFound("Role Nadarim ");
        }


        var roles = await _userManager.GetRolesAsync(user);
 
        var result = await _userManager.RemoveFromRoleAsync(user, roleName);

        return Ok(result);
    }

    [HttpGet("user-roles")]
    public async Task<IActionResult> GetUserroles(string userNameOrPhoneNumberOrEmail = "Ali@gmail.com")
    {
        var user = await _userManager.Users.Where(x => x.Email == userNameOrPhoneNumberOrEmail
               || x.PhoneNumber == userNameOrPhoneNumberOrEmail
               || x.UserName ==  userNameOrPhoneNumberOrEmail).FirstOrDefaultAsync();

        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }


    #endregion

}

