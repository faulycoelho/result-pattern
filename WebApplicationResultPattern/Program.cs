using WebApplicationResultPattern;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/test", async () =>
{
    var result = await DoSomething()
    .BindAsync(tst => DoSomethingTwo())
    .BindAsync(tst => DoSomethingThree())
    .BindAsync(tst => DoSomethingThree())
    .MapAsync(tst => DoSomethingFour())
    .BindAsync(tst => DoSomethingFive())
    .MatchAsync(
        success => Results.Ok(success),
        error => Results.BadRequest(error)
        );

    return result;
});

app.Run();

async Task<Result<string, string>> DoSomething()
{
    await Task.Delay(100);

    var rs = Result<string, string>.Success("success!!");
    return rs;
}
Result<string, string> DoSomethingTwo()
{
    //await Task.Delay(100);

    var rs = Result<string, string>.Success("success Two!!");
    return rs;
}

async Task<Result<string, string>> DoSomethingThree()
{
    await Task.Delay(100);

    var rs = Result<string, string>.Success("success three!!");
    return rs;
}

async Task<int> DoSomethingFour()
{
    await Task.Delay(100);
    var rs = 10_444;
    return rs;
}

async Task<Result<int, string>> DoSomethingFive()
{
    await Task.Delay(100);
    var rs = Result<int, string>.Success(10_555);
    Console.WriteLine(rs.Value);
    return rs;
}