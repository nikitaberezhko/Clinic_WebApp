using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class MedicalCard : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly DiagnosisService _diagnosisService;
    private readonly PatientStorageService _patientStorageService;
    private readonly ReferralService _referralService;
    private readonly AnalyzesStorageService _analyzesStorageService;

    public MedicalCard(AuthorizationService authorizationService,
        DiagnosisService diagnosisService,
        PatientStorageService patientStorageService,
        ReferralService referralService,
        AnalyzesStorageService analyzesStorageService)
    {
        _authorizationService = authorizationService;
        _diagnosisService = diagnosisService;
        _patientStorageService = patientStorageService;
        _referralService = referralService;
        _analyzesStorageService = analyzesStorageService;
    }
    
    public async Task OnGet(string id)
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        Patient = await _patientStorageService.GetPatientByIdAsync(id);
        Diagnosis = await _diagnosisService.GetDiagnosisByPatientIdAsync(id);
        Referrals = await _referralService.GetReferralsByPatientIdAsync(id);
        Analyzes = await _analyzesStorageService.GetAnalysesListByPatientIdAsync(id);
    }

    public async Task OnPost(string id)
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
        
        Patient = await _patientStorageService.GetPatientByIdAsync(id);
        if (ModelState.IsValid)
        { 
            _diagnosisService.RefreshDiagnosisAsync(id, DiagnosisInput.disease, DiagnosisInput.text);
        }
        Diagnosis = await _diagnosisService.GetDiagnosisByPatientIdAsync(id);
        Referrals = await _referralService.GetReferralsByPatientIdAsync(id);
        Analyzes = await _analyzesStorageService.GetAnalysesListByPatientIdAsync(id);
    }
    [BindProperty] public DiagnosisInputModel DiagnosisInput { get; set; }
    public Patient? Patient { get; set; }
    public Diagnosis? Diagnosis { get; set; }
    public List<Referral>? Referrals { get; set; }
    public List<Analysis>? Analyzes { get; set; }
}

public class DiagnosisInputModel
{
    public string disease { get; set; }
        
    public string text { get; set; }
}