using System.Threading.Tasks;
using Clinic_WebApp.Model;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Clinic_WebApp.Tests.Model;

[TestSubject(typeof(AuthenticateService))]
public class AuthenticateServiceTest
{
    private AuthenticateService _service = new AuthenticateService(
        new ClientMongoService(new MongoSettings()
        {
            Host = "mongodb://localhost:27017",
            Database = "ClinicWebApp"
        }));
    
    /// <summary>
    /// Тест аутенфикации 1.
    /// InlineData - корректные данные, необходимо вернуть <c>true</c>.
    /// </summary>
    [Theory]
    [InlineData("doctor1", "000000")]
    [InlineData("doctor2", "000001")]
    public async Task AuthenticateTest_ShouldReturnTrue(string login, string password)
    {
        // Arrange
        
        // Act
        bool expected = await _service.AuthenticateAsync(new DefaultHttpContext(), login, password);

        // Assert
        Assert.True(expected);
    }
    
    /// <summary>
    /// Тест аутенфикации 2.
    /// InlineData - некорректные данные, необходимо вернуть <c>false</c>.
    /// </summary>
    [Theory]
    [InlineData("doctor1", "888888")]
    [InlineData("doctor2", "666666")]
    [InlineData("adsdsas", "234234")]
    [InlineData("sadas", "")]
    [InlineData("sas", "")]
    [InlineData("", "232323")]
    [InlineData("", "")]
    public async Task AuthenticateTest_ShouldReturnFalse(string login, string password)
    {
        // Arrange

        // Act
        bool expected = await _service.AuthenticateAsync(new DefaultHttpContext(), login, password);

        //Assert
        Assert.False(expected);
    }
}