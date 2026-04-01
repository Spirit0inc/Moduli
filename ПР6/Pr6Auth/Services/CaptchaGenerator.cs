using System;
using System.Text;

namespace Pr6Auth.Services
{
    public class CaptchaGenerator
    {
        private static readonly Random random = new Random();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateCaptchaText(int length)
        {
            StringBuilder captcha = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(Characters.Length);
                captcha.Append(Characters[index]);
            }
            return captcha.ToString();  // ← Здесь было captcha.Text, исправь на ToString()
        }
    }
}