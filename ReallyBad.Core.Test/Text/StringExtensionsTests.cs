using Xunit;

using ReallyBad.Core.Text;

namespace ReallyBad.Core.Test.Text
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
