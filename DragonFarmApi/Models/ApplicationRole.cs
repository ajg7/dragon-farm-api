using Microsoft.AspNetCore.Identity;

namespace DragonFarmApi.Models;

/// <summary>
/// Represents a role in the Dragon Farm application
/// </summary>
public class ApplicationRole : IdentityRole
{
    /// <summary>
    /// Description of what this role provides access to
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// When this role was created
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}