using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using StoreAdminUI.ViewModels;
using static System.Net.WebRequestMethods;

namespace StoreAdminUI.Controllers
{
    public class CategoryController : Controller
    {
        public async Task<IActionResult> Index(int page = 1)
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:7057/admin/api/Categories?page=" + page;
            var response =await client.GetAsync(url);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                PaginatedVM<CategoryListItem> model = JsonConvert.DeserializeObject<PaginatedVM<CategoryListItem>>(content); ;
                return View(model);
            }

            return View("Error");
        }
    }
}
