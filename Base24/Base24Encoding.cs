using System;
using System.Text;
using System.Collections.Generic;

namespace Base24
{
    public static class Base24Encoding
    {
        private const string _alphabet = "ZAC2B3EF4GH5TK67P8RS9WXY";
        private static readonly IReadOnlyDictionary<char, uint> _decodeMap = CreateDecodeMap();

        private static IReadOnlyDictionary<char, uint> CreateDecodeMap()
        {
            var res = new Dictionary<char, uint>();
            for (int i = 0; i < _alphabet.Length; ++i)
            {
                var normalizedChar = char.ToUpperInvariant(_alphabet[i]);
                res[normalizedChar] = (uint)i;
            }
            return res;
        }

        public static string Encode(ReadOnlySpan<byte> data)
        {
            if (data.Length == 0)
                return string.Empty;

            // The data length must be multiple of 32 bits. There is no padding mechanism in the encoder.
            if (data.Length % 4 != 0)
                throw new ArgumentException("The data length must be multiple of 4 bytes (32 bits).");

            var alphabetLength = (uint)_alphabet.Length;
            var encodedDataLength = (data.Length / 4) * 7; // Same as data.Length * 1.75 (but with integers only)

            StringBuilder sb = new StringBuilder(encodedDataLength);
            Span<char> subResult = stackalloc char[7];

            for (int i = 0; i < data.Length; i += 4)
            {
                // We're not sure about the endianess, so do this manually
                uint value =
                      (uint)(data[i + 0] << 24)
                    | (uint)(data[i + 1] << 16)
                    | (uint)(data[i + 2] << 8)
                    | (uint)(data[i + 3] << 0);

                subResult.Clear();

                for (int k = 6; k >= 0; --k)
                {
                    var idx = value % alphabetLength;
                    value /= alphabetLength;
                    subResult[k] = _alphabet[(int)idx];
                }

                sb.Append(subResult);
            }
            return sb.ToString();
        }

        public static byte[] Decode(in string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length % 7 != 0)
                throw new ArgumentException("The data length must be multiple of 7 chars.");

            var alphabetLength = (uint)_alphabet.Length;
            var decodedDataLength = (data.Length / 7) * 4;

            var charSpan = data.AsSpan();
            var result = new byte[decodedDataLength];

            for (int i = 0; i < charSpan.Length / 7; ++i)
            {
                var j = i * 7;

                var subData = charSpan.Slice(j, 7); // Maybe we can range-syntax ([1..2]) some day? Seems to be supported with constants only
                uint value = 0;

                foreach (var c in subData)
                {
                    if (!_decodeMap.TryGetValue(char.ToUpperInvariant(c), out uint index))
                        throw new FormatException($"Unsupported character in input: {c}");

                    value = alphabetLength * value + index;
                }

                var resultIndex = i * 4;
                result[resultIndex + 0] = (byte)(value >> 24);
                result[resultIndex + 1] = (byte)(value >> 16);
                result[resultIndex + 2] = (byte)(value >> 8);
                result[resultIndex + 3] = (byte)(value >> 0);
            }

            return result;
        }
    }
}
