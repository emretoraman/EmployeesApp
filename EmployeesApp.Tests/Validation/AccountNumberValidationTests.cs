using EmployeesApp.Validation;
using System;
using Xunit;

namespace EmployeesApp.Tests.Validation
{
	public class AccountNumberValidationTests
	{
		private readonly AccountNumberValidation _validation;

		public AccountNumberValidationTests()
		{
			_validation = new AccountNumberValidation();
		}

		[Fact]
		public void IsValid_ValidAccountNumber_ReturnsTrue()
		{
			Assert.True(_validation.IsValid("123-4543234576-23"));
		}

		[Theory]
		[InlineData("1234-4543234576-23")]
		[InlineData("12-4543234576-23")]
		public void IsValid_AccountNumberFirstPartWrong_ReturnsFalse(string accountNumber)
		{
			Assert.False(_validation.IsValid(accountNumber));
		}

		[Theory]
		[InlineData("123-45432345767-23")]
		[InlineData("123-454323457-23")]
		public void IsValid_AccountNumberMiddlePartWrong_ReturnsFalse(string accountNumber)
		{
			Assert.False(_validation.IsValid(accountNumber));
		}

		[Theory]
		[InlineData("123-4543234576-234")]
		[InlineData("123-4543234576-2")]
		public void IsValid_AccountNumberLastPartWrong_ReturnsFalse(string accountNumber)
		{
			Assert.False(_validation.IsValid(accountNumber));
		}

		[Theory]
		[InlineData("123-345456567633=23")]
		[InlineData("123+345456567633-23")]
		[InlineData("123+345456567633=23")]
		public void IsValid_InvalidDelimiters_ThrowsArgumentException(string accountNumber)
		{
			Assert.Throws<ArgumentException>(() => _validation.IsValid(accountNumber));
		}

	}
}
