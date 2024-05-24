using System.Collections.Generic;
using System.Threading.Tasks;
using Clinic_WebApp.Model;
using JetBrains.Annotations;
using MongoDB.Bson;
using Xunit;

namespace Clinic_WebApp.Tests.Model;

[TestSubject(typeof(PatientStorageService))]
public class PatientStorageServiceTest
{
    private PatientStorageService _service = new PatientStorageService(
        new ClientMongoService(new MongoSettings()
        {
            Host = "mongodb://localhost:27017",
            Database = "ClinicWebApp"
        }));
    
    
    // /// <summary>
    // /// Тест метода получения пациента по id.
    // /// Проверка что при неправильном id возвращается <c>null</c>.
    // /// </summary>
    // /// <param name="id">идентификатор пациента</param>
    // [Theory]
    // [InlineData("65cb3018a267f20d8182d840")]
    // [InlineData("65cf41d2d8297efb74373e62")]
    // public async Task GetPatientByIdTest_ShouldReturnEquivalentPatientAndNull(string id)
    // {
    //     // Arrange
    //     Patient exp1 = new Patient() {
    //         _id = ObjectId.Parse("65cb3018a267f20d8182d840"),
    //         refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
    //         age = 30,
    //         gender = "male",
    //         phone = "+79000000000",
    //         inArchive = false,
    //         name = "Пациент 1 для доктора 1",
    //         policy = "0000 0000 0000 0000"
    //     };
    //     Patient exp2 = new Patient() {
    //         _id = ObjectId.Parse("65cf41d2d8297efb74373e62"),
    //         refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
    //         age = 30,
    //         gender = "male",
    //         phone = "+79000000000",
    //         inArchive = false,
    //         name = "Пациент 3 для доктора 1",
    //         policy = "0000 0000 0000 0000"
    //     };
    //
    //     // Act
    //     Patient patient1 = await _service.GetPatientByIdAsync("65cb3018a267f20d8182d840");
    //     Patient patient2 = await _service.GetPatientByIdAsync("65cf41d2d8297efb74373e62");
    //     Patient patientNull = await _service.GetPatientByIdAsync("65cf41d2d8297efb74373e63");
    //
    //     // Assert
    //     Assert.Equivalent(exp1, patient1);
    //     Assert.Equivalent(exp2, patient2);
    //     Assert.Null(patientNull);
    // }
    
