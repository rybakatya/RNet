using RapidNetworkLibrary;
using RapidNetworkLibrary.Connections;

internal class Program
{
    private static void Main(string[] args)
    {
        RNet.Init(onInit);

        while (true)
        {
            if (isInit == false)
                continue;

            RNet.Tick();
        }
    }
    private static bool isInit;
    private static void onInit()
    {
#if CLIENT
        RNet.InitializeClient(255);
        RNet.RegisterOnConnectedToServerEvent(LogicOnConnectedToServer, GameOnConnectedToServer);
#endif

            RNet.Connect("127.0.0.1", 7777);

        isInit = true;
    }

    private static void GameOnConnectedToServer(Connection connection)
    {
        Console.WriteLine("[Game Thread]: connected to a server!");
    }

    private static void LogicOnConnectedToServer(Connection connection)
    {
        Console.WriteLine("[Logic Thread]: connected to the server!");
    }
}