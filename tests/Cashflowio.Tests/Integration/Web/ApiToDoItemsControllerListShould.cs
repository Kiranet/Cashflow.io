using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cashflowio.Core.Entities;
using Cashflowio.Web;
using Newtonsoft.Json;
using Xunit;

namespace Cashflowio.Tests.Integration.Web
{

    public class ApiRawTransactionsControllerList : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ApiRawTransactionsControllerList(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsTwoItems()
        {
            var response = await _client.GetAsync("/api/RawTransactions");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<RawTransaction>>(stringResponse).ToList();

            Assert.Equal(2, result.Count());
            //Assert.Equal(1, result.Count(a => a.Title == "Test Item 2"));
        }
    }
}
