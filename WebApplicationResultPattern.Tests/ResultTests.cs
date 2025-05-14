namespace WebApplicationResultPattern.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Success_ShouldSetIsSuccessToTrue_AndExposeValue()
        {
            var result = Result<string, string>.Success("Operation successful");

            Assert.True(result.IsSuccess);
            Assert.Equal("Operation successful", result.Value);
            Assert.Throws<Exception>(() => { var _ = result.Error; });
        }

        [Fact]
        public void Failure_ShouldSetIsSuccessToFalse_AndExposeError()
        {
            var result = Result<string, string>.Failure("Something went wrong");

            Assert.False(result.IsSuccess);
            Assert.Equal("Something went wrong", result.Error);
            Assert.Throws<Exception>(() => { var _ = result.Value; });
        }

        [Fact]
        public void Success_WithNullValue_ShouldStillBeSuccess()
        {
            var result = Result<string?, string>.Success(null);

            Assert.True(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Failure_WithNullError_ShouldStillBeFailure()
        {
            var result = Result<string, string?>.Failure(null);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Error);
        }

        [Fact]
        public void AccessingValue_OnFailure_ShouldThrow()
        {
            var result = Result<string, string>.Failure("fail");

            var ex = Assert.Throws<Exception>(() => _ = result.Value);
            Assert.Equal("Result is not a Success.", ex.Message);
        }

        [Fact]
        public void AccessingError_OnSuccess_ShouldThrow()
        {
            var result = Result<string, string>.Success("ok");

            var ex = Assert.Throws<Exception>(() => _ = result.Error);
            Assert.Equal("Result is a Success.", ex.Message);
        }
    }
}
