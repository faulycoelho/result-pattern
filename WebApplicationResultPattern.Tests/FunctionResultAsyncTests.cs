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

        [Fact]
        public async Task MapAsync_TaskResultOfSuccess_ShouldMapValue()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Success(10));

            var mapped = await resultTask.MapAsync(async i =>
            {
                await Task.Delay(10);
                return i * 2;
            });

            Assert.True(mapped.IsSuccess);
            Assert.Equal(20, mapped.Value);
        }

        [Fact]
        public async Task MapAsync_TaskResultOfFailure_ShouldPreserveError()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Failure("Erro"));

            var mapped = await resultTask.MapAsync(async i =>
            {
                await Task.Delay(10);
                return i * 2;
            });

            Assert.False(mapped.IsSuccess);
            Assert.Equal("Erro", mapped.Error);
        }

        [Fact]
        public async Task BindAsync_TaskResultOfSuccess_ShouldReturnNewResult()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Success(5));

            var bound = await resultTask.BindAsync(async i =>
            {
                await Task.Delay(10);
                return Result<string, string>.Success($"Valor: {i}");
            });

            Assert.True(bound.IsSuccess);
            Assert.Equal("Valor: 5", bound.Value);
        }

        [Fact]
        public async Task BindAsync_TaskResultOfFailure_ShouldReturnSameError()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Failure("Erro original"));

            var bound = await resultTask.BindAsync(async i =>
            {
                await Task.Delay(10);
                return Result<string, string>.Success($"Valor: {i}");
            });

            Assert.False(bound.IsSuccess);
            Assert.Equal("Erro original", bound.Error);
        }

        [Fact]
        public async Task MapErrorAsync_TaskResultOfFailure_ShouldMapError()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Failure("ERRO"));

            var mappedError = await resultTask.MapErrorAsync(async err =>
            {
                await Task.Delay(10);
                return $"Mapped: {err}";
            });

            Assert.False(mappedError.IsSuccess);
            Assert.Equal("Mapped: ERRO", mappedError.Error);
        }

        [Fact]
        public async Task MapErrorAsync_TaskResultOfSuccess_ShouldPreserveValue()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Success(99));

            var mappedError = await resultTask.MapErrorAsync(async err =>
            {
                await Task.Delay(10);
                return $"Mapped: {err}";
            });

            Assert.True(mappedError.IsSuccess);
            Assert.Equal(99, mappedError.Value);
        }

        [Fact]
        public async Task MatchAsync_TaskResultOfSuccess_ExecutesSuccessAsyncBranch()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Success(7));

            var output = await resultTask.MatchAsync(
                async value =>
                {
                    await Task.Delay(10);
                    return $"Sucesso: {value}";
                },
                error => $"Erro: {error}"
            );

            Assert.Equal("Sucesso: 7", output);
        }

        [Fact]
        public async Task MatchAsync_TaskResultOfFailure_ExecutesFailureSyncBranch()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Failure("Falha"));

            var output = await resultTask.MatchAsync(
                async value =>
                {
                    await Task.Delay(10);
                    return $"Sucesso: {value}";
                },
                error => $"Erro: {error}"
            );

            Assert.Equal("Erro: Falha", output);
        }

        [Fact]
        public async Task MatchAsync_TaskResultOfSuccess_ExecutesSuccessSyncBranch()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Success(42));

            var output = await resultTask.MatchAsync(
                value => $"Sync Sucesso: {value}",
                async error =>
                {
                    await Task.Delay(10);
                    return $"Async Erro: {error}";
                });

            Assert.Equal("Sync Sucesso: 42", output);
        }

        [Fact]
        public async Task MatchAsync_TaskResultOfFailure_ExecutesFailureAsyncBranch()
        {
            Task<Result<int, string>> resultTask = Task.FromResult(Result<int, string>.Failure("ErrorX"));

            var output = await resultTask.MatchAsync(
                value => $"Sync Sucesso: {value}",
                async error =>
                {
                    await Task.Delay(10);
                    return $"Async Erro: {error}";
                });

            Assert.Equal("Async Erro: ErrorX", output);
        }
    }

}
