using tusdotnet;
using tusdotnet.Models.Configuration;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddCors();

var app = builder.Build();
app.Use(async (httpContext, next) =>
{
    // Specify timeout, in this case 60 seconds. 
    // If running behind a reverse proxy make sure this value matches the request timeout in the proxy.
    var requestTimeout = TimeSpan.FromSeconds(60);

    // Add timeout to the current request cancellation token. 
    // If the client does a clean disconnect the cancellation token will also be flagged as cancelled.
    using var timoutCts = CancellationTokenSource.CreateLinkedTokenSource(httpContext.RequestAborted);

    // Make sure to cancel the cancellation token after the timeout. 
    // Once this timeout has been reached, tusdotnet will cancel all pending reads 
    // from the client and save the parts of the file has been received so far.
    timoutCts.CancelAfter(requestTimeout);

    // Replace the request cancellation token with our token that supports timeouts.
    httpContext.RequestAborted = timoutCts.Token;

    // Continue the execution chain.
    await next();
});

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders(tusdotnet.Helpers.CorsHelper.GetExposedHeaders())
);

Directory.CreateDirectory("/tusfiles");

app.MapTus("/files", async httpContext => new()
{
    // This method is called on each request so different configurations can be returned per user, domain, path etc.
    // Return null to disable tusdotnet for the current request.

    // Where to store data?
    Store = new tusdotnet.Stores.TusDiskStore("/tusfiles"),
    Events = new Events
    {
        // What to do when file is completely uploaded?
        OnFileCompleteAsync = ctx =>
        {
            var fileName = ctx.FileId;
            Console.WriteLine($"{fileName} has been completed");
            return Task.CompletedTask;
        }
    }
});
app.Run();