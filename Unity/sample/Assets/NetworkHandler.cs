using RapidNetworkLibrary;
using RapidNetworkLibrary.Connections;
using RapidNetworkLibrary.Memory;
using System;
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
        RNet.RegisterReceiveEvent(logicReceiveAction: LogicOnMessageReceived);
        RNet.InitializeClient(255);
            RNet.Connect("127.0.0.1", 7777);
#endif
    }

    private void LogicOnMessageReceived(Connection connection, ushort messageID, IntPtr messageData)
    {
        switch (messageID)
        {
#if CLIENT
            case NetworkMessageIDS.PlayerUpdateNetMessage:
                //convert the pointer back into our message struct.
                PlayerUpdateNetMessage updateMessage = MemoryHelper.Read<PlayerUpdateNetMessage>(messageData);
                Debug.Log("Message received from server! Message ID: " + messageID + ", " +  "health: " + updateMessage.health + " gold: " + updateMessage.gold + " armor: " + updateMessage.armor);
                break;

#endif
        }
    }

    private void LogicOnSocketConnect(Connection connection)
    {
        RapidNetworkLibrar
#if SERVER
        Debug.Log("[Logic Thread]: A client has connected to the server");
        RNet.SendReliable<PlayerUpdateNetMessage>(connection, NetworkMessageIDS.PlayerUpdateNetMessage, new PlayerUpdateNetMessage()
        {
            armor = 150,
            gold = 245323,
            health = 255
        });

#endif
    }

    private void OnDestroy()
    {
        RNet.TearDown();
    }
    private void Update()
    {
        RNet.Tick();
    }
}