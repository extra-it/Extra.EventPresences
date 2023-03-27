using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Extentions
{
    public static class StringExtensions
    {

        public static string Clean(this string input)
        {
            string result = input;

            //normalizza
            if (result == null) { result = ""; }

            //mette in minuscolo
            result = result.ToLower();

            //sostituisce i caratteri non alfanumerici con uno spazio
            result = Regex.Replace(result, @"\W", " ");

            //riporta eventuali doppi spazi a uno solo
            result = Regex.Replace(result, @"\s+", " ");

            //sistema gli accenti
            StringBuilder sb = new StringBuilder(result);
            sb.Replace("à", "a");
            sb.Replace("á", "a");
            sb.Replace("è", "e");
            sb.Replace("é", "e");
            sb.Replace("ì", "i");
            sb.Replace("í", "i");
            sb.Replace("ò", "o");
            sb.Replace("ó", "o");
            sb.Replace("ù", "u");
            sb.Replace("ú", "u");

            //sistema i caratteri particolari
            sb.Replace("â", "a");
            sb.Replace("ã", "a");
            sb.Replace("ä", "a");
            sb.Replace("å", "a");
            sb.Replace("æ", "a");
            sb.Replace("ç", "c");
            sb.Replace("ê", "e");
            sb.Replace("ë", "e");
            sb.Replace("î", "i");
            sb.Replace("ï", "i");
            sb.Replace("ð", "o");
            sb.Replace("ñ", "n");
            sb.Replace("ô", "o");
            sb.Replace("õ", "o");
            sb.Replace("ö", "o");
            sb.Replace("ø", "o");
            sb.Replace("û", "u");
            sb.Replace("ü", "u");
            sb.Replace("ý", "y");
            sb.Replace("ÿ", "y");
            sb.Replace("ā", "a");
            sb.Replace("ă", "a");
            sb.Replace("ą", "a");
            sb.Replace("ć", "c");
            sb.Replace("ĉ", "c");

            //restituisce
            return sb.ToString();
        }

        public static String UrlNormalize(this String input)
        {
            const string pattern1 = @"\W";
            const string pattern2 = @"\-{2,}";

            String res = input;

            //normalizza
            if (res == null) res = "";

            //mette in minuscolo
            res = res.ToLower();

            //sostituisce i caratteri non alfanumerici con un trattino
            res = Regex.Replace(res, pattern1, "-");

            //riporta eventuali doppi trattini a uno solo
            res = Regex.Replace(res, pattern2, "-");

            //sistema gli accenti
            StringBuilder sb = new StringBuilder(res);
            sb.Replace("à", "a");
            sb.Replace("á", "a");
            sb.Replace("è", "e");
            sb.Replace("é", "e");
            sb.Replace("ì", "i");
            sb.Replace("í", "i");
            sb.Replace("ò", "o");
            sb.Replace("ó", "o");
            sb.Replace("ù", "u");
            sb.Replace("ú", "u");

            //sistema i caratteri particolari
            sb.Replace("â", "a");
            sb.Replace("ã", "a");
            sb.Replace("ä", "a");
            sb.Replace("å", "a");
            sb.Replace("æ", "a");
            sb.Replace("ç", "c");
            sb.Replace("ê", "e");
            sb.Replace("ë", "e");
            sb.Replace("î", "i");
            sb.Replace("ï", "i");
            sb.Replace("ð", "o");
            sb.Replace("ñ", "n");
            sb.Replace("ô", "o");
            sb.Replace("õ", "o");
            sb.Replace("ö", "o");
            sb.Replace("ø", "o");
            sb.Replace("û", "u");
            sb.Replace("ü", "u");
            sb.Replace("ý", "y");
            sb.Replace("ÿ", "y");
            sb.Replace("ā", "a");
            sb.Replace("ă", "a");
            sb.Replace("ą", "a");
            sb.Replace("ć", "c");
            sb.Replace("ĉ", "c");

            //restituisce
            return sb.ToString();

        }

        public static String UpperTrim(this String input)
        {
            return input.Trim().ToUpper();
        }

        public static String UrlNormalizeNoMinus(this String input)
        {

            //restituisce
            return input.UrlNormalize().Replace("-", "");

        }

        public static String ConvertCR2BR(this string input)
        {
            return input.Replace("\n", "<br />");
        }

        public static String Truncate(this string input, int maxchars)
        {
            if (input.Length > maxchars)
                return input.Substring(0, maxchars) + "...";
            else
                return input;
        }

        public static String EstrapolaFrase(this String testo, String keyword, int margine)
        {
            int idx = testo.Trim().ToLower().IndexOf(keyword.Trim().ToLower());
            int ind = idx;
            if (ind > 0)
            {
                String res = String.Empty;
                while (testo[ind] != ' ' && ind > 0)
                {
                    ind--;
                    res = res + testo[ind];
                }
                res = res.Trim();
                res = Reverse(res);
                String final = res + keyword;
                idx = testo.Trim().ToLower().IndexOf(final.Trim().ToLower());
            }
            if (idx > 0)
            {
                String second = testo.Substring(idx);
                second = SmoothCut(" " + second, margine);
                String firstReverse = Reverse(testo.Substring(0, idx));
                String first = Reverse(SmoothCut(firstReverse, margine));
                return String.Format("{0}{1}", first, second);
            }
            return SmoothCut(testo, margine);
        }

        public static String Reverse(String text)
        {
            char[] strArray = text.ToCharArray();
            Array.Reverse(strArray);
            string strReversed = new string(strArray);
            return strReversed;
        }

        public static string TruncateAt(this string str, int maxSize = Int32.MaxValue, string suspensionDots = null)
        {
            if (String.IsNullOrWhiteSpace(str) || str.Length < maxSize)
                return str;
            int firstSpaceID = str.IndexOf(' ', maxSize);
            if (firstSpaceID != -1)
                return String.Format("{0}{1}", str.Substring(0, firstSpaceID), suspensionDots);
            else
                return String.Format("{0}{1}", str.Substring(0, maxSize), suspensionDots);
        }

        public static String SmoothCut(this String longText, int maxLength, string suspensionDots = "...")
        {
            String abs = (String)longText;

            if (String.IsNullOrEmpty(abs))
                return String.Empty;
            else
            {
                if (abs.Length <= maxLength)
                    return abs;
            }

            String pCR = @"\s*\r?\n\s*";
            String absText = Regex.Replace(abs, pCR, " ");

            String pattern = String.Format(".{{0,{0}}}(.\\w+)", maxLength.ToString());
            String matchText = Regex.Match(absText, pattern, RegexOptions.IgnoreCase).ToString();
            //replace
            String repPatt = @"(\s\w+$)";
            String res = Regex.Replace(matchText, repPatt, suspensionDots);

            if (String.IsNullOrEmpty(res))
                return abs;
            else
                return res;
        }

        public static string UppercaseWords(this String value)
        {
            if (String.IsNullOrWhiteSpace(value)) return String.Empty;

            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string NormalizeForUri(this string ToBeNormalized)
        {
            if (ToBeNormalized == null)
                return "";

            string Ret = "";
            foreach (char C in ToBeNormalized.ToLower())
            {
                // Metto in minuscolo, accetto solo numeri, spazio e lettere (inclusa qualche accentata)
                if (" 1234567890-/abcdefghijklmnopqrstuvwxyzàèéìòù".Contains(C.ToString()))
                {
                    switch (C)
                    {
                        case '/':
                        case ' ':           // Spazio diventa '-'
                            Ret += "-";
                            break;
                        case 'à':           // Lettere accentate, le dis-accento
                            Ret += 'a';
                            break;
                        case 'è':
                        case 'é':
                            Ret += 'e';
                            break;
                        case 'ì':
                            Ret += 'i';
                            break;
                        case 'ò':
                            Ret += 'o';
                            break;
                        case 'ù':
                            Ret += 'u';
                            break;
                        default:            // Tutte le altre lettere valide: così come sono
                            Ret += C;
                            break;
                    }
                }
            }

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[-]{2,}", options);
            Ret = regex.Replace(Ret, " ");

            return Ret;
        }

        private static Random Rnd = new Random();

        /// <summary>
        /// Crea una stringa alfanumerica casuale
        /// </summary>
        /// <param name="MinLenght">Lunghezza minima della stringa</param>
        /// <param name="MaxLenght">Lunghezza massima della stringa</param>
        /// <returns></returns>
        public static string CreateRandomString(int MinLenght, int MaxLenght)
        {
            string Ret = "";

            for (int StrLen = Rnd.Next(MinLenght, MaxLenght + 1); StrLen > 0; StrLen--)
                switch (Rnd.Next(3)) // 0..2
                {
                    case 0:
                        Ret += (char)('A' + (char)Rnd.Next(26));
                        break;
                    case 1:
                        Ret += (char)('a' + (char)Rnd.Next(26));
                        break;
                    case 2:
                        Ret += (char)('0' + (char)Rnd.Next(10));
                        break;
                }

            return Ret;
        }

        /// <summary>
        /// Crea una stringa alfanumerica casuale
        /// </summary>
        /// <param name="Lenght">Lunghezza fissa della stringa</param>
        /// <returns></returns>
        public static string CreateRandomString(int Lenght)
        {
            return CreateRandomString(Lenght, Lenght);
        }

        public static string EncodeTo64(this string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFrom64(this string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public const string ENCRYPT_KEY = "f3ioqwgv59o3c5g";

        public static string GetMd5Hash(this string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Encrypt the given string using the specified key.
        /// </summary>
        /// <param name="strToEncrypt">The string to be encrypted.</param>
        /// <param name="strKey">The encryption key.</param>
        /// <returns>The encrypted string.</returns>
        public static string Encrypt(this string strToEncrypt, string strKey)
        {
            try
            {
                TripleDESCryptoServiceProvider objDESCrypto =
                    new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey ?? ENCRYPT_KEY;
                byteHash = objHashMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = UTF8Encoding.UTF8.GetBytes(strToEncrypt);
                return Convert.ToBase64String(
                    objDESCrypto.CreateEncryptor().
                    TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (System.Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }

        public static string Encrypt(this string strToEncrypt)
        {
            return strToEncrypt.Encrypt(null);
        }

        /// <summary>
        /// Decrypt the given string using the specified key.
        /// </summary>
        /// <param name="strEncrypted">The string to be decrypted.</param>
        /// <param name="strKey">The decryption key.</param>
        /// <returns>The decrypted string.</returns>
        public static string Decrypt(this string strEncrypted, string strKey)
        {
            try
            {
                TripleDESCryptoServiceProvider objDESCrypto =
                    new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
                byte[] byteHash, byteBuff;
                string strTempKey = strKey ?? ENCRYPT_KEY;
                byteHash = objHashMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
                byteBuff = Convert.FromBase64String(strEncrypted);
                string strDecrypted = UTF8Encoding.UTF8.GetString(
                    objDESCrypto.CreateDecryptor().TransformFinalBlock
                    (byteBuff, 0, byteBuff.Length));
                objDESCrypto = null;
                return strDecrypted;
            }
            catch (System.Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }

        public static string Decrypt(this string strEncrypted)
        {
            return strEncrypted.Decrypt(null);
        }

    }
}
