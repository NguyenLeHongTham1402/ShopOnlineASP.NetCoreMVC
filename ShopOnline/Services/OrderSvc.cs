using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Services
{
    public class OrderSvc
    {
        private FlowerShopContext context;
        public List<OrderSale> GetListOrderSales(string username)
        {
            using (context = new FlowerShopContext())
            {
                if (!string.IsNullOrWhiteSpace(username))
                {
                    return context.OrderSales.Include(x => x.OrderDtls).AsEnumerable().Where(x => x.Username.Equals(username, StringComparison.InvariantCulture) == true && x.IsActive == true).ToList();
                }
                return context.OrderSales.Include(x => x.OrderDtls).AsEnumerable().Select(x => x).ToList();
            }
        }

        public OrderSale GetOrderSaleById(int id)
        {
            using(context=new FlowerShopContext())
            {
                try
                {

                    return context.OrderSales.Include(x => x.OrderDtls).ThenInclude(x => x.Product).SingleOrDefault(x => x.Id == id);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public string AddOrder(IFormCollection collection, List<Cart> carts)
        {
            string res = string.Empty;

            OrderSale orderSale = new OrderSale();
            orderSale.Username = collection["Username"].ToString();
            orderSale.CreatedDate = DateTime.Now;
            orderSale.OrderName = collection["OrderName"].ToString();
            orderSale.Address = collection["Address"].ToString();
            orderSale.Phone = collection["Phone"].ToString();
            orderSale.Email = collection["Email"].ToString();
            orderSale.ReceivedDate = DateTime.Parse(collection["ReceivedDate"].ToString());
            orderSale.Payment = collection["Payment"].ToString();
            orderSale.Receiver = collection["Receiver"].ToString();
            orderSale.TransportFee = double.Parse(collection["TransportFee"].ToString());
            orderSale.Transportion = collection["Transport"].ToString();
            orderSale.IsPaid = false;
            orderSale.Status = collection["PhoneReceiver"].ToString() + "#" + collection["Note"].ToString();
            orderSale.IsActive = true;

            using(context = new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var p = context.OrderSales.Add(orderSale);
                        int i = context.SaveChanges();
                        if (i > 0)
                        {
                            List<OrderDtl> orderDtls = new List<OrderDtl>();
                            foreach(var item in carts)
                            {
                                orderDtls.Add(new OrderDtl(p.Entity.Id, item.productId, item.quantity, item.price, item.total));
                            }

                            context.OrderDtls.AddRange(orderDtls);
                            int j = context.SaveChanges();
                            if (j > 0 && j == orderDtls.Count)
                            {
                                trans.Commit();
                                res = "t";
                            }
                            else
                            {
                                trans.Rollback();
                                res = "f";
                            }
                        }
                        else
                        {
                            trans.Rollback();
                            res = "f";
                        }
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        res = ex.StackTrace;
                    }
                }
            }

            return res;
        }

        public string UpdateOrder(IFormCollection collection, int id)
        {
            string res = string.Empty;

            using(context=new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    var od = context.OrderSales.SingleOrDefault(x => x.Id == id);
                    if(od != null)
                    {
                        od.Address = collection["Address"].ToString();
                        od.IsPaid = collection["IsPaid"].ToString() == "true" ? true : false;
                        od.Status = collection["PhoneReceive"].ToString() + "#" + collection["Note"].ToString();

                        try
                        {
                            context.OrderSales.Update(od);
                            int i = context.SaveChanges();
                            if (i > 0)
                            {
                                trans.Commit();
                                res = "t";
                            }
                            else
                            {
                                trans.Rollback();
                                res = "f";
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            res = ex.StackTrace;
                        }
                    }
                    else
                    {
                        res = "n";
                    }
                }
            }

            return res;

        }

        public void UpdatePayment(int id)
        {
            using(context = new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var od = context.OrderSales.SingleOrDefault(x => x.Id == id);
                        if (od != null)
                        {
                            od.IsPaid = true;
                        }
                        else
                            return;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        public string DeleteOrder(int id)
        {
            string res = string.Empty;

            using(context=new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    var p = context.OrderSales.SingleOrDefault(x => x.Id == id);
                    DateTime createdDate = p.CreatedDate ?? DateTime.Now;
                    var t = DateTime.Now.Subtract(createdDate).TotalHours;
                    if (t <= 3 && t >= 0)
                    {
                        try
                        {
                            p.IsActive = false;
                            context.OrderSales.Update(p);
                            int i = context.SaveChanges();
                            if (i > 0)
                            {
                                trans.Commit();
                                res = "t";
                            }
                            else
                            {
                                trans.Rollback();
                                res = "f";
                            }
                        }
                        catch(Exception ex)
                        {
                            trans.Rollback();
                            res = ex.StackTrace;
                        }
                    }
                    else
                    {
                        res = "to";
                    }
                }
            }

            return res;
        }


    }
}
