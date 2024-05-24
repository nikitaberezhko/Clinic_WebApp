using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class SchedulePage : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly PatientStorageService _patientStorageService;
    private readonly ScheduleService _scheduleService;

    public SchedulePage(
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
        
        Schedule = await _scheduleService.GetScheduleByDoctorIdAsync(doc._id.ToString());

        if (Schedule == null)
        {
            _scheduleService.CreateScheduleAsync(doc._id.ToString());
            Schedule = await _scheduleService.GetScheduleByDoctorIdAsync(doc._id.ToString());
        }
    }
    public Schedule Schedule { get; set; }
}
