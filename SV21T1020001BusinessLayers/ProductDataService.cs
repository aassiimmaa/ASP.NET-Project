using SV21T1020001.BusinessLayers;
using SV21T1020001.DataLayers.SQLServer;
using SV21T1020001.DataLayers;
using SV21T1020001.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020001.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;

        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        // Phương thức tiện ích gọi hàm trên và trả về dữ liệu với `rowCount`
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue, categoryId, supplierId, minPrice, maxPrice);
            return (List<Product>)productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice);
        }
        /// <summary>
        /// Lấy thông tin 1 mặt hang theo mã mặt hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static Product? GetProduct(int productId)
        {
            return productDB.Get(productId);
        }
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }
        public static bool DeleteProduct(int productId)
        {
            return productDB.Delete(productId);
        }
        public static bool InUserProduct(int productId)
        {
            return productDB.InUsed(productId);
        }
        public static List<ProductPhoto> ListPhotos(int productID)
        {
            return (List<ProductPhoto>)productDB.ListPhotos(productID);
        }
        public static ProductPhoto? GetPhoto(long productID)
        {
            return productDB.GetPhoto(productID);
        }
        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        public static bool DeletePhoto(long productID)
        {
            return productDB.DeletePhoto(productID);
        }
        public static List<ProductAttribute> ListAttributes(int productID)
        {
            return (List<ProductAttribute>)productDB.ListAttributes(productID);
        }
        public static ProductAttribute? GetAttribute(int attributeID)
        {
            return productDB.GetAttribute(attributeID);
        }
        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        public static bool DeleteAttribute(long attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }

    }
}
