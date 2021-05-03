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
			Assert.Throws<ArgumentNullException>( () => Validator.ValidateNotEmpty( null, Name ) );
		}

		[Fact]
		public void ValidateObjectNotEmptyTest()
		{
			Validator.ValidateNotEmpty( new object(), Name );
		}

		[Fact]
		public void ValidateEmptyStringEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => Validator.ValidateNotEmpty( string.Empty, Name ) );
		}

		[Fact]
		public void ValidateStringNotEmptyTest()
		{
			Validator.ValidateNotEmpty( "a string", Name );
		}

		[Fact]
		public void ValidateEmptyListEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => Validator.ValidateNotEmpty( new List<int>(), Name ) );
		}

		[Fact]
		public void ValidateListNotEmptyTest()
		{
			Validator.ValidateNotEmpty( new List<int> { 1, }, Name );
		}

		[Fact]
		public void ValidateEmptyArrayEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => Validator.ValidateNotEmpty( Array.Empty<float>(), Name ) );
		}

		[Fact]
		public void ValidateArrayNotEmptyTest()
		{
			Validator.ValidateNotEmpty( new float[ 1 ], Name );
		}

		[Fact]
		public void ValidateEmptyGuidEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => Validator.ValidateNotEmpty( Guid.Empty, Name ) );
		}

		[Fact]
		public void ValidateGuidNotEmptyTest()
		{
			Validator.ValidateNotEmpty( Guid.NewGuid(), Name );
		}

		[Fact]
		public void ValidateEmptyDateEmptyTest()
		{
			Assert.Throws<ArgumentNullException>( () => Validator.ValidateNotEmpty( new DateTime(), Name ) );
		}

		[Fact]
		public void ValidateDateNotEmptyTest()
		{
			Validator.ValidateNotEmpty( new DateTime( 2021, 5, 2 ), Name );
		}

	}

}
