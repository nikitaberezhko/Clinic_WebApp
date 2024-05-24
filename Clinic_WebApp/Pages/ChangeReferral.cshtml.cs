using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;

namespace Clinic_WebApp.Pages;

public class ChangeReferral : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly ReferralService _referralService;

    public ChangeReferral(
        AuthorizationService authorizationService,
        ReferralService referralService)
    {
        _authorizationService = authorizationService;
        _referralService = referralService;
    }
    
    public async Task OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        oldReferral = await _referralService.GetReferralByReferralIdAsync(id);
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);

        oldReferral = await _referralService.GetReferralByReferralIdAsync(id);
        
        _referralService.UpdateReferralAsync(new Referral()
        {
            _id = ObjectId.Parse(id),
            AnalysisType = Input.analysisName,
            dateTime = Input.date + " " + Input.time,
            refToPatient = oldReferral.refToPatient
        });
    }
    [BindProperty(SupportsGet = true)] public string id { get; set; }
    [BindProperty] public ReferralInput Input { get; set; }
    public Referral oldReferral { get; set; }
}

public class ReferralInput
{
    public string analysisName { get; set; }
    public string date { get; set; }
    public string time { get; set; }
}