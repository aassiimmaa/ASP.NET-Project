using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SV21T1020001.BusinessLayers;
using SV21T1020001.DomainModels;
using SV21T1020001.Shop.Models;

namespace SV21T1020001.Shop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";

        private List<CartItem> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<CartItem>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }
            return shoppingCart;
        }

        public IActionResult AddToCart(CartItem item)
        {
            if (item.SalePrice < 0 || item.Quantity <= 0)
                return Json("Giá bán và số lượng không hợp lệ");

            var shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == item.ProductID);
            if (existsProduct == null)
            {
                shoppingCart.Add(item);
            }
            else
            {
                existsProduct.Quantity += item.Quantity;
                existsProduct.SalePrice = item.SalePrice;
            }

            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            TempData["Message"] = "OK";
            return RedirectToAction("Index", "DetailProduct", new {ProductID = item.ProductID});
        }

        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
            {
                shoppingCart.RemoveAt(index);
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }
            return RedirectToAction("ShoppingCart");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
                return View("ShoppingCart", GetShoppingCart());

            var shoppingCart = GetShoppingCart();

            var product = shoppingCart.FirstOrDefault(m => m.ProductID == productId);
            if (product == null)
                return Json("Sản phẩm không có trong giỏ hàng");

            product.Quantity = quantity;

            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            Json("Số lượng đã được cập nhật");
            return RedirectToAction("ShoppingCart");
        }

        public IActionResult ShoppingCart()
        {
            return View(GetShoppingCart());
        }

        public IActionResult Orders() {
            List<Order> orders = OrderDataService.GetOrdersByCustomerId(int.Parse(User.GetUserData().UserId));
            return View(orders);
        }

        public IActionResult Init(int customerID = 0, string deliveryProvince = "", string deliveryAddress = "")
        {
            var shoppingCart = GetShoppingCart();
            if (shoppingCart.Count == 0)
            {
                ModelState.AddModelError("EmptyCart", "Giỏ hàng trống, vui lòng chọn sản phẩm thêm vào giỏ");
                return View("ShoppingCart", GetShoppingCart());
            }

            if (customerID == 0 || string.IsNullOrWhiteSpace(deliveryProvince) || string.IsNullOrWhiteSpace(deliveryAddress))
            {
                ModelState.AddModelError("EmptyInfo", "Vui lòng nhập đầy đủ thông tin nơi giao hàng.");
                return View("ShoppingCart", GetShoppingCart());
            }

            int employeeID = 1; //TODO: Thay bởi ID nhân viên đang login vào hệ thống

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCart)
            {
                orderDetails.Add(new OrderDetail()
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    SalePrice = item.SalePrice
                });
            }
            int orderID = OrderDataService.InitOrder(employeeID, customerID, deliveryProvince, deliveryAddress, orderDetails);
            ClearCart();
            return RedirectToAction("Orders");
        }
        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult OrderDetail(int id) {
            Order order = OrderDataService.GetOrder(id);
            return View(order);
        }

        public IActionResult CancelOrder(int id) {
            OrderDataService.CancelOrder(id);
            Order order = OrderDataService.GetOrder(id);
            return View("OrderDetail", order);
        }

    }
}
