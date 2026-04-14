using EnemPrep.EntityModels.DTOS;
using EnemPrep.Server.Authorization;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Server.Controllers;

[Route("/api/[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Login([FromForm] LoginUserRequest request)
    {
        AuthResponse response = await _authService.LoginAsync(request);

        if (!response.Success)
            return BadRequest(response);
        
        return Ok(response);
    }

    [HasPermission(Permission.AccessQuestions)]
    [HttpPost]
    [Route("[action]")]
    public IActionResult Logout()
    {
        AuthResponse response = _authService.Logout();

        if (!response.Success) 
            return BadRequest(response);
        
        return RedirectToAction("Login", "Auth");
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register([FromForm] CreateUserRequest request)
    {
        AuthResponse response = await _authService.RegisterAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return StatusCode(201, response);
    }
}