// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core.Test
//     File:       ValidatorTests.cs
// 
//     Created:    05/02/2021 8:57 PM
//     Updated:    05/02/2021 9:17 PM
// 

using System;
using System.Collections.Generic;

using ReallyBad.Core.Validation;

using Xunit;

#nullable enable

namespace ReallyBad.Core.Test.Validation
{

	public class ValidatorTests
	{

		private const string Name = "parameter";

		[Fact]
		public void ValidateNullObjectEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => ArgumentValidator.ValidateNotEmpty( null, Name ) );
		}

		[Fact]
		public void ValidateObjectNotEmptyTest()
		{
			ArgumentValidator.ValidateNotEmpty( new object(), Name );
		}

		[Fact]
		public void ValidateEmptyStringEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => ArgumentValidator.ValidateNotEmpty( string.Empty, Name ) );
		}

		[Fact]
		public void ValidateStringNotEmptyTest()
		{
			ArgumentValidator.ValidateNotEmpty( "a string", Name );
		}

		[Fact]
		public void ValidateEmptyListEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => ArgumentValidator.ValidateNotEmpty( new List<int>(), Name ) );
		}

		[Fact]
		public void ValidateListNotEmptyTest()
		{
			ArgumentValidator.ValidateNotEmpty( new List<int> { 1, }, Name );
		}

		[Fact]
		public void ValidateEmptyArrayEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => ArgumentValidator.ValidateNotEmpty( Array.Empty<float>(), Name ) );
		}

		[Fact]
		public void ValidateArrayNotEmptyTest()
		{
			ArgumentValidator.ValidateNotEmpty( new float[ 1 ], Name );
		}

		[Fact]
		public void ValidateEmptyGuidEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => ArgumentValidator.ValidateNotEmpty( Guid.Empty, Name ) );
		}

		[Fact]
		public void ValidateGuidNotEmptyTest()
		{
			ArgumentValidator.ValidateNotEmpty( Guid.NewGuid(), Name );
		}

		[Fact]
		public void ValidateEmptyDateEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => ArgumentValidator.ValidateNotEmpty( new DateTime(), Name ) );
		}

		[Fact]
		public void ValidateDateNotEmptyTest()
		{
			ArgumentValidator.ValidateNotEmpty( new DateTime( 2021, 5, 2 ), Name );
		}

	}

}
