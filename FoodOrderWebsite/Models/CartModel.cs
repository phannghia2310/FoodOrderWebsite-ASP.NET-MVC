using FoodOrderWebsite.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrderWebsite.Models
{
    public class CartItem
    {
        public Product _product { get; set; }
        public int _quantity { get; set; }
    }

    public class CartModel
    {
        List<CartItem> items = new List<CartItem>();

        public List<CartItem> Items
        {
            get { return items; }
        }

        public void Add(Product product)
        {
            var item = items.FirstOrDefault(p => p._product.ProductID == product.ProductID);
            if(item == null)
            {
                items.Add(new CartItem
                {
                    _product = product,
                    _quantity = 1
                });
            }
            else
            {
                item._quantity += 1;
            }
        }

        public void Remove(Product product)
        {
            var item = items.FirstOrDefault(p => p._product.ProductID == product.ProductID);
            if(item != null)
            {
                if (item._quantity > 1)
                {
                    item._quantity -= 1;
                }
                else
                {
                    items.Remove(item);
                }
            }
        }

        public void Update(int id, int quantity)
        {
            var item = items.Find(i => i._product.ProductID == id);
            if (item != null)
            {
                item._quantity = quantity;
            }
        }

        public int TotalQuanity()
        {
            return items.Sum(s => s._quantity);
        }

        public void ClearCart()
        {
            items.Clear();
        }

        public int TotalPrice()
        {
            int total = 0;
            foreach(var item in items)
            {
                total += item._product.Price.Value * item._quantity;
            }
            return total;
        }
    }
}