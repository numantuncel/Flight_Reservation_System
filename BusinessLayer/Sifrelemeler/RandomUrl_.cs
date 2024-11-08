using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Sifrelemeler
{
    public class RandomUrl_
    {
        private static Random random = new Random();

        public static string GenerateComplexRoleId()
        {
            // Rol ID'sini oluşturacak karakterlerin havuzu
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string specialChars = "!@#$%^&*";

            // Rastgele eleman sayıları belirle
            int letterCount = random.Next(2, 5); // 2 ila 4 harf
            int numberCount = random.Next(2, 5); // 2 ila 4 sayı
            int specialCharCount = random.Next(1, 3); // 1 ila 2 özel karakter

            // Harfleri, sayıları ve özel karakterleri rastgele seç
            string randomLetters = new string(Enumerable.Repeat(chars, letterCount)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            string randomNumbers = new string(Enumerable.Repeat(numbers, numberCount)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            string randomSpecialChars = new string(Enumerable.Repeat(specialChars, specialCharCount)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Tüm parçaları birleştir ve karıştır
            string combined = randomLetters + randomNumbers + randomSpecialChars;
            string complexRoleId = new string(combined.OrderBy(_ => random.Next()).ToArray());

            return complexRoleId;
        }
    }
}
