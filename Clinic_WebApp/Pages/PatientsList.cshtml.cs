using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class PatientsList : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly PatientStorageService _patientStorageService;

    public PatientsList(AuthorizationService authorizationService, PatientStorageService patientStorageService)
    {
        _authorizationService = authorizationService;
        _patientStorageService = patientStorageService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor? doc);
        
        Doctor = doc;
        Patients = await _patientStorageService.GetPatientsByDoctorIdAsync(doc._id);
    }
    public Doctor? Doctor { get; set; } 
    public List<Patient>? Patients { get; set; }
}