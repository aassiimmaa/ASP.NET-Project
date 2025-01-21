using Microsoft.AspNetCore.Mvc;
using SV21T1020001.DomainModels;
using SV21T1020001.BusinessLayers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020001.Shop.Controllers
{
    public class DetailProductController : Controller
    {
        public IActionResult Index(int ProductId)
        {
            Product product = ProductDataService.GetProduct(ProductId);
            return View(product);
        }
    }
}
