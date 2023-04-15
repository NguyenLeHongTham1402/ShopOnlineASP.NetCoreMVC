using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using ShopOnline.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Utils
{
    public class ProductStats
    {
        private readonly ProductSvc productSvc = new ProductSvc();
        private FlowerShopContext context;
        public List<ProductStat> TopSellingProductByMonthYear(int month, int year)
        {
            List<ProductStat> p = new List<ProductStat>();
            using(context = new FlowerShopContext())
            {
                if (month != 0 && year != 0)
                {
                    var ps = from od in context.OrderDtls
                             join o in context.OrderSales on od.OrderId equals o.Id
                             where o.CreatedDate.Value.Month == month && o.CreatedDate.Value.Year == year && o.IsActive == true && o.IsPaid == true
                             group od by new { od.ProductId } into g
                             orderby (double)g.Sum(x => x.Quantity) descending
                             select new ProductStat
                             {

                                 ProductName = productSvc.GetProductById(g.Key.ProductId).Name,
                                 Quantity = (double)g.Sum(x => x.Quantity),
                                 Revenue = (double)g.Sum(x => x.Total)
                             };
                    p = ps.Select(x => x).ToList();
                                            
                }
                
            }

            return p;
        }
    }
}
