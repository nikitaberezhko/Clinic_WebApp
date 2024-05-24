using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic_WebApp.Pages;

public class DeleteReferral : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly ReferralService _referralService;

    public DeleteReferral(
        AuthorizationService authorizationService,
        ReferralService referralService)
    {
        _authorizationService = authorizationService;
        _referralService = referralService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
        
        var referral = await _referralService.GetReferralByReferralIdAsync(id);
        refToPatient = referral.refToPatient.ToString();
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
        
        var referral = await _referralService.GetReferralByReferralIdAsync(id);
        
        await _referralService.DeleteReferralByIdAsync(id);

        var locationString = "/MedicalCard?id=" + referral.refToPatient.ToString();
        Response.Redirect(locationString);
    }
    [BindProperty(SupportsGet = true)] public string id { get; set; }
    public string refToPatient { get; set; }
}