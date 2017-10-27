using Xunit;
using System;
using System.Collections.Generic;

namespace csMACnz.SeaOrDew.Tests
{
    public class CommandErrorTests
    {
        [Theory]
        [InlineData("Failed.")]
        [InlineData("A Message with spaces")]
        [InlineData(@"A Really long message that has multiple sentences. It also has:
* Formatting
* Bulleted lists
* 😉 emoji characters

* Blank lines
* Trailing whitespace    
")]
        public void StringConstructor_CorrectDefaultValues(string input)
        {
            var error = new CommandError(input);

            Assert.Equal(input, error.Message);
            Assert.Equal(500, error.ErrorCode);
            Assert.Null(error.Ex);
        }

        [Theory]
        [MemberData(nameof(NullOrEmptyStrings))]
        public void StringConstructor_NullOrWhitespaceMessageFails(string input)
        {
            Assert.Throws<ArgumentException>(() => new CommandError(input));
        }

        [Theory]
        [MemberData(nameof(NullOrEmptyStrings))]
        public void CodeStringConstructor_NullOrWhitespaceMessageFails(string input)
        {
            Assert.Throws<ArgumentException>(() => new CommandError(42, input));
        }

        [Theory]
        [MemberData(nameof(NullOrEmptyStrings))]
        public void StringExceptionConstructor_NullOrWhitespaceMessageFails(string input)
        {
            Assert.Throws<ArgumentException>(() => new CommandError(input, new Exception()));
        }

        [Theory]
        [MemberData(nameof(NullOrEmptyStrings))]
        public void CodeStringExceptionConstructor_NullOrWhitespaceMessageFails(string input)
        {
            Assert.Throws<ArgumentException>(() => new CommandError(42, input, new Exception()));
        }

        [Theory]
        [InlineData(1, "Failed.")]
        [InlineData(-1, "Failed.")]
        [InlineData(1024, "Failed.")]
        [InlineData(int.MinValue, "Failed.")]
        [InlineData(int.MaxValue, "Failed.")]
        [InlineData(42, "A Message with spaces")]
        [InlineData(412, @"A Really long message that has multiple sentences. It also has:
* Formatting
* Bulleted lists
* 😉 emoji characters

* Blank lines
* Trailing whitespace    
")]
        public void CodeStringConstructor_CorrectValuesSet(int code, string message)
        {
            var error = new CommandError(code, message);

            Assert.Equal(message, error.Message);
            Assert.Equal(code, error.ErrorCode);
            Assert.Null(error.Ex);
        }

        [Fact]
        public void StringExceptionConstructor_nullException_ExceptionSet()
        {
            var message = "Failed.";
            Exception exception = null;
            var error = new CommandError(message, exception);

            Assert.Equal(message, error.Message);
            Assert.Null(error.Ex);
        }

        [Fact]
        public void StringExceptionConstructor_newException_ExceptionSet()
        {
            var message = "Failed.";
            var exception = new Exception();
            var error = new CommandError(message, exception);

            Assert.Equal(message, error.Message);
            Assert.Equal(exception, error.Ex);
        }

        [Fact]
        public void StringExceptionConstructor_CustomException_ExceptionSet()
        {
            var message = "Failed.";
            var exception = new CustomException();
            var error = new CommandError(message, exception);

            Assert.Equal(message, error.Message);
            Assert.Equal(exception, error.Ex);
        }

        [Fact]
        public void CodeStringExceptionConstructor_nullException_ExceptionSet()
        {
            var message = "Failed.";
            var code = 42;
            Exception exception = null;
            var error = new CommandError(code, message, exception);

            Assert.Equal(message, error.Message);
            Assert.Equal(code, error.ErrorCode);
            Assert.Null(error.Ex);
        }

        [Fact]
        public void CodeStringExceptionConstructor_newException_ExceptionSet()
        {
            var message = "Failed.";
            var code = 42;
            var exception = new Exception();
            var error = new CommandError(code, message, exception);

            Assert.Equal(message, error.Message);
            Assert.Equal(code, error.ErrorCode);
            Assert.Equal(exception, error.Ex);
        }

        [Fact]
        public void CodeStringExceptionConstructor_CustomException_ExceptionSet()
        {
            var message = "Failed.";
            var code = 42;
            var exception = new CustomException();
            var error = new CommandError(code, message, exception);

            Assert.Equal(message, error.Message);
            Assert.Equal(code, error.ErrorCode);
            Assert.Equal(exception, error.Ex);
        }

        private class CustomException : Exception { }

        public static IEnumerable<object[]> NullOrEmptyStrings => new List<object[]>{
            new[]{(string)null},
            new[]{""},
            new[]{"\t"},
            new[]{"\n"},
            new[]{" "},
            new[]{"         "},
        };
    }
}