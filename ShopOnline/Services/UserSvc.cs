using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ShopOnline.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Services
{
    public class UserSvc
    {
        private FlowerShopContext context;
        private static readonly string key = "yffbfJDHDhdhd#$%&dhdg#$%hdhd)855Dhdgd%";
        private static readonly string ROLE_ADMIN = "ADMIN";
        private static readonly string ROLE_USER = "USER";
        private readonly CommonSvc commonSvc = new CommonSvc();


        #region Function

        private string Encrypt(string pass)
        {
            using(var md5=new MD5CryptoServiceProvider())
            {
                using(var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    using(var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(pass);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        private string Decrypt(string pass_cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(pass_cipher);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        private bool IsExistsUsername(string username)
        {
            User user;
            using (context=new FlowerShopContext())
            {
                user = context.Users.AsEnumerable().SingleOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCulture));
            }
            return user != null ? true : false;
        }
        #endregion

        #region Function CRUD
        public string CreateUser(IFormCollection collection, IFormFile avatar)
        {
            string res = string.Empty;

            User user = new User();
            user.Username = collection["Username"].ToString();
            user.Password = Encrypt(collection["Password"].ToString());
            user.Name = collection["Name"].ToString();
            user.Email = collection["Email"].ToString();
            user.Avatar = commonSvc.UploadImageToCloudinary(avatar);
            user.Role = ROLE_USER;
            user.IsActive = true;

            if (!IsExistsUsername(user.Username))
            {
                using (context = new FlowerShopContext())
                {
                    using (var trans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Users.Add(user);
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
            }
            else
            {
                res = "u";
            }

            return res;            
        } 

        public User Signin(IFormCollection collection)
        {
            string username = collection["Username"].ToString();
            string password = Encrypt(collection["Password"].ToString());

            User user;
            using(context = new FlowerShopContext())
            {
                user = context.Users.AsEnumerable().SingleOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCulture) && x.Password == password);
            }

            return user;
        }


        #endregion
    }
}
