using APP.Domain.Shared;
using APP.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace APP.Data
{
    public class ApiContext : IApiContext
    {
        public readonly IConfiguration _configuration;
        public readonly string _API_URL;
        public ApiContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _API_URL = _configuration.GetValue<string>("API_URL");
        }

        public ApiSet<OrderModel> Orders => new ApiSet<OrderModel>(_API_URL);
        public ApiSet<ClientModel> Clients => new ApiSet<ClientModel>(_API_URL);
        public ApiSet<ProductModel> Products => new ApiSet<ProductModel>(_API_URL);
    }
    public class ApiSet<T> 
    {        
        public readonly string _API_URL;
        public ApiSet(string api_url)  => _API_URL = api_url;        

        public async Task<Response<T>> InsertAsync(T data, bool inMemoryCache = false)
        {
            if (typeof(T) == typeof(OrderModel))
            {
                if (inMemoryCache)
                    return await PostAsync<T, T>("api/Order/Add", data);
                else
                    return await PostAsync<T, T>("api/Order/Insert", data);
            }
            else if (typeof(T) == typeof(ProductModel))            
                return await PostAsync<T, T>("api/Product/Insert", data);            
            else
            {
                return Response<T>.Fail(StatusCode.NotImplemented, "Not Implemented");
            }
        }
        public async Task<Response> DeleteAsync(int id)
        {
            if (typeof(T) == typeof(OrderModel))
                return await DeleteAsync(string.Format("api/Order/Delete/{0}", id));
            else
            {
                return Response.Fail(StatusCode.NotImplemented, "Not Implemented");
            }
        }
        public async Task<Response> DeleteItemAsync(params object[] param)
        {
            if (typeof(T) == typeof(OrderModel))
                return await DeleteAsync(string.Format("api/Order/DeleteItem/{0}/{1}", param[0], param[1]));
            else
            {
                return Response.Fail(StatusCode.NotImplemented, "Not Implemented");
            }
        }
        public async Task<Response<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(OrderModel))
                return await GetAsync<T>("api/Order/GetAll");
            else if (typeof(T) == typeof(ClientModel))
                return await GetAsync<T>("api/Client/GetAll");
            else if (typeof(T) == typeof(ProductModel))
                return await GetAsync<T>("api/Product/GetAll");
            else
            {
                return Response<T>.Fail(StatusCode.NotImplemented, "Not Implemented");
            }
        }
        public async Task<Response<T>> SingleOrDefaultAsync(params object[] param)
        {
            if (typeof(T) == typeof(OrderModel))
                return await GetAsync<T>(string.Format("api/Order/Single/{0}", param[0]));
            else
            {
                return Response<T>.Fail(StatusCode.NotImplemented, "Not Implemented");
            }
        }
        public async Task<Response<T>> SearchAsync(string filter)
        {
            if (typeof(T) == typeof(OrderModel))
                return await GetAsync<T>("api/Order/Search?filter=" + WebUtility.UrlEncode(filter));
            else if (typeof(T) == typeof(ClientModel))
                return await GetAsync<T>("api/Client/Search?filter=" + WebUtility.UrlEncode(filter));
            else if (typeof(T) == typeof(ProductModel))
                return await GetAsync<T>("api/Product/Search?filter=" + WebUtility.UrlEncode(filter));
            else
            {
                return Response<T>.Fail(StatusCode.NotImplemented, "Not Implemented");
            }
        }
        private async Task<string> GetToken()
        {
            var user = new AuthenticationRequest { Email = "basicuser@vaetech.net", Password = "Abc123." };

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_API_URL}/api/Account/authenticate");
            var content = new StringContent(JsonSerializer.Serialize(user), null, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string rsContent = await response.Content!.ReadAsStringAsync();
                var rs = JsonSerializer.Deserialize<Response<AuthenticationResponse>>(rsContent)!;

                if (rs.Successed)
                    return rs.Entity?.JWToken!;
                else
                    throw new ArgumentException(rs.Message);
            }
            else
            {
                var rs = await response.Content.ReadAsStringAsync();
                throw new Exception(rs);
            }
        }
        public async Task<Response<TResult>> GetAsync<TResult>(string query)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_API_URL}/{query}");
            request.Headers.Add("Authorization", $"Bearer {await GetToken()}");

            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content!.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<TResult>>(content)!;
            }
            else
            {
                string content = await response.Content!.ReadAsStringAsync();
                return Response<TResult>.Fail(StatusCode.InternalError, content);
            }
        }
        public async Task<Response<TResult>> PostAsync<TResult, TRequest>(string query,TRequest data)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_API_URL}/{query}");
            request.Headers.Add("Authorization", $"Bearer {await GetToken()}");
            var body = new StringContent(JsonSerializer.Serialize(data), null, "application/json");
            request.Content = body;

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content!.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<TResult>>(content)!;
            }
            else
            {
                string content = await response.Content!.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<TResult>>(content)!;
            }
        }
        public async Task<Response> DeleteAsync(string query) {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_API_URL}/{query}");
            request.Headers.Add("Authorization", $"Bearer {await GetToken()}");
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content!.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content)!;
            }
            else
            {
                string content = await response.Content!.ReadAsStringAsync();
                return Response.Fail(StatusCode.InternalError, content);
            }
        }
    }
}
