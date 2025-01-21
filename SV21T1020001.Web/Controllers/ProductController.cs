using Microsoft.AspNetCore.Mvc;
using SV21T1020001.BusinessLayers;
using SV21T1020001.DomainModels;
using SV21T1020001.Web.Models;
using SV21T1020001.Web;
using Microsoft.AspNetCore.Authorization;
using SV21T1020001.Web.AppCodes;

namespace SV21T1020001.Web.Controllers
{
    //[Authorize(Roles = $"{WebUserRoles.ADMINSTRATOR},{WebUserRoles.EMPLOYEE}")]
    [Authorize]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";
        public IActionResult Index()
        {
            ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    CategoryID = 0,
                    SupplierID = 0,
                    MinPrice = 0,
                    MaxPrice = 0,
                    SearchValue = ""
                };
            }
            return View(condition);
        }
        public IActionResult Search(ProductSearchInput condition)
        {
            int rowCount;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.CategoryID, condition.SupplierID, condition.MinPrice, condition.MaxPrice);

            ProductSearchResult model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                MinPrice = condition.MinPrice,
                MaxPrice = condition.MaxPrice,
                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            var data = new Product()
            {
                ProductID = 0,
                IsSelling = false,
            };
            return View("Edit", data);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            var data = ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
        public IActionResult Photo(int id = 0, string method = "", int photoID = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    ProductPhoto data = new ProductPhoto()
                    {
                        ProductID = id,
                        PhotoID = photoID,
                        Photo = "no.png"
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh cho mặt hàng";
                    var condition = ProductDataService.GetPhoto(photoID);
                    if (condition == null)
                    {
                        return RedirectToAction("Edit", id);
                    }
                    return View(condition);
                case "delete":
                    // Xóa  ảnh  ( xóa trực tiếp, ko cần confirm)
                    bool result = ProductDataService.DeletePhoto(photoID);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }

        }
        public IActionResult Attribute(int id = 0, string method = "", int attributeID = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    ProductAttribute data = new ProductAttribute()
                    {
                        ProductID = id,
                        AttributeID = attributeID,
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính cho mặt hàng";
                    var condition = ProductDataService.GetAttribute(attributeID);
                    if (condition == null)
                    {
                        return RedirectToAction("Edit", id);
                    }
                    return View(condition);
                case "delete":
                    // Xóa thuộc tính  ( xóa trực tiếp, ko cần confirm)
                    bool result = ProductDataService.DeleteAttribute(attributeID);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
            if (string.IsNullOrWhiteSpace(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên hàng không được rỗng");
            if (data.CategoryID == 0)
                ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
            if (data.SupplierID == 0)
                ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng chọn nhà cung cấp");
            if (string.IsNullOrWhiteSpace(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được rỗng");
            if (string.IsNullOrWhiteSpace(data.Price.ToString()) || data.Price <= 0)
                ModelState.AddModelError(nameof(data.Price), "Giá bán không hợp lệ");

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, "images/product", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                
                data.Photo = fileName;
            }
            if (data.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(data);

            }
            else
            {
                bool result = ProductDataService.UpdateProduct(data);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được thông tin mặt hàng.");
                    ViewBag.Title = "Cập nhật thông tin mặt hàng";
                    ViewBag.IsEdit = true;
                    return View("Edit", data);
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            ViewBag.Title = data.AttributeID == 0 ? "Bổ sung thuộc tính cho mặt hàng" : "Cập nhật thuộc tính cho mặt hàng";
            if (string.IsNullOrWhiteSpace(data.AttributeName))
                ModelState.AddModelError(nameof(data.AttributeName), "Tên không được rỗng");
            if (string.IsNullOrWhiteSpace(data.AttributeValue))
                ModelState.AddModelError(nameof(data.AttributeValue), "Giá trị thuộc tính không được rỗng");
            if (data.DisplayOrder <= 0)
                ModelState.AddModelError(nameof(data.DisplayOrder), "Thứ tự không hợp lệ");

            if (!ModelState.IsValid)
            {
                return View("Attribute", data);
            }

            if (data.AttributeID == 0)
            {
                long id = ProductDataService.AddAttribute(data);
            }
            else
            {
                bool result = ProductDataService.UpdateAttribute(data);

            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto = null)
        {

            ViewBag.Title = data.PhotoID == 0 ? "Bổ sung ảnh cho mặt hàng" : "Cập nhật ảnh cho mặt hàng";
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, "images/product", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            else
            {
                ModelState.AddModelError("uploadPhotoNull", "Chưa tải hình ảnh lên");
            }
            if (data.DisplayOrder <= 0)
                ModelState.AddModelError(nameof(data.DisplayOrder), "Thứ tự hiển thị không hợp lệ");
            if (!ModelState.IsValid)
            {
                return View("Photo", data);
            }
            if (data.PhotoID == 0)
            {
                long id = ProductDataService.AddPhoto(data);
            }
            else
            {
                bool result = ProductDataService.UpdatePhoto(data);

            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var data = ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");
            return View(data);
        }
    }
}