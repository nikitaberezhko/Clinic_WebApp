using System.ComponentModel.DataAnnotations;
using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;

namespace Clinic_WebApp.Pages;

public class CreatePatient : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly PatientStorageService _patientStorageService;

    public CreatePatient(AuthorizationService authorizationService, PatientStorageService patientStorageService)
    {
        _authorizationService = authorizationService;
        _patientStorageService = patientStorageService;
    }

    public void OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        Doctor = doc;
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        Doctor = doc;

        if (ModelState.IsValid)
        {
            Patient newPatient = new() {
                name = Input.Name,
                _id = ObjectId.GenerateNewId(),
                refToDoctor = doc._id,
                age = Input.Age,
                gender = Input.Gender,
                inArchive = false,
                policy = Input.Policy,
                phone = Input.Phone
            };
            
            await _patientStorageService.CreatePatientAsync(newPatient);
            Response.Redirect("/PatientsList");
        }
    }

    public Doctor Doctor { get; set; }
    [BindProperty] public CreatePatientInput Input { get; set; }
}

public class CreatePatientInput
{
    [Required] public string Name { get; set; }
    [Required] public string Gender { get; set; }
    [Required] public string Policy { get; set; }
    [Required] public int Age { get; set; }
    [Required] public string Phone { get; set; }
}