    /// <summary>
    /// Тест получения коллекции пациентов 1.
    /// InlineData - корректные данные, необходимо вернуть не <c>null</c>.
    /// Проверка, что возвращается не null.
    /// Проверка на соответсвие типу.
    /// </summary>
    [Theory]
    [InlineData("65cb2fa8a267f20d8182d83e")]
    [InlineData("65cf029faef54bee2fe8b4da")]
    public async Task GetPatientsTest_ShouldReturnNotNullAndNotEmpty(string id)
    {
        // Arrange
        ObjectId doctorId = ObjectId.Parse(id);

        // Act
        List<Patient> patients = await _service.GetPatientsByDoctorIdAsync(doctorId);

        // Assert
        Assert.NotNull(patients);
        Assert.IsType<List<Patient>>(patients);
        Assert.NotEmpty(patients);
    }

    
    // /// <summary>
    // /// Тест получения коллекции пациентов 2.
    // /// Сравнение с эквивалентами получаемой коллекции.
    // /// </summary>
    // [Fact]
    // public async Task GetPatientsTest_ShouldReturnEquivalentList()
    // {
    //     // Arrange
    //     List<Patient> exp1 = new List<Patient>() {
    //         new Patient()
    //         {
    //             _id = ObjectId.Parse("65cb3018a267f20d8182d840"),
    //             refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
    //             name = "Пациент 1 для доктора 1",
    //             policy = "0000 0000 0000 0000",
    //             phone = "+79000000000",
    //             gender = "male",
    //             age = 30,
    //             inArchive = false
    //         },
    //         new Patient()
    //         {
    //             _id = ObjectId.Parse("65cf41ccd8297efb74373e61"),
    //             refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
    //             name = "Пациент 2 для доктора 1",
    //             policy = "0000 0000 0000 0000",
    //             gender = "male",
    //             phone = "+79000000000",
    //             age = 30,
    //             inArchive = false
    //         },
    //         new Patient()
    //         {
    //             _id = ObjectId.Parse("65cf41d2d8297efb74373e62"),
    //             refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
    //             name = "Пациент 3 для доктора 1",
    //             policy = "0000 0000 0000 0000",
    //             gender = "male",
    //             phone = "+79000000000",
    //             age = 30,
    //             inArchive = false
    //         }
    //     };
    //     List<Patient> exp2 = new List<Patient>() {
    //         new Patient()
    //         {
    //             _id = ObjectId.Parse("65cf41ffd8297efb74373e67"),
    //             refToDoctor = ObjectId.Parse("65cf029faef54bee2fe8b4da"),
    //             name = "Пациент 1 для доктора 2",
    //             policy = "0000 0000 0000 0000",
    //             gender = "male",
    //             phone = "+79000000000",
    //             age = 30,
    //             inArchive = false
    //         },
    //         new Patient()
    //         {
    //             _id = ObjectId.Parse("65cf4206d8297efb74373e68"),
    //             refToDoctor = ObjectId.Parse("65cf029faef54bee2fe8b4da"),
    //             name = "Пациент 2 для доктора 2",
    //             policy = "0000 0000 0000 0000",
    //             gender = "male",
    //             phone = "+79000000000",
    //             age = 30,
    //             inArchive = false
    //         }
    //     };
    //
    //     // Act
    //     List<Patient> patients1 = await _service.GetPatientsByDoctorIdAsync(ObjectId.Parse("65cb2fa8a267f20d8182d83e"));
    //     List<Patient> patients2 = await _service.GetPatientsByDoctorIdAsync(ObjectId.Parse("65cf029faef54bee2fe8b4da"));
    //
    //     // Assert
    //     Assert.Equivalent(exp1, patients1);
    //     Assert.Equivalent(exp2, patients2);
    // }
    
    
    /// <summary>
    /// Тест получения пациентов из архива 1.
    /// Сравнение с эквивалентами получаемой коллекции.
    /// </summary>
    [Fact]
    public async Task GetArchivePatientsTest1_ShouldReturnEquivalentList()
    {
        // Arrange
        List<Patient> expected = new List<Patient>() {
            new Patient()
            {
                _id = ObjectId.Parse("65d31086162f902e16b86c59"),
                refToDoctor = ObjectId.Parse("65cf029faef54bee2fe8b4da"),
                name = "Пациент 1 для архива",
                policy = "0000 0000 0000 0000",
                phone = "+79000000000",
                gender = "male",
                age = 30,
                inArchive = true,
            },
            new Patient()
            {
                _id = ObjectId.Parse("65f031d6438c9910f83a3572"),
                refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
                name = "Пациент 2 для архива",
                policy = "0000 0000 0000 0000",
                phone = "+79000000000",
                gender = "male",
                age = 30,
                inArchive = true,
            }
        };

        // Act
        List<Patient> patientsFromAchive = await _service.GetArchivePatientsAsync();

        // Assert
        Assert.Equivalent(expected, patientsFromAchive);
    }

    
    /// <summary>
    /// Тест получения пациентов с параметром поиска по имени из архива 2.
    /// Сравнение с эквивалентами получаемой коллекции.
    /// </summary>
    [Theory]
    [InlineData("Пациент 2 для архива")]
    public async Task GetArchivePatientsTest2_ShouldReturnEquivalentList(string searchName)
    {
        // Arrange
        List<Patient> test = new List<Patient>() {
            new Patient()
            {
                _id = ObjectId.Parse("65f031d6438c9910f83a3572"),
                refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
                name = "Пациент 2 для архива",
                policy = "0000 0000 0000 0000",
                phone = "+79000000000",
                gender = "male",
                age = 30,
                inArchive = true,
            }
        };

        // Act
        List<Patient> patientsFromArchive = await _service.GetArchivePatientsAsync("Пациент 2 для архива");

        // Assert
        Assert.Equivalent(test, patientsFromArchive);
    }

    // /// <summary>
    // /// 
    // /// </summary>
    // [Fact]
    // public async Task ChangePatientArchiveStasus_ShouldReturnNull()
    // {
    //     // Arrange
    //     Patient expected = new Patient() {
    //         _id = ObjectId.Parse("65cf41d2d8297efb74373e62"),
    //         refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
    //         name = "Пациент 3 для доктора 1",
    //         policy = "0000 0000 0000 0000",
    //         gender = "male",
    //         phone = "+79000000000",
    //         age = 30,
    //         inArchive = true
    //     };
    //     
    //     // Act
    //     await _service.ChangePatientArchiveStatus(new Patient() {
    //         _id = ObjectId.Parse("65cf41d2d8297efb74373e62"),
    //         refToDoctor = ObjectId.Parse("65cb2fa8a267f20d8182d83e"),
    //         name = "Пациент 3 для доктора 1",
    //         policy = "0000 0000 0000 0000",
    //         gender = "male",
    //         phone = "+79000000000",
    //         age = 30,
    //         inArchive = false
    //     });
    //     Patient test = await _service.GetPatientByIdAsync(expected._id.ToString());
    //
    //     // Assert
    //     Assert.Equivalent(expected, test);
    // }
}