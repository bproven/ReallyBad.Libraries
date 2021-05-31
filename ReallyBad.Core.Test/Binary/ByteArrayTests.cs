// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core.Test
//     File:       ByteArrayTests.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 11:00 PM
// 

using System;
using System.Text;

using ReallyBad.Core.Binary;

using Xunit;

// ReSharper disable ExpressionIsAlwaysNull

namespace ReallyBad.Core.Test.Binary
{

    public class ByteArrayTests
    {

        [Fact]
        public void ByteArrayValueEqualsTest()
        {
            byte[] array1 = { 0, 1, 2, 3, };
            byte[] array2 = { 0, 1, 2, 3, };
            Assert.True( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayValueNotEqualsTest()
        {
            byte[] array1 = { 0, 1, 2, 3, };
            byte[] array2 = { 0, 1, 1, 3, };
            Assert.False( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayLengthNotEqualsTest1()
        {
            byte[] array1 = { 0, 1, 2, 3, };
            byte[] array2 = { 0, 1, 2, };
            Assert.False( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayLengthNotEqualsTest2()
        {
            byte[] array1 = { 0, 1, 2, };
            byte[] array2 = { 0, 1, 2, 3, };
            Assert.False( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayRefEqualsTest()
        {
            byte[] array1 = { 0, 1, 2, 3, };
            var array2 = array1;
            Assert.True( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayNullEqualsTest()
        {
            byte[] array1 = null;
            byte[] array2 = null;
            Assert.True( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayOneNullNotEqualsTest1()
        {
            byte[] array1 = { 0, 1, 2, 3, };
            byte[] array2 = null;
            Assert.False( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayOneNullNotEqualsTest2()
        {
            byte[] array1 = null;
            byte[] array2 = { 0, 1, 2, 3, };
            Assert.False( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayEmptyEqualsTest()
        {
            byte[] array1 = { };
            byte[] array2 = { };
            Assert.True( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayOneEmptyNotEqualsTest1()
        {
            byte[] array1 = { 0, 1, 2, 3, };
            byte[] array2 = { };
            Assert.False( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void ByteArrayOneEmptyNotEqualsTest2()
        {
            byte[] array1 = { };
            byte[] array2 = { 0, 1, 2, 3, };
            Assert.False( array1.ValueEquals( array2 ) );
        }

        [Fact]
        public void UTF8EncodeDecodeTest()
        {
            var expected = "test";
            var encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes( expected );
            var result = bytes.GetEncoded( encoding );
            Assert.Equal( expected, result );
        }

        [Fact]
        public void Base64EncodeDecodeTest()
        {
            var value = "test";
            var encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes( value );
            var base64 = bytes.GetBase64();
            var bytes2 = Convert.FromBase64String( base64 );
            var result = bytes2.GetBase64();
            Assert.Equal( base64, result );
        }

    }

}
