using Ipfs.Http;
using OwlCore.Kubo;
using OwlCore.Storage;
using OwlCore.Storage.SystemIO;

public class Program
{
    public static async Task Main()
    {
        var ipfsClient = await GetIpfsClient();
        
        var pubsub =  ipfsClient.PubSub;
        var thisPeer = await ipfsClient.IdAsync();
        
        var peerRoom = new PeerRoom(thisPeer, pubsub, topicName: "sample");
        peerRoom.MessageReceived += (s, e) =>
        {
            Console.WriteLine($"{e.Id}: {new StreamReader(e.DataStream).ReadToEnd()}");
        };
        
        while (true)
        {
            Console.Write("> ");
            var msg = Console.ReadLine();
            await peerRoom.PublishAsync(msg);
        }
    }

    private static async Task<IpfsClient> GetIpfsClient()
    {
        IFile kuboBinary = await GetKuboBinary();

        using var bootstrapper = new KuboBootstrapper(kuboBinary, @"C:\Users\c.anders\AppData\Local\Temp\06bd1567-ae38-4cc2-b1c9-a8b0a78ff97a")
        {
            ApiUri = new Uri("http://127.0.0.1:8080"),
        };
        
        await bootstrapper.StartAsync();

        return new IpfsClient(bootstrapper.ApiUri.ToString());
    }

    private static async Task<IFile> GetKuboBinary()
    {
        var downloader = new KuboDownloader();
        //return await downloader.DownloadLatestBinaryAsync();

        return new SystemFile(
            @"C:\Users\c.anders\AppData\Local\Temp\6f8580fa-ce91-4612-be13-4f66df8bb697\kubo\ipfs.exe");
    }
}