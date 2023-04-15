using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Services
{
    public class ProductSvc
    {
        private FlowerShopContext context;

        private readonly CommonSvc commonSvc = new CommonSvc();

        #region Function
        public bool IsExistProduct(string name)
        {
            using (context = new FlowerShopContext())
            {
                var p = context.Products.SingleOrDefault(x => x.Name == name);
                if (p != null)
                    return true;
                return false;
            }
        }

        public Product GetProductById(int id)
        {
            using (context = new FlowerShopContext())
            {
                try
                {
                    return context.Products.Include(x => x.Category).SingleOrDefault(x => x.Id == id);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public List<Product> GetListProduct()
        {
            using (context = new FlowerShopContext())
            {
                try
                {
                    return context.Products.Include(x => x.Category).Select(s => s).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public List<Product> GetListProductByCategoryId(int categoryId)
        {
            using (context = new FlowerShopContext())
            {
                try
                {
                    return context.Products.Include(x => x.Category).Where(x => x.CategoryId == categoryId).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        #endregion


        #region Function CRUD
        public string CreateProduct(IFormCollection collection, IFormFile image)
        {
            string res = string.Empty;

            Product product = new Product();
            product.Name = collection["Name"].ToString();
            product.Description = collection["Description"].ToString();
            if (image != null)
            {
                product.Image = commonSvc.UploadImageToCloudinary(image);
            }
            product.CategoryId = int.Parse(collection["CategoryId"].ToString());
            product.CreatedDate = DateTime.Now;
            product.Note = collection["Note"].ToString();
            product.Price = double.Parse(collection["Price"].ToString());
            product.View = 0;
            product.IsActive = true;

            if (!IsExistProduct(product.Name))
            {
                using (context = new FlowerShopContext())
                {
                    using (var trans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Products.Add(product);
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

                }
            }
            else
            {
                res = "u";
            }
            return res;
        }

        public string UpdateProduct(IFormCollection collection, IFormFile image, int id)
        {
            string res = string.Empty;

            Product product = new Product();
            product.Id = id;
            product.Name = collection["Name"].ToString();
            product.Description = collection["Description"].ToString();
            if (image != null)
            {
                product.Image = commonSvc.UploadImageToCloudinary(image);
            }
            else
            {
                product.Image = this.GetProductById(id).Image;
            }
            product.CategoryId = int.Parse(collection["CategoryId"].ToString());
            product.CreatedDate = DateTime.Parse(collection["CreatedDate"].ToString());
            product.Note = collection["Note"].ToString();
            product.Price = double.Parse(collection["Price"].ToString());
            product.View = int.Parse(collection["View"].ToString());
            product.IsActive = bool.Parse(collection["IsActive"].ToString());

            using (context = new FlowerShopContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Products.Update(product);
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
            }

            return res;
        }

        public string DeleteProduct(int id)
        {
            string res = string.Empty;

            using (context = new FlowerShopContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    var p = context.Products.SingleOrDefault(x => x.Id == id);
                    if (p != null)
                    {
                        try
                        {
                            context.Products.Remove(p);
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

        public void UpdataView(int id)
        {
            using(context=new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var p = context.Products.SingleOrDefault(x => x.Id == id);
                        p.View += 1;
                        context.Products.Update(p);
                        int i = context.SaveChanges();
                        if (i > 0)
                        {
                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                }
            }
        }
        #endregion

        
    }
}
