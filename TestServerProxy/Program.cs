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


#if SERVER
    private static void onInit()
    {
        isInit = true;
        

        RNet.RegisterGetConnectionTypeEvent(DetermineConnectionType);
        RNet.RegisterOnClientConnectEvent(clientConnectLogicAction: LogicOnClientConnect);
        RNet.RegisterOnServerConnectEvent(serverConnectLogicAction: LogicOnServerConnect);

        RNet.InitializeServer("127.0.0.1", 7778, 255, 2048);

        RNet.Connect("127.0.0.1", 7777);
        
    }

   

    private static void LogicOnServerConnect(Connection connection)
    {
        Console.WriteLine("Formed a connection with server");
    }

   
    private static void LogicOnClientConnect(Connection connection)
    {
        Console.WriteLine("Formed a connection with a client");
    }

    private static ConnectionType DetermineConnectionType(string ip, ushort port)
    {
        if(ip.Equals("127.0.0.1") && port == 7777)
            return ConnectionType.Server;
        return ConnectionType.Client;
    }
#endif
}

