using System.Collections.Generic;
using System.Threading.Tasks;
using Clinic_WebApp.Model;
using JetBrains.Annotations;
using MongoDB.Bson;
using Xunit;

namespace Clinic_WebApp.Tests.Model;

[TestSubject(typeof(DiagnosisService))]
public class DiagnosisServiceTest
{
    private DiagnosisService _service = new DiagnosisService(
        new ClientMongoService(new MongoSettings()
        {
            Host = "mongodb://localhost:27017",
            Database = "ClinicWebApp"
        }));

    
    /// <summary>
    /// Тест метода получения диагноза по id пациента.
    /// </summary>
    [Fact]
    public async Task GetDiagnosisByPatientIdTest_ShouldReturnEquivalentDiagnosis()
    {
        // Arrange
        Diagnosis test = new Diagnosis() {
            _id = ObjectId.Parse("65fbf4478236d43230a5a8cb"),
            refToPatient = ObjectId.Parse("65cb3018a267f20d8182d840"),
            disease = "Аритмия",
            text = "Текст диагноза из БД"
        };
    
        // Act
        Diagnosis diagnosis = await _service.GetDiagnosisByPatientIdAsync("65cb3018a267f20d8182d840");
    
        // Assert
        Assert.Equivalent(test, diagnosis);
    }
    
    
    /// <summary>
    /// Тест метода получения коллекции направлений по id пацинета.
    /// </summary>
    // [Fact]
    // public async Task GetReferralsByPatientIdTest()
    // {
    //     // Arrange
    //     List<Referral> test = new List<Referral>() {
    //         new Referral()
    //         {
    //             _id = ObjectId.Parse("65cb6404ebcc1e045858027a"),
    //             refToPatient = ObjectId.Parse("65cb3018a267f20d8182d840"),
    //             AnalysisType = "Анализ крови",
    //             dateTime = "20.02.2024 10:00"
    //         }
    //     };
    //
    //     // Act
    //     List<Referral> referrals = await _service.GetReferralsByPatientIdAsync("65cb3018a267f20d8182d840");
    //
    //     // Assert
    //     Assert.Equivalent(test, referrals);
    //
    // }
}