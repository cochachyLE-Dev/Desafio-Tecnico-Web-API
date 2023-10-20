using API.Domain.Auth;
using API.Domain.Shared;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Xunit;

namespace API.Test
{
    public class UnitTest: IDisposable
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:44379") };

        [Fact]
        public async Task<AuthenticationResponse> Authenticate()
        {
            var expectedStatusCode = HttpStatusCode.OK;
            var requestContent = new AuthenticationRequest { Email = "basicuser@vaetech.net", Password = "Abc123." };
            
            var response = await _httpClient.PostAsync("/api/Account/authenticate", TestHelpers.GetJsonStringContent(requestContent));                                                

            Assert.Equal(expectedStatusCode, response.StatusCode);
            if (response.StatusCode == expectedStatusCode)
            {
                string rsContent = await response.Content!.ReadAsStringAsync();
                var rs = JsonSerializer.Deserialize<Response<AuthenticationResponse>>(rsContent)!;

                if (rs.Successed)
                    return rs.Entity;
                else
                    throw new ArgumentException(rs.Message);
            }
            else
            {
                var rs = await response.Content.ReadAsStringAsync();
                throw new Exception(rs);
            }
        }

        [Fact]
        public async Task GetOrders()
        {
            // Arrange.
            var expectedStatusCode = HttpStatusCode.OK;        
            var stopwatch = Stopwatch.StartNew();

            // Act.
            var token = await Authenticate();          

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Order/GetAll");
            request.Headers.Add("Authorization", $"Bearer {token.JWToken}");

            var response = await _httpClient.SendAsync(request);

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);

        }

        public void Dispose()
        {
            _httpClient.DeleteAsync("/state").GetAwaiter().GetResult();
        }
    }
}