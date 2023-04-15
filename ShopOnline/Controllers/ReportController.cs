using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class ReportController : Controller
    {
        private readonly ProductStats productStats = new ProductStats();
        private readonly OrderStats orderStats = new OrderStats();

        public IActionResult ReportProduct()
        {
            return View();
        }

        public IActionResult ReportOrder()
        {
            return View();
        }

        [HttpPost]
        public List<object> ProductRevunue(IFormCollection collection)
        {
            int month = int.Parse(collection["month"].ToString());
            int year = int.Parse(collection["year"].ToString());
            var res = productStats.TopSellingProductByMonthYear(month, year);
            List<object> data = new List<object>();

            List<string> labels = res.Select(x => x.ProductName).ToList();
            data.Add(labels);

            List<double> revunues = res.Select(x => x.Revenue).ToList();
            data.Add(revunues);

            return data;
        }

        [HttpPost]
        public IActionResult ProductQuantity(IFormCollection collection)
        {
            int month = int.Parse(collection["month"].ToString());
            int year = int.Parse(collection["year"].ToString());
            var res = productStats.TopSellingProductByMonthYear(month, year);
            List<object> data = new List<object>();

            List<string> labels = res.Select(x => x.ProductName).ToList();
            data.Add(labels);

            List<double> quantities = res.Select(x => x.Quantity).ToList();
            data.Add(quantities);

            return Json(data);
        }

        [HttpPost]
        public List<object> OrderRevenue(IFormCollection collection)
        {
            int year = int.Parse(collection["year"].ToString());
            var res = orderStats.RevenueByMonthYear(year);

            List<object> data = new List<object>();

            List<string> labels = res.Select(x => (x.month.ToString() + "/" + x.year.ToString())).ToList();
            data.Add(labels);

            List<double> revenues = res.Select(x => x.revenue).ToList();
            data.Add(revenues);

            return data;
        }
    }
}
