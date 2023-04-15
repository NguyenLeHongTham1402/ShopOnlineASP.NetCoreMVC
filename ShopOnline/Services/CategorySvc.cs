using Microsoft.AspNetCore.Http;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Services
{
    public class CategorySvc
    {
        private FlowerShopContext context;

        public string CreateCategory(IFormCollection collection)
        {
            string res = string.Empty;

            Category cate = new Category();
            cate.Name = collection["Name"].ToString();

            using (context = new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Categories.Add(cate);
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
            }

            return res;
        }

        public string UpdateCategory(IFormCollection collection, int id)
        {
            string res = string.Empty;

            Category cate = new Category();
            cate.Id = id;
            cate.Name = collection["Name"].ToString();

            using(context=new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Categories.Update(cate);
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

        public string DeleteCategory(int id)
        {
            string res = string.Empty;

            using(context = new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var c = context.Categories.SingleOrDefault(x => x.Id == id);
                        if (c != null)
                        {
                            context.Categories.Remove(c);
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
                        else
                        {
                            res = "n";
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

        public List<Category> ListCategories()
        {
            List<Category> res;
            using(context = new FlowerShopContext())
            {
                try
                {
                    res = context.Categories.Select(x => x).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return res;
        }

        public Category GetCategoryById(int id)
        {
            using(context=new FlowerShopContext())
            {
                try
                {
                    return context.Categories.SingleOrDefault(x => x.Id == id);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
