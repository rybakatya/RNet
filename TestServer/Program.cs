using RapidNetworkLibrary;
using RapidNetworkLibrary.Connections;



internal class Program
{
    private static bool isInit;
    private static void Main(string[] args)
    {
#if SERVER
        Console.WriteLine("Hello, World!");

        RNet.Init(onInit);
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
       
        RNet.RegisterReceiveEvent(gameReceiveAction: OnNetworkMessage);


        RNet.RegisterOnSocketConnect(LogicOnServerConnect, GameOnServerConnect);

        RNet.InitializeServer("127.0.0.1", 7777, 255, 2048);

        isInit = true;
    }

    private static void GameOnServerConnect(Connection connection)
    {
        Console.WriteLine("[Game Thread]: A server has connected to the server!");
    }

    private static void LogicOnServerConnect(Connection connection)
    {
        Console.WriteLine("[Logic Thread]: A server has connected to the server!");
    }

    private static void GameOnClientConnect(Connection connection)
    {
        Console.WriteLine("[Game Thread]: A client has connected to the server!");
    }

    private static void LogicOnClientConnect(Connection connection)
    {
        Console.WriteLine("[Logic Thread]: A client has connected to the server!");
    }

    private static void OnNetworkMessage(Connection connection, ushort messageID, IntPtr msg)
    {
        Console.WriteLine("A client has sent message id " + messageID);
    }
#endif
}