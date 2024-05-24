using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace Clinic_WebApp.Pages;

public class UpdateSchedule : PageModel
{
    private AuthorizationService _authorizationService;
    private PatientStorageService _patientStorageService;
    private ScheduleService _scheduleService;

    public UpdateSchedule(
        AuthorizationService authorizationService,
        PatientStorageService patientStorageService,
        ScheduleService scheduleService)
    {
        _authorizationService = authorizationService;
        _patientStorageService = patientStorageService;
        _scheduleService = scheduleService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        Doctor = doc;
        Patients = await _patientStorageService.GetPatientsByDoctorIdAsync(doc._id);
        Schedule = await _scheduleService.GetScheduleByDoctorIdAsync(doc._id.ToString());
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        Doctor = doc;
        Patients = await _patientStorageService.GetPatientsByDoctorIdAsync(doc._id);
        Schedule = await _scheduleService.GetScheduleByDoctorIdAsync(doc._id.ToString());
        
        await _scheduleService.UpdateScheduleAsync(new Schedule()
        {
            _id = ObjectId.GenerateNewId(),
            refToDoctor = doc._id,
            schedule = Input.ToArray()
        });
        
        Response.Redirect("/SchedulePage");
    }
    public Doctor Doctor { get; set; }
    public List<Patient> Patients { get; set; }
    public Schedule Schedule { get; set; }
    [BindProperty] public string[] Input { get; set; }
}
