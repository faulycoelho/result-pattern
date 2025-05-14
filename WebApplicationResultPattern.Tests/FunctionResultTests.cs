namespace WebApplicationResultPattern.Tests
{
    public class FunctionResultTests
    {
        [Fact]
        public void Map_ShouldTransformValue_WhenResultIsSuccess()
        {
            var result = Result<int, string>.Success(2);

            var mapped = result.Map(x => x * 10);

            Assert.True(mapped.IsSuccess);
            Assert.Equal(20, mapped.Value);
        }

        [Fact]
        public void Map_ShouldPreserveError_WhenResultIsFailure()
        {
            var result = Result<int, string>.Failure("fail");

            var mapped = result.Map(x => x * 10);

            Assert.False(mapped.IsSuccess);
            Assert.Equal("fail", mapped.Error);
        }

        [Fact]
        public void Bind_ShouldChain_WhenResultIsSuccess()
        {
            var result = Result<int, string>.Success(3);

            var bound = result.Bind(x => Result<string, string>.Success($"Value is {x}"));

            Assert.True(bound.IsSuccess);
            Assert.Equal("Value is 3", bound.Value);
        }

        [Fact]
        public void Bind_ShouldSkip_WhenResultIsFailure()
        {
            var result = Result<int, string>.Failure("something went wrong");

            var bound = result.Bind(x => Result<string, string>.Success($"Value is {x}"));

            Assert.False(bound.IsSuccess);
            Assert.Equal("something went wrong", bound.Error);
        }

        [Fact]
        public void MapError_ShouldTransformError_WhenResultIsFailure()
        {
            var result = Result<int, string>.Failure("NotFound");

            var mappedError = result.MapError(err => new { Message = err });

            Assert.False(mappedError.IsSuccess);
            Assert.Equal("NotFound", mappedError.Error.Message);
        }

        [Fact]
        public void MapError_ShouldPreserveValue_WhenResultIsSuccess()
        {
            var result = Result<int, string>.Success(42);

            var mappedError = result.MapError(err => new { Message = err });

            Assert.True(mappedError.IsSuccess);
            Assert.Equal(42, mappedError.Value);
        }

        [Fact]
        public void Match_ShouldReturnMappedValue_WhenResultIsSuccess()
        {
            var result = Result<int, string>.Success(5);

            var output = result.Match(
                value => $"Success: {value}",
                error => $"Error: {error}"
            );

            Assert.Equal("Success: 5", output);
        }

        [Fact]
        public void Match_ShouldReturnMappedError_WhenResultIsFailure()
        {
            var result = Result<int, string>.Failure("Oops");

            var output = result.Match(
                value => $"Success: {value}",
                error => $"Error: {error}"
            );

            Assert.Equal("Error: Oops", output);
        }
    }
}
