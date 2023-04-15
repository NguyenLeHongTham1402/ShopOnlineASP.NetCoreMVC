using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using ShopOnline.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Utils
{
    public class OrderStats
    {
        private FlowerShopContext context;
        private readonly OrderSvc orderSvc = new OrderSvc();
        public List<OrderStat> RevenueByMonthYear(int year)
        {
            List<OrderStat> p = new List<OrderStat>(); 
            if (year != 0)
            {
                using (context = new FlowerShopContext())
                {
                    var orderDtl = context.OrderDtls.GroupBy(x => new { x.OrderId }).Select(x => new
                    {
                        OrderId = x.Key.OrderId,
                        Total = x.Sum(x => x.Total)
                    }).ToList();

                    var os = from od in orderDtl
                             join o in context.OrderSales on od.OrderId equals o.Id
                             where o.CreatedDate.Value.Year == year && o.IsActive == true && o.IsPaid == true
                             group od by new { o.CreatedDate.Value.Month } into g
                             orderby g.Key.Month
                             select new OrderStat
                             {
                                 month = g.Key.Month,
                                 year = year,
                                 revenue = (double)g.Sum(x => x.Total) + (double)g.Sum(x => orderSvc.GetOrderSaleById(x.OrderId).TransportFee)
                             };
                    p = os.Select(x => x).ToList();

                }
            }
            return p;
        }
    }
}
