namespace WebApplicationResultPattern.Tests
{
    public class FunctionResultAsyncTests
    {
        [Fact]
        public async Task MapAsync_WithTaskResultAndSyncFunc_ShouldMapValue()
        {
            var taskResult = Task.FromResult(Result<int, string>.Success(5));

            var result = await taskResult.MapAsync(x => x * 2);

            Assert.True(result.IsSuccess);
            Assert.Equal(10, result.Value);
        }

        [Fact]
        public async Task MapAsync_WithSyncResultAndAsyncFunc_ShouldMapValue()
        {
            var result = Result<int, string>.Success(3);

            var mapped = await result.MapAsync(async x => await Task.FromResult(x + 7));

            Assert.True(mapped.IsSuccess);
            Assert.Equal(10, mapped.Value);
        }

        [Fact]
        public async Task BindAsync_WithTaskResultAndSyncFunc_ShouldBind()
        {
            var taskResult = Task.FromResult(Result<int, string>.Success(4));

            var result = await taskResult.BindAsync(x => Result<string, string>.Success($"Value:{x}"));

            Assert.True(result.IsSuccess);
            Assert.Equal("Value:4", result.Value);
        }

        [Fact]
        public async Task BindAsync_WithSyncResultAndAsyncFunc_ShouldBind()
        {
            var result = Result<int, string>.Success(8);

            var bound = await result.BindAsync(async x => await Task.FromResult(Result<string, string>.Success($"Async:{x}")));

            Assert.True(bound.IsSuccess);
            Assert.Equal("Async:8", bound.Value);
        }

        [Fact]
        public async Task MapErrorAsync_WithTaskResultAndSyncFunc_ShouldMapError()
        {
            var taskResult = Task.FromResult(Result<int, string>.Failure("Fail"));

            var mapped = await taskResult.MapErrorAsync(e => $"Mapped-{e}");

            Assert.False(mapped.IsSuccess);
            Assert.Equal("Mapped-Fail", mapped.Error);
        }

        [Fact]
        public async Task MapErrorAsync_WithSyncResultAndAsyncFunc_ShouldMapError()
        {
            var result = Result<int, string>.Failure("Original");

            var mapped = await result.MapErrorAsync(async e => await Task.FromResult($"Async-{e}"));

            Assert.False(mapped.IsSuccess);
            Assert.Equal("Async-Original", mapped.Error);
        }

        [Fact]
        public async Task MatchAsync_WithAllSyncFuncs_ShouldReturnSuccessValue()
        {
            var taskResult = Task.FromResult(Result<string, string>.Success("hello"));

            var output = await taskResult.MatchAsync(
                value => $"ok:{value}",
                err => $"err:{err}");

            Assert.Equal("ok:hello", output);
        }

        [Fact]
        public async Task MatchAsync_WithAsyncSuccessFunc_ShouldReturnMappedValue()
        {
            var result = Result<int, string>.Success(99);

            var output = await result.MatchAsync(
                async val => await Task.FromResult($"Number:{val}"),
                err => "fail");

            Assert.Equal("Number:99", output);
        }

        [Fact]
        public async Task MatchAsync_WithAsyncFailureFunc_ShouldReturnMappedError()
        {
            var result = Result<int, string>.Failure("Oops");

            var output = await result.MatchAsync(
                val => $"Value:{val}",
                async err => await Task.FromResult($"Error:{err}"));

            Assert.Equal("Error:Oops", output);
        }

        [Fact]
        public async Task MatchAsync_WithAsyncBoth_ShouldReturnCorrectly()
        {
            var result = Result<int, string>.Success(123);

            var output = await result.MatchAsync(
                async val => await Task.FromResult($"Success:{val}"),
                async err => await Task.FromResult($"Failure:{err}"));

            Assert.Equal("Success:123", output);
        }

        [Fact]
        public async Task MatchAsync_WithTaskResultAndAsyncBoth_ShouldReturnCorrectly()
        {
            var taskResult = Task.FromResult(Result<int, string>.Failure("ErrValue"));

            var output = await taskResult.MatchAsync(
                async val => await Task.FromResult($"Success:{val}"),
                async err => await Task.FromResult($"Failure:{err}"));

            Assert.Equal("Failure:ErrValue", output);
        }
    }

}
