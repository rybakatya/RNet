using RapidNetworkLibrary;
using RapidNetworkLibrary.Connections;
using UnityEngine;

public class NetworkHandler : MonoBehaviour
{
    private void Start()
    {
        RNet.Init(onInit);
    }

    private void onInit()
    {
#if SERVER
        RNet.RegisterOnSocketConnectEvent(socketConnectLogicAction: LogicOnSocketConnect);
        RNet.InitializeServer("127.0.0.1", 7777, 255, 1024);
#elif  CLIENT
        RNet.InitializeClient(255);
            RNet.Connect("127.0.0.1", 7777);
#endif
    }

    private void LogicOnSocketConnect(Connection connection)
    {
        Debug.Log("[Logic Thread]: A connection has been formed between two sockets!");
    }

    private void Update()
    {
        RNet.Tick();
    }
}