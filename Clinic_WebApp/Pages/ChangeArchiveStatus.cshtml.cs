using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class ChangeArchiveStatus : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly PatientStorageService _patientStorageService;

    public ChangeArchiveStatus(AuthorizationService authorizationService, PatientStorageService patientStorageService)
    {
        _authorizationService = authorizationService;
        _patientStorageService = patientStorageService;
    }
    
    public void OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        Patient patient = await _patientStorageService.GetPatientByIdAsync(id);
        await _patientStorageService.ChangePatientArchiveStatus(patient);
        
        Response.Redirect("/PatientsList");
    }
    [BindProperty(SupportsGet = true)] public string id { get; set; }
}