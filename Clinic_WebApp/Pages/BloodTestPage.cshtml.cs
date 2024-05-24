using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class BloodTestPage : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly AnalyzesStorageService _analyzesStorageService;

    public BloodTestPage(
        AuthorizationService authorizationService,
        AnalyzesStorageService analyzesStorageService)
    {
        _authorizationService = authorizationService;
        _analyzesStorageService = analyzesStorageService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        BloodT = await _analyzesStorageService.GetBloodTestById(id);
    }
    [BindProperty(SupportsGet = true)] public string id { get; set; }
    public BloodTest BloodT { get; set; }
}