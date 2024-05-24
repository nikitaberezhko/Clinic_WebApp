using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;

namespace Clinic_WebApp.Pages;

public class Authentication : PageModel
{
    private readonly AuthenticateService _authenticateService;

    public Authentication(AuthenticateService service) =>
        _authenticateService = service;
    
    [BindProperty] public InputModel Input { get; set; }
    
    
    public void OnGet() { }
    
    
    public async Task OnPost()
    {
        if (ModelState.IsValid)
        {
            bool authenticationConfirm = 
                await _authenticateService.AuthenticateAsync(HttpContext, Input.Login, Input.Password);
            if (authenticationConfirm)
            {
                Response.Redirect("/Index");   
            }
        }
    }
}

public class InputModel
{
    [Required]
    public string Login { get; set; }
    
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
}