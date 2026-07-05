using Microsoft.AspNetCore.Identity;

namespace Library.Api.Entities;

/// <summary>
/// کاربر سیستم
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// نام و نام خانوادگی
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
