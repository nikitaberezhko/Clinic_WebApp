using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using MongoDB.Bson;
namespace Clinic_WebApp.Model;

public static class MongoServiceCollection
{
    public static IServiceCollection Configure(this IServiceCollection services, MongoSettings settings)
    {
        services.AddSingleton(new ClientMongoService(settings));
        services.AddScoped<AuthenticateService>();
        services.AddScoped<AuthorizationService>();
        services.AddSingleton<AnnouncementService>();
        services.AddScoped<PatientStorageService>();
        services.AddScoped<DiagnosisService>();
        services.AddScoped<ReferralService>();
        services.AddScoped<AnalyzesStorageService>();
        services.AddScoped<ScheduleService>();
        return services;
    }
}


public class MongoSettings
{
    public string Host { get; set; }
    public string Database { get; set; }
}


public class ClientMongoService
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    
    
    public IMongoDatabase Database { get => _database; }
    
    
    public ClientMongoService(MongoSettings settings)
    {
        _client = new MongoClient(settings.Host);
        _database = _client.GetDatabase(settings.Database);
    }
}


public class AuthenticateService
{
    private readonly IMongoDatabase _database;

    
    public AuthenticateService(ClientMongoService client)
    {
        _database = client.Database;
    }

    
    public async Task<bool> AuthenticateAsync(HttpContext context, string login, string password)
    {
        var doctors = _database.GetCollection<Doctor>("doctors");
        var filter = new BsonDocument{{"Login", login}, {"Password", password}};
        
        var doctorsList = doctors.Find(filter);
        
        if (doctorsList != null && doctorsList.FirstOrDefault() != null)
        {   
            var response = context.Response;
            response.Cookies.Append("Login", login);
            response.Cookies.Append("Password", password);
            response.Cookies.Append("id", doctorsList.FirstOrDefault()._id.ToString());
            return true;
        }
        return false;
    }
}


public class AuthorizationService
{
    private readonly IMongoDatabase _database;
    
    public AuthorizationService(ClientMongoService client)
    {
        _database = client.Database;
    }

    
    public void Authorization(HttpContext context, out Doctor? doctor)
    {
        bool access = AccessVerification(context, out Doctor? doc);
        doctor = doc;
        if (!access)
        {
            context.Response.Redirect("/Authentication");
        }
        
        bool AccessVerification(HttpContext context, out Doctor doctor)
        {
            doctor = new Doctor();
            var request = context.Request;
            string login;
            string password;
            ObjectId id;
        
            try
            {
                login = request.Cookies["Login"];
                password = request.Cookies["Password"];
                id = ObjectId.Parse(request.Cookies["id"]);
            }
            catch { return false; }
        
            var doctors = _database.GetCollection<Doctor>("doctors");
            var filter = new BsonDocument{{"Login", login}, {"Password", password}, {"_id", id}};
        
            IFindFluent<Doctor, Doctor> doc = doctors.Find(filter);
        
            if (doc != null && doc.FirstOrDefault() != null)
            {
                doctor = doc.FirstOrDefault();
                return true;
            }
            return false;
        }
    }
}


public class AnnouncementService
{
    private readonly IMongoDatabase _database;

    
    public AnnouncementService(ClientMongoService client)
    {
        _database = client.Database;
    }
    
    
    public async Task<Announcement> GetAnnouncementAsync()
    {
        IMongoCollection<Announcement> announcement = _database.GetCollection<Announcement>("announcement");
        return await announcement.Find(_ => true).FirstOrDefaultAsync();
    }
}


public class PatientStorageService
{
    private readonly IMongoDatabase _database;

    
    public PatientStorageService(ClientMongoService client)
    {
        _database = client.Database;
    }
    
    
    public async Task<List<Patient>> GetPatientsByDoctorIdAsync(ObjectId doctorId)
    {
        var patientsCollection = _database.GetCollection<Patient>("patients");
        var filter = new BsonDocument{{"refToDoctor", doctorId}, {"inArchive", false}};
        
        IFindFluent<Patient, Patient> listPatients = patientsCollection.Find(filter);
        
        return await listPatients.ToListAsync();
    }

    
    public async Task<List<Patient>> GetArchivePatientsAsync(string searchName = "")
    {
        var patientsCollection = _database.GetCollection<Patient>("patients");
        var filter = new BsonDocument{{"inArchive", true}};
        
        if (searchName.Any())
        {
            filter = new BsonDocument { { "inArchive", true }, { "name", searchName } };
        }
        
        IFindFluent<Patient, Patient> listPatients = patientsCollection.Find(filter);
        
        return await listPatients.ToListAsync();
    }

    
    public async Task<Patient?> GetPatientByIdAsync(string patientId)
    {
        var patientsCollection = _database.GetCollection<Patient>("patients");
        var filter = new BsonDocument{{"_id", ObjectId.Parse(patientId)}};
        
        IFindFluent<Patient, Patient> listPatients = patientsCollection.Find(filter);

        return await listPatients.FirstOrDefaultAsync();
    }
    
    
    public async Task CreatePatientAsync(Patient patient)
    {
        var patientsCollection = _database.GetCollection<Patient>("patients");
        await patientsCollection.InsertOneAsync(patient);

        var diagnosesCollection = _database.GetCollection<Diagnosis>("diagnoses");
        await diagnosesCollection.InsertOneAsync(new Diagnosis()
        {
            _id = ObjectId.GenerateNewId(),
            refToPatient = patient._id,
            text = String.Empty,
            disease = String.Empty
        });
    }

    
    public async Task DeletePatientByIdAsync(string patientId)
    {
        var patientsCollection = _database.GetCollection<Patient>("patients");
        var filter = new BsonDocument() { { "_id", ObjectId.Parse(patientId) } };
        await patientsCollection.DeleteOneAsync(filter);
    }
    
    
    public async Task UpdatePatientAsync(Patient patient)
    {
        var patientsCollection = _database.GetCollection<Patient>("patients");
        var filter = new BsonDocument() { { "_id", patient._id } };
        await patientsCollection.ReplaceOneAsync(filter, patient);
    }
    
    
    public async Task ChangePatientArchiveStatus(Patient patient)
    {
        var patientsCollection = _database.GetCollection<Patient>("patients");
        var filter = new BsonDocument() { { "_id", patient._id } };
        var updateSettings = new BsonDocument();
        if (patient.inArchive == true)
        {
            updateSettings = new BsonDocument("$set", new BsonDocument("inArchive", false));
        }
        else
        {
            updateSettings = new BsonDocument("$set", new BsonDocument("inArchive", true));
        }
        await patientsCollection.UpdateOneAsync(filter, updateSettings);
    }
}

public class DiagnosisService
{
    private readonly IMongoDatabase _database;

    public DiagnosisService(ClientMongoService client)
    {
        _database = client.Database;
    }
    
    
    public async Task<Diagnosis?> GetDiagnosisByPatientIdAsync(string patientId)
    {
        var diagnosisCollection = _database.GetCollection<Diagnosis>("diagnoses");
        var filter = new BsonDocument{{"refToPatient", ObjectId.Parse(patientId)}};
        
        IFindFluent<Diagnosis, Diagnosis> listDiagnoses = diagnosisCollection.Find(filter);
        
        return await listDiagnoses.FirstOrDefaultAsync();
    }
    
    
    public async Task RefreshDiagnosisAsync(string refToPatientId, string newDisease, string newText)
    {
        var diagnosisCollection = _database.GetCollection<Diagnosis>("diagnoses");
        var filter = new BsonDocument {{ "refToPatient", ObjectId.Parse(refToPatientId) }};
        
        await diagnosisCollection.DeleteOneAsync(filter);
        await diagnosisCollection.InsertOneAsync(new Diagnosis()
        {
            _id = ObjectId.GenerateNewId(),
            refToPatient = ObjectId.Parse(refToPatientId),
            text = newText,
            disease = newDisease
        });
    }
}


public class ReferralService
{
    private readonly IMongoDatabase _database;

    
    public ReferralService(ClientMongoService client)
    {
        _database = client.Database;
    }
    
    
    public async Task<List<Referral>> GetReferralsByPatientIdAsync(string refToPatientId)
    {
        var referralsCollection = _database.GetCollection<Referral>("referrals");
        var filter = new BsonDocument{{"refToPatient", ObjectId.Parse(refToPatientId)}};
        
        IFindFluent<Referral, Referral> listReferrals = referralsCollection.Find(filter);
        
        return await listReferrals.ToListAsync();
        
        return null;
    }
    
    
    public async Task CreateReferralByPatientIdAsync(Referral referral)
    {
        var referralsCollection = _database.GetCollection<Referral>("referrals");
        await referralsCollection.InsertOneAsync(referral);
    }

    
    public async Task<Referral> GetReferralByReferralIdAsync(string referralId)
    {
        var referralsCollection = _database.GetCollection<Referral>("referrals");
        var filter = new BsonDocument{{"_id", ObjectId.Parse(referralId)}};
        IFindFluent<Referral, Referral> listReferrals = referralsCollection.Find(filter);
        
        return await listReferrals.FirstOrDefaultAsync();
        
        return null;
    }

