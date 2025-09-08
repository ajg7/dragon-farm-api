using DragonFarmApi.DTOs;
using DragonFarmApi.DTOs.Responses;
using DragonFarmApi.Models;
using DragonFarmApi.Orchestrators.Interfaces;
using DragonFarmApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DragonFarmApi.Controllers;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthOrchestrator _authOrchestrator;


    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtService jwtService,
        ILogger<AuthController> logger,
        IAuthOrchestrator authOrchestrator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _logger = logger;
        _authOrchestrator = authOrchestrator;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">Registration details</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists");
            }

            // Create new user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true // For simplicity, auto-confirm emails
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            // Assign default "User" role
            await _userManager.AddToRoleAsync(user, "User");

            // Generate JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtService.GenerateTokenAsync(user, roles);

            return Ok(new AuthResponse
            {
                Token = token,
                Expires = DateTime.UtcNow.AddDays(7),
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Roles = roles.ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, "An error occurred during registration");
        }
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
            {
                return Unauthorized("Invalid credentials");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid credentials");
            }

            // Generate JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtService.GenerateTokenAsync(user, roles);

            return Ok(new AuthResponse
            {
                Token = token,
                Expires = DateTime.UtcNow.AddDays(7),
                Email = user.Email ?? string.Empty,
                FullName = $"{user.FirstName} {user.LastName}",
                Roles = roles.ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return StatusCode(500, "An error occurred during login");
        }
    }

    /// <summary>
    /// Assign a role to a user (Admin only)
    /// </summary>
    /// <param name="request">Role assignment details</param>
    /// <returns>Success message</returns>
    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(AssignRoleRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
            {
                return BadRequest("Role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok($"Role '{request.RoleName}' assigned to user '{request.Email}' successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during role assignment");
            return StatusCode(500, "An error occurred during role assignment");
        }
    }

    /// <summary>
    /// Get current user information
    /// </summary>
    /// <returns>Current user details</returns>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<object>> GetCurrentUser()
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = $"{user.FirstName} {user.LastName}",
                Roles = roles,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return StatusCode(500, "An error occurred while retrieving user information");
        }
    }

    /// <summary>
    /// Get all roles (Admin only)
    /// </summary>
    /// <returns>List of all roles</returns>
    [HttpGet("roles")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GetRolesResponse>> GetRoles()
    {
        try
        {
            var roles = await _authOrchestrator.GetAllRolesAsync();

            var response = new GetRolesResponse
            {
                DragonFarmUserRoles = roles,
                TotalRoles = roles.Count,
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles");
            return StatusCode(500, "An error occurred while retrieving roles");
        }
    }
}