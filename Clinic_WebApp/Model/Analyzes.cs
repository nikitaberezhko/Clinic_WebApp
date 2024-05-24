using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Clinic_WebApp.Model;

/// <summary>
/// Класс описывает малообогащенную доменную сущность анализа
/// </summary>
[BsonIgnoreExtraElements]
public class Analysis
{
    public ObjectId _id;
    public ObjectId refToPatient;
    public string name;

    public string GenerateLocationString() =>
        name switch
        {
            "Анализ крови" => "/BloodTestPage",
            "ЭКГ" => "/ECGPage",
            _ => "/undefined"
        };
}

/// <summary>
/// Класс описывает анализ крови
/// </summary>
public class BloodTest
{
    public ObjectId _id;
    public ObjectId refToPatient;
    public string dateTime;
    public string name;
    public int HGB;
    public int PLT;
    public int WBC;
    public int RBC;
    public int HTC;
    public int Neutrophils;
    public int Monocytes;
    public int Eosinophils;
    public int Basophils;
    public int Pymphocytes;
}

/// <summary>
/// класс описывает сущность ЭКГ
/// </summary>
public class ECG
{
    public ObjectId _id;
    public ObjectId refToPatient;
    public string name; 
    public string dateTime;
    public string description;
    public string urlToScan;
}