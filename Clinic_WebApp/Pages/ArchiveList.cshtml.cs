using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class ArchiveList : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly PatientStorageService _patientStorageService;

    public ArchiveList(AuthorizationService authorizationService, PatientStorageService patientStorageService)
    {
        _authorizationService = authorizationService;
        _patientStorageService = patientStorageService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doctor);
        
        ArchivePatients = await _patientStorageService.GetArchivePatientsAsync();
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doctor);
        
        if (ModelState.IsValid)
        {
            ArchivePatients = await _patientStorageService.GetArchivePatientsAsync(SearchName);
        }
    }

    [BindProperty] public string SearchName{ get; set; } = "";
    public List<Patient>? ArchivePatients { get; set; }
}