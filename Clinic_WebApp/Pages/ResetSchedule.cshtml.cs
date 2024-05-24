using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class ResetSchedule : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly ScheduleService _scheduleService;

    public ResetSchedule(
        AuthorizationService authorizationService,
        ScheduleService scheduleService)
    {
        _authorizationService = authorizationService;
        _scheduleService = scheduleService;
    }
    
    public void OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
        
        await _scheduleService.ResetScheduleAsync(id);
        
        Response.Redirect("/SchedulePage");
    }

    [BindProperty(SupportsGet = true)] public string id { get; set; }
}