using System.ComponentModel.DataAnnotations;
using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;

namespace Clinic_WebApp.Pages;

public class UpdatePatient : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly PatientStorageService _patientStorageService;

    public UpdatePatient(AuthorizationService authorizationService, PatientStorageService patientStorageService)
    {
        _authorizationService = authorizationService;
        _patientStorageService = patientStorageService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        Patient = await _patientStorageService.GetPatientByIdAsync(id);
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        Patient = await _patientStorageService.GetPatientByIdAsync(id);
        
        await _patientStorageService.UpdatePatientAsync(new Patient() {
            refToDoctor = Patient.refToDoctor,
            _id = ObjectId.Parse(id),
            age = Input.Age,
            gender = Input.Gender,
            inArchive = false,
            name = Input.Name,
            policy = Input.Policy,
            phone = Input.Phone
        });
        
        Response.Redirect("/PatientsList");
    }
    public Patient Patient { get; set; }
    [BindProperty] public UpdatePatientInput Input { get; set; }
    [BindProperty(SupportsGet = true)] public string id { get; set; }
}

public class UpdatePatientInput
{
    [Required] public string Name { get; set; }
    [Required] public string Gender { get; set; }
    [Required] public string Policy { get; set; }
    [Required] public int Age { get; set; }
    [Required] public string Phone { get; set; }
}