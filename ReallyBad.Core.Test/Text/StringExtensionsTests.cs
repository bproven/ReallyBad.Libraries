// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core.Test
//     File:       StringExtensionsTests.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 11:03 PM
// 

using ReallyBad.Core.Text;

using Xunit;

namespace ReallyBad.Core.Test.Text
{

    public class StringExtensionsTests
    {

        [Fact]
        public void TrimStartTest()
        {
            Assert.Equal( "test", "trimtest".TrimStart( "trim" ) );
        }

        [Fact]
        public void TrimEndTest()
        {
            Assert.Equal( "test", "testtrim".TrimEnd( "trim" ) );
        }

    }

}
