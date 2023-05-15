using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoodOrderWebsite.Models
{
    public class UserSetting
    {
        [DisplayName("Mã người dùng")]
        public int UserID { get; set; }
        [DisplayName("Tên người dùng")]
        public string Name { get; set; }
        [DisplayName("Điện thoại")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }
        [DisplayName("Hình ảnh")]
        public string ImageURL { get; set; }

        [NotMapped]
        public System.Web.HttpPostedFileBase ImageUpload { get; set; }
    }
}