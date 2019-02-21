using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVC_Challenge.Security;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MVC_Challenge.Controllers
{
    using MVC_Challenge.Models;

    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserAccount user)
        {
            if(user.UserAccountName != null && user.UserAccountName == "MSTR")
            {
                if (ModelState.IsValid)
                {

                    using (nologodbEntities UserAccount = new nologodbEntities())
                    {
                        var obj = UserAccount.UserAccounts.Where(a => a.UserAccountName.Equals(user.UserAccountName) && a.UserAccountPassword.Equals(user.UserAccountPassword)).FirstOrDefault();
                        
                        if (obj != null)
                        {
                            Session["UserID"] = obj.UserAccountID.ToString();
                            Session["UserName"] = obj.UserAccountName.ToString();
                            return RedirectToAction("Posts", "Post");
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                }
                else
                {
                    return View(user);
                }
            }
            else if(user.UserAccountName != "MSTR")
            {

                string clearedpasstestString = Encrypt(user.UserAccountPassword);
                using(nologodbEntities UserAccount = new nologodbEntities())
                {
                    var obj = UserAccount.UserAccounts.Where(a => a.UserAccountName.Equals(user.UserAccountName) && a.UserAccountPassword.Equals(clearedpasstestString)).FirstOrDefault();
                    Console.Write(obj);
                    if (obj != null)
                    {
                        Session["UserID"] = obj.UserAccountID.ToString();
                        Session["UserName"] = obj.UserAccountName.ToString();
                        return RedirectToAction("Posts", "Post");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        public ActionResult Register(UserAccount User)
        {
            if (ModelState.IsValid)
            {
                User.UserAccountPassword = Encrypt(User.UserAccountPassword);
                nologodbEntities UserAccount = new nologodbEntities();
                UserAccount.UserAccounts.Add(User);
                UserAccount.SaveChanges();
                
                
            }
            return View();


        }

        public ActionResult Tournaments()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}