﻿using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZaylandShop.IdentityService.Controllers.Invariants;
using ZaylandShop.IdentityService.Controllers.ViewModels;
using ZaylandShop.IdentityService.Entities;
using ITokenService = ZaylandShop.IdentityService.Abstractions.ITokenService;

namespace ZaylandShop.IdentityService.Controllers.Auth.V1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly IIdentityServerInteractionService _interactiveService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(
        IIdentityServerInteractionService interactiveService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ITokenService tokenService)
    {
        _interactiveService = interactiveService;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpGet]
    [Produces("text/html")]
    public IActionResult Login(
        string response_type, 
        string client_id, 
        string redirect_uri, 
        string scope,
        string state)
    {
        var loginView = new LoginViewModel()
        {
            ReturnUrl = redirect_uri,
            State = state
        };
        
        return View(loginView);
    }
    
    [HttpPost("login")]
    [Produces("text/html")]
    public async Task<IActionResult> Login([FromForm] LoginViewModel loginView)
    {
        if (!ModelState.IsValid)
        {
            return View(loginView);
        }
    
        var user = await _userManager.FindByEmailAsync(loginView.UserName);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, AuthControllerInvariants.UserNotFound);
            return View(loginView);
        }
        
        var result = await _signInManager.PasswordSignInAsync(loginView.UserName, loginView.Password, false, false);
        if (result.Succeeded)
        {
            var token = await _tokenService.CreateTokenAsync(user);
            return Redirect(loginView.ReturnUrl + $"?code=fhvbsivkasvjabkasjvbkvsdjkvksad&state={loginView.State}&token={token}");   
        }
        ModelState.AddModelError(string.Empty, AuthControllerInvariants.InvalidPassword);
        
        return View(loginView);
    }
    
    [HttpGet("register")]
    [Produces("text/html")]
    public IActionResult Register(string returnUrl)
    {
        var registerView = new RegisterViewModel()
        {
            ReturnUrl = returnUrl
        };
        
        return View(registerView);
    }
    
    [HttpPost("register")]
    [Produces("text/html")]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerViewModel);
        }
    
        var user = new AppUser()
        {
            UserName = registerViewModel.UserName,
            Email = registerViewModel.UserName,
            EmailConfirmed = true
        };
        user.PasswordHash = new PasswordHasher<AppUser>()
            .HashPassword(user, registerViewModel.Password);
    
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return Redirect(registerViewModel.ReturnUrl);
        }
    
        ModelState.AddModelError(string.Empty, AuthControllerInvariants.RegistrationError);
        
        return View(registerViewModel);
    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        var logoutRequest = await _interactiveService.GetLogoutContextAsync(logoutId);
        return Redirect(logoutRequest.PostLogoutRedirectUri);
    }
}