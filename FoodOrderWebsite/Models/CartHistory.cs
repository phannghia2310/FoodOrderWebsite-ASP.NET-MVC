using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FoodOrderWebsite.Models
{
    public class CartHistory
    {
        [DisplayName("Mã đơn hàng")]
        public int OrderID { get; set; }
        [DisplayName("Tên đơn hàng")]
        public string OrderName { get; set; }
        [DisplayName("Ngày đặt hàng")]
        public Nullable<System.DateTime> OrderDate { get; set; }
        [DisplayName("Hình ảnh")]
        public string ImageURL { get; set; }
        [DisplayName("Trạng thái")]
        public Nullable<int> Status { get; set; }
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [DisplayName("Thành tiền")]
        public Nullable<int> Total { get; set; }
        [DisplayName("Số lượng")]
        public Nullable<int> Quantity { get; set; }
    }
}