using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace PersonalTaskManager.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Message { get; set; }

        public static string CalcHash(string pass)
        {
            MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();
            byte[] buf = Encoding.GetEncoding(1251).GetBytes(pass);
            byte[] hashBytes = csp.ComputeHash(buf);
            string hash = string.Empty;
            foreach(byte b in hashBytes)
            {
                hash += string.Format("{0:x2}", b);
            }
            return hash;
        }

    }
}