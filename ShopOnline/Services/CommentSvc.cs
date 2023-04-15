using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Services
{
    public class CommentSvc
    {
        private FlowerShopContext context;

        public void CreateComment(IFormCollection collection,string username,int parentId, int productId)
        {
            Comment comment = new Comment();
            comment.Content = collection["Content"].ToString();
            comment.Title = collection["Title"].ToString();
            comment.Username = username;
            comment.ProductId = productId;
            comment.IsActive = true;
            comment.CreatedDate = DateTime.Now;
            if (parentId!=0)
            {
                comment.ParentId = parentId;
            }

            using(context=new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Comments.Add(comment);
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
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void EditComment(int id, IFormCollection collection)
        {
            using(context=new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    var c = context.Comments.SingleOrDefault(x => x.Id == id);
                    c.Content = collection["Content"].ToString();
                    c.Title = collection["Title"].ToString();

                    try
                    {
                        context.Comments.Update(c);
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
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteComment(int id)
        {
            using(context=new FlowerShopContext())
            {
                using(var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var c = context.Comments.SingleOrDefault(x => x.Id == id);
                        c.IsActive = false;
                        context.Comments.Update(c);
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
                        throw;
                    }

                }
            }
        }

        public List<Comment> GetComments(int id)
        {
            using(context=new FlowerShopContext())
            {
                return context.Comments.Include(x => x.Parent).Include(x => x.UsernameNavigation).Where(x => x.ProductId == id && x.IsActive == true).ToList();
            }
        }
    }
}
