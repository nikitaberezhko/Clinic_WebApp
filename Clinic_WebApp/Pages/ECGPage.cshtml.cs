using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class ECGPage : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly AnalyzesStorageService _analyzesStorageService;

    public ECGPage(
        AuthorizationService authorizationService,
        AnalyzesStorageService analyzesStorageService)
    {
        _authorizationService = authorizationService;
        _analyzesStorageService = analyzesStorageService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
        
        EcgAnalysis = await _analyzesStorageService.GetEcgById(id);
        if (EcgAnalysis == null)
        {
            EcgAnalysis.description = "";
            EcgAnalysis.urlToScan = "";
        }
    }
    [BindProperty(SupportsGet = true)] public string id { get; set; }
    public ECG? EcgAnalysis { get; set; }
}