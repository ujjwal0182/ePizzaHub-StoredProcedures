using ePizzaHub.UI.Models.ApiModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ePizzaHub.UI.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IHttpClientFactory httpClientFactory;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var client = httpClientFactory.CreateClient("ePizzaAPI");
            var items = await client.GetFromJsonAsync<IEnumerable<ItemResponseModel>>("Item");
            return View(items);
        }
    }
}
