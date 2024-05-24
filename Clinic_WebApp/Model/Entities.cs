using MongoDB.Bson;

namespace Clinic_WebApp.Model;


/// <summary>
/// Класс описывает пользователя - доктора
/// </summary>
public class Doctor
{
    public ObjectId _id;
    public string? Login;
    public string? Password;
    public string? Name;
}

/// <summary>
/// Класс описывает пациента
/// </summary>
public class Patient
{
    public ObjectId _id;
    public ObjectId refToDoctor;
    public string? name;
    public string? policy;
    public string? gender;
    public string? phone;
    public int? age;
    public bool? inArchive;

    public string ShowGenderRu() => gender == "male" ? "Мужской" : "Женский";
}

/// <summary>
/// Класс описывает диагноз
/// </summary>
public class Diagnosis
{
    public ObjectId _id;
    public ObjectId refToPatient;
    public string disease;
    public string text;
}

/// <summary>
/// Класс описывает направление
/// </summary>
public class Referral
{
    public ObjectId _id;
    public ObjectId refToPatient;
    public string dateTime;
    public string AnalysisType;
}

/// <summary>
/// Класс описывает объявление на странице /Index в личном кабинете доктора
/// </summary>
public class Announcement
{
    public ObjectId _id;
    public string? Title;
    public string? Text;
}
