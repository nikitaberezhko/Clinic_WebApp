using Clinic_WebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace Clinic_WebApp.Pages;

public class CreateReferral : PageModel
{
    private readonly AuthorizationService _authorizationService;
    private readonly ReferralService _referralService;
    

    public CreateReferral(AuthorizationService authorizationService, ReferralService referralService)
    {
        _authorizationService = authorizationService;
        _referralService = referralService;
        _analyzes = new List<SelectListItem>
        {
            new SelectListItem{ Value = "Анализ крови", Text = "Анализ крови" },
            new SelectListItem{ Value = "ЭКГ", Text = "ЭКГ" }
        };
    }
    
    public void OnGet()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
    }

    public async Task OnPost()
    {
        _authorizationService.Authorization(HttpContext, out Doctor doc);
        
        await _referralService.CreateReferralByPatientIdAsync(new Referral()
        {
            _id = ObjectId.GenerateNewId(),
            AnalysisType = Input.analysisName,
            dateTime = Input.date + " " + Input.time,
            refToPatient = ObjectId.Parse(id)
        });
        
        var redirectLocation = "/MedicalCard?id=" + id;
        Response.Redirect(redirectLocation);
    }
    public IEnumerable<SelectListItem> _analyzes { get; set; }
    [BindProperty(SupportsGet = true)] public string id { get; set; }
    [BindProperty] public InputReferral Input { get; set; }
}

public class InputReferral
{
    public string analysisName { get; set; }
    public string date { get; set; }
    public string time { get; set; }
}