using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Utils
{
    public static class StringUtils
    {
        private static readonly Dictionary<char, string> _mapping = new Dictionary<char, string>()
        {
            {  'ä', "ae" },
            {  'Ä', "AE" },
            {  'ö', "oe" },
            {  'Ö', "OE" },
            {  'ü', "ue" },
            {  'Ü', "UE" },
            {  'ß', "ss" }
        };

        public static string ConvertUmlaute(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            string normalizedString = value.Normalize();

            foreach (KeyValuePair<char, string> item in _mapping)
            {
                string temp = normalizedString;
                normalizedString = temp.Replace(item.Key.ToString(), item.Value);
            }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                normalizedString = normalizedString.Normalize(NormalizationForm.FormD);
                string c = normalizedString[i].ToString();
                if (CharUnicodeInfo.GetUnicodeCategory(Convert.ToChar(c)) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString();

        }
    }
}
