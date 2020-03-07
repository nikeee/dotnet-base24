using System;
using Xunit;
using System.Collections.Generic;

namespace Base24.Tests
{
    public class Encode
    {
        [Fact]
        public void ExpectArgumentNullExceptionDecode()
        {
            Assert.Throws<ArgumentNullException>(() => Base24Encoding.Decode(null));
        }

        [Theory]
        [InlineData(new byte[] { 1 })]
        [InlineData(new byte[] { 1, 2 })]
        [InlineData(new byte[] { 1, 2, 3 })]
        [InlineData(new byte[] { 1, 2, 3, 4, 5 })]
        public void InvalidDataLengthForEncoding(byte[] data)
        {
            Assert.Throws<ArgumentException>(() => Base24Encoding.Encode(data));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        [InlineData("123456")]
        [InlineData("12345678")]
        public void InvalidDataLengthForDecoding(string data)
        {
            Assert.Throws<ArgumentException>(() => Base24Encoding.Decode(data));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void TestEncoding(string dataHex, string encodedData)
        {
            Assert.NotNull(dataHex);
            Assert.NotNull(encodedData);

            var expectedEncoding = encodedData;

            var data = dataHex.AsHexStringToByteArray();
            Assert.NotNull(data);

            Console.WriteLine($"Testing {BitConverter.ToString(data)}");

            var actualEncoding = Base24Encoding.Encode(data);

            Assert.Equal(expectedEncoding, actualEncoding);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void TestDecoding(string dataHex, string encodedData)
        {
            Assert.NotNull(dataHex);
            Assert.NotNull(encodedData);

            var expectedData = dataHex.AsHexStringToByteArray();
            Assert.NotNull(expectedData);

            Console.WriteLine($"Testing {encodedData}");

            var actualData = Base24Encoding.Decode(encodedData);

            Assert.Equal(expectedData, actualData);
        }

        public static IEnumerable<object[]> Data => new List<object[]>()
        {
            new object[] {"00000000", "ZZZZZZZ"},
            new object[] {"000000000000000000000000", "ZZZZZZZZZZZZZZZZZZZZZ"},
            new object[] {"000000010000000100000001", "ZZZZZZAZZZZZZAZZZZZZA"},
            new object[] {"00000030", "ZZZZZCZ"},
            new object[] {"88553311", "5YEATXA"},
            new object[] {"FFFFFFFF", "X5GGBH7"},
            new object[] {"FFFFFFFFFFFFFFFFFFFFFFFF", "X5GGBH7X5GGBH7X5GGBH7"},
            new object[] {"1234567887654321", "A64KHWZ5WEPAGG"},
            new object[] {"FF0001FF001101FF01023399", "XGES63FZZ247C7ZC2ZA6G"},
            new object[] {"25896984125478546598563251452658", "2FC28KTA66WRST4XAHRRCF237S8Z"},
            new object[] {"00000001", "ZZZZZZA"},
            new object[] {"00000002", "ZZZZZZC"},
            new object[] {"00000004", "ZZZZZZB"},
            new object[] {"00000008", "ZZZZZZ4"},
            new object[] {"00000010", "ZZZZZZP"},
            new object[] {"00000020", "ZZZZZA4"},
            new object[] {"00000040", "ZZZZZCP"},
            new object[] {"00000080", "ZZZZZ34"},
            new object[] {"00000100", "ZZZZZHP"},
            new object[] {"00000200", "ZZZZZW4"},
            new object[] {"00000400", "ZZZZARP"},
            new object[] {"00000800", "ZZZZ2K4"},
            new object[] {"00001000", "ZZZZFCP"},
            new object[] {"00002000", "ZZZZ634"},
            new object[] {"00004000", "ZZZABHP"},
            new object[] {"00008000", "ZZZC4W4"},
            new object[] {"00010000", "ZZZB8RP"},
            new object[] {"00020000", "ZZZG5K4"},
            new object[] {"00040000", "ZZZRYCP"},
            new object[] {"00080000", "ZZAKX34"},
            new object[] {"00100000", "ZZ229HP"},
            new object[] {"00200000", "ZZEFPW4"},
            new object[] {"00400000", "ZZT7GRP"},
            new object[] {"00800000", "ZAAESK4"},
            new object[] {"01000000", "ZCCK7CP"},
            new object[] {"02000000", "ZB32E34"},
            new object[] {"04000000", "Z4HETHP"},
            new object[] {"08000000", "ZP9KZW4"},
            new object[] {"10000000", "AG8CARP"},
            new object[] {"20000000", "CSHB2K4"},
            new object[] {"40000000", "3694FCP"},
            new object[] {"80000000", "53PP634"},
        };
    }
}
