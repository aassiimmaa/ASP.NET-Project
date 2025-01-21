using SV21T1020001.DomainModels;

namespace SV21T1020001.Shop.Models
{
    public class ProductSearchResult : PaginationSearchResult
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; }
        public required List<Product> Data { get; set; }
    }
}
