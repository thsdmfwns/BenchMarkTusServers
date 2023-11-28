using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TusSharp;

namespace BenchMarkTusServer;

[SimpleJob(RuntimeMoniker.Net70)]
public class BenchMarkTus
{
    [Params("testfile1.zip", "testfile2.png", "testfile3.mp4", "testfile4.txt")]
    public string FileName = "testfile1.zip";
    
    [Benchmark]
    public async Task SendByDotnet() => await SendFile(Path.Combine("/TestFiles", FileName), "http://bm_dotnet:80/files");
    [Benchmark]
    public async Task SendByDotnetAOT() => await SendFile(Path.Combine("/TestFiles", FileName), "http://bm_dotnet_aot:8080/files");
    [Benchmark]
    public async Task SendByGo() => await SendFile(Path.Combine("/TestFiles", FileName), "http://bm_golang:8080/files");

    [IterationSetup]
    public void SetUp()
    {
        var path = "/files";
        var dir = new DirectoryInfo(path);
        dir.GetFiles().ToList().ForEach(x => x.Delete());
    }
    
    public async Task SendFile(string filePath, string url)
    {
        var client = new TusClient();
        await using var stream = File.OpenRead(filePath);
        var opt = new TusUploadOption()
        {
            EndPoint = new Uri(url),
            ChunkSize = 1 * 1024 * 1024, //1MB
        };
        using var upload = client.Upload(opt, stream);
        await upload.Start();
    }
}