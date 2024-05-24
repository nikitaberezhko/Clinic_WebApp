using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class ArchiveMedicalCard : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly PatientStorageService _patientStorageService;
    private readonly AnalyzesStorageService _analyzesStorageService;

    public ArchiveMedicalCard(
        AuthorizationService authorizationService, 
        PatientStorageService patientStorageService,
        AnalyzesStorageService analyzesStorageService)
    {
        _authorizationService = authorizationService;
        _patientStorageService = patientStorageService;
        _analyzesStorageService = analyzesStorageService;
    }
    
    public async Task OnGet(string id)
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
        
        Patient = await _patientStorageService.GetPatientByIdAsync(id);
        Analyzes = await _analyzesStorageService.GetAnalysesListByPatientIdAsync(id);
    }
    public Patient? Patient { get; set; }
    public List<Analysis> Analyzes { get; set; }
}