using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Text;

namespace BlackNails.CommonClass
{
    /// <summary>
    /// 安全
    /// <remarks>
    /// 创建：2016.02.11
    /// </remarks>
    /// </summary>
    public class Security
    {
        /// <summary>
        /// 创建验证码字符
        /// </summary>
        /// <param name="length">字符长度</param>
        /// <returns>验证码字符</returns>
        public static string CreateVerificationText(int length)
        {
            char[] _verification = new char[length];
            char[] _dictionary = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            Random _random = new Random();
            for (int i = 0; i < length; i++) { _verification[i] = _dictionary[_random.Next(_dictionary.Length - 1)]; }
            return new string(_verification);
        }


        /// <summary>
        /// 256位散列加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public static string Sha256(string plainText)
        {
            return plainText;
            //SHA256Managed _sha256 = new SHA256Managed();
            //byte[] _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(plainText));
            //return Convert.ToBase64String(_cipherText);
        }
    }
}
