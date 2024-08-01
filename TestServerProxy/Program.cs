using RapidNetworkLibrary;
using RapidNetworkLibrary.Connections;

internal class Program
{
    private static bool isInit;

    private static void Main(string[] args)
    {
#if SERVER
        Console.WriteLine("Hello, World!");
        RNet.Init(onInit, ConnectionType.Server);
        while (true)
        {
            if (isInit == false)
                continue;

            RNet.Tick();
        }
#endif
    }

    private static void onInit()
    {
        throw new NotImplementedException();
    }
}