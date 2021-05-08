using Newtonsoft.Json;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<User> LoginAsync(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (user != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }

            using (var httpClient = new HttpClient())
            {

            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            else
            {
                return new User();
            }
        }

        public async Task<bool> RegisterAsync(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (user != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
