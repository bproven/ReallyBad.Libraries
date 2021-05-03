using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

using ReallyBad.Core.Text;

namespace ReallyBad.Common.Test.Text
{

    public class StringExtensionsTests
    {

        [Fact]
        public void TrimStartTest()
        {
            Assert.Equal( "test", "trimtest".TrimStart( "trim" )  );
        }

        [Fact]
        public void TrimEndTest()
        {
            Assert.Equal( "test", "testtrim".TrimEnd( "trim" ) );
        }

    }

}
