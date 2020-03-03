using System;

namespace Base24.Tests
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Taken from: https://stackoverflow.com/a/311179
        /// Performance does not matter here, since it is only used in tests.
        /// </summary>
        public static byte[] AsHexStringToByteArray(this string hexadecimalString)
        {
            int NumberChars = hexadecimalString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hexadecimalString.Substring(i, 2), 16);
            return bytes;
        }
    }
}
