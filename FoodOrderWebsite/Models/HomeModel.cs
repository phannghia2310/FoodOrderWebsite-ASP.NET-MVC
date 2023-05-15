using FoodOrderWebsite.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrderWebsite.Models
{
    public class HomeModel
    {
        public List<Product> listProduct { get; set; }
        public List<Category> listCategory { get; set; }
    }
}