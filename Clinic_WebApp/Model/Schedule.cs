using MongoDB.Bson;

namespace Clinic_WebApp.Model;

/// <summary>
/// Класс описывает расписание
/// </summary>
public class Schedule
{
    public ObjectId _id;
    public ObjectId refToDoctor;
    public string[] schedule = new string[35];

    public string GetIndexValue(int index) => (schedule[index] == null) ? String.Empty : schedule[index];
}