using Ipfs;

public class Program
{
    public static async Task Main()
    {
        Environment.SetEnvironmentVariable("IPFS_PASS", "hdhcdidgegeb friwrugfwiguhwgwgwg");
        var engine = new Ipfs.Engine.IpfsEngine();
        
        await engine.StartAsync();
        
        engine.PubSub.SubscribeAsync("sample", handler, default);
        
        while (true)
        {
            Console.Write("> ");
            var msg = Console.ReadLine();
            await engine.PubSub.PublishAsync("sample", msg);
        }
    }

    static void handler(IPublishedMessage msg)
    {
        Console.WriteLine($"{msg.Sender.Id}: {new StreamReader(msg.DataStream).ReadToEnd()}");
    }
}