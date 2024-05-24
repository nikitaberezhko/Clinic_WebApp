using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;

namespace Clinic_WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly AnnouncementService _announcementService;

    public IndexModel(AuthorizationService authorizationService, AnnouncementService announcementService)
    {
        _authorizationService = authorizationService;
        _announcementService = announcementService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
        
        Doctor = doc;
        Announcement = await _announcementService.GetAnnouncementAsync();
    }
    public Doctor? Doctor { get; set; }
    public Announcement? Announcement { get; set; }
}