
namespace SV21T1020001.Web.Models
    {/// <summary>
    ///  Lưu giữ thông tin đầu vào sử dụng cho chức năng tìm kiếm và hiển thị dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        /// <summary>
        /// Trang cần hiển thị
        public int Page { get; set; } = 1;
        /// Số dòng hiển thị trên mõi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        ///  chuỗi giá trị cần tìm kiếm
        /// </summary>
        public String SearchValue { get; set; } = "";  
    }
}