    public async Task UpdateReferralAsync(Referral referral)
    {
        var referralsCollection = _database.GetCollection<Referral>("referrals");
        var filter = new BsonDocument{{"_id", referral._id}};

        await referralsCollection.DeleteOneAsync(filter);
        await referralsCollection.InsertOneAsync(referral);
    }

    public async Task DeleteReferralByIdAsync(string referralId)
    {
        var referralsCollection = _database.GetCollection<Referral>("referrals");
        var filter = new BsonDocument{{"_id", ObjectId.Parse(referralId)}};

        await referralsCollection.DeleteOneAsync(filter);
    }
}


public class AnalyzesStorageService
{
    private readonly IMongoDatabase _database;

    public AnalyzesStorageService(ClientMongoService client)
    {
        _database = client.Database;
    }

    public async Task<List<Analysis>> GetAnalysesListByPatientIdAsync(string patientId)
    {
        var filter = new BsonDocument
        {
            {"refToPatient", ObjectId.Parse(patientId)}
        };
            
        var bloodTestCollection = _database.GetCollection<Analysis>("bloodTests");
        var ECGsCollection = _database.GetCollection<Analysis>("ECGs");
        
        IFindFluent<Analysis, Analysis> listBloodTests = bloodTestCollection.Find(filter);
        IFindFluent<Analysis, Analysis> listECGs = ECGsCollection.Find(filter);

        List<Analysis> result = new();
        result = await listBloodTests.ToListAsync();
        result.AddRange(listECGs.ToList());
        return result;
    }

    public async Task<BloodTest> GetBloodTestById(string bloodTestId)
    {
        var bloodTestCollection = _database.GetCollection<BloodTest>("bloodTests");
        var filter = new BsonDocument() { { "_id", ObjectId.Parse(bloodTestId) } };
        
        IFindFluent<BloodTest, BloodTest> listBloodTests = bloodTestCollection.Find(filter);
        
        return await listBloodTests.FirstOrDefaultAsync();
    }

    public async Task<ECG> GetEcgById(string ecgId)
    {
        var ecgCollection = _database.GetCollection<ECG>("ECGs");
        var filter = new BsonDocument() { { "_id", ObjectId.Parse(ecgId) } };

        IFindFluent<ECG, ECG> listECGs = ecgCollection.Find(filter);

        return await listECGs.FirstOrDefaultAsync();
    }
}

public class ScheduleService
{
    private readonly IMongoDatabase _database;

    
    public ScheduleService(ClientMongoService client)
    {
        _database = client.Database;
    }


    public async Task<Schedule> GetScheduleByDoctorIdAsync(string doctorId)
    {
        var scheduleCollection = _database.GetCollection<Schedule>("schedules");
        var filter = new BsonDocument() { { "refToDoctor", ObjectId.Parse(doctorId) } };

        IFindFluent<Schedule, Schedule> listSchedules = scheduleCollection.Find(filter);

        return await listSchedules.FirstOrDefaultAsync();
    }
    
    
    public async Task CreateScheduleAsync(string doctorId)
    {
        var scheduleCollection = _database.GetCollection<Schedule>("schedules");
        await scheduleCollection.InsertOneAsync(new Schedule()
        {
            _id = ObjectId.GenerateNewId(),
            refToDoctor = ObjectId.Parse(doctorId),
            schedule = new[]
            { 
                " ", " ", " ", " ", " ",
                " ", " ", " ", " ", " ",
                " ", " ", " ", " ", " ",
                " ", " ", " ", " ", " ",
                " ", " ", " ", " ", " ",
                " ", " ", " ", " ", " ",
                " ", " ", " ", " ", " "
            }
        });
    }

    
    public async Task UpdateScheduleAsync(Schedule schedule)
    {
        var scheduleCollection = _database.GetCollection<Schedule>("schedules");
        var filter = new BsonDocument() { { "refToDoctor", schedule.refToDoctor } };
        await scheduleCollection.DeleteOneAsync(filter);
        await scheduleCollection.InsertOneAsync(schedule);
    }

    public async Task ResetScheduleAsync(string doctorId)
    {
        var scheduleCollection = _database.GetCollection<Schedule>("schedules");
        var filter = new BsonDocument() { { "refToDoctor", ObjectId.Parse(doctorId) } };

        await scheduleCollection.DeleteOneAsync(filter);
        await scheduleCollection.InsertOneAsync(new Schedule()
        {
            _id = ObjectId.GenerateNewId(),
            refToDoctor = ObjectId.Parse(doctorId),
            schedule = new[]
            {
                "", "", "", "", "",
                "", "", "", "", "",
                "", "", "", "", "",
                "", "", "", "", "",
                "", "", "", "", "",
                "", "", "", "", "",
                "", "", "", "", ""
            }
        });
    }
}
