using Clinic_WebApp.Model;

namespace Clinic_WebApp;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        builder.Services.AddRazorPages();

        var settings = builder.Configuration.GetSection("MongoSettings").Get<MongoSettings>();
        MongoServiceCollection.Configure(builder.Services, settings);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}