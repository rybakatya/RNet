using RapidNetworkLibrary;
using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Serialization;


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
        isInit = true;
        

      
        RNet.RegisterOnSocketConnect(socketConnectLogicAction: LogicOnServerConnect);

        RNet.InitializeServer("127.0.0.1", 7778, 255, 2048);

        RNet.Connect("127.0.0.1", 7777);
        
    }

   

    private static void LogicOnServerConnect(Connection connection)
    {
        Console.WriteLine("Formed a connection with server");
    }

    
    private static void LogicOnClientConnect(Connection connection)
    {

        Console.WriteLine("Formed a connection with a client " + connection.IpAddress + ":" + connection.Port);
    }

   
#endif
}

