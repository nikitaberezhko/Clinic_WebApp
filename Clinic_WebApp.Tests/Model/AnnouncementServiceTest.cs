using System.Threading.Tasks;
using Clinic_WebApp.Model;
using JetBrains.Annotations;
using MongoDB.Bson;
using Xunit;

namespace Clinic_WebApp.Tests.Model;

[TestSubject(typeof(AnnouncementService))]
public class AnnouncementServiceTest
{
    private AnnouncementService _service = new AnnouncementService(
        new ClientMongoService(new MongoSettings()
        {
            Host = "mongodb://localhost:27017",
            Database = "ClinicWebApp"
        }));
    
    /// <summary>
    /// Тест получения объявления из БД.
    /// Объявление представляет класс <c>Announcement</c>
    /// Проверка на <c>null</c>.
    /// Проверка на соотвествие типу <c>Announcement</c>.
    /// Сравнение с эквивалентом получаемого объекта.
    /// </summary>
    [Fact]
    public void AnnouncementTest()
    {
        // Arrange
        Announcement expected = new Announcement() {
            _id = ObjectId.Parse("65cf4447d8297efb74373e6a"),
            Title = "Объявление из БД",
            Text = "Lorem Ipsum из базы данных - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. " +
                   "Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. " +
                   "В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя " +
                   "Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений" +
                   " пять веков, но и перешагнул в электронный дизайн."
        };

        // Act
        Task<Announcement> announcement = _service.GetAnnouncementAsync();

        // Assert
        Assert.NotNull(announcement.Result);
        Assert.IsType<Announcement>(announcement.Result);
        Assert.Equivalent(expected, announcement.Result);
    }
}