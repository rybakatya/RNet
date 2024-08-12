# RNet

RNet is an engine agnostic lock free multithreaded reliable udp library designed for games. RNet uses native C sockets internally for minmized latency, zig zag encoding and float quantization for low bandwidth usage. RNet is blazing fast and makes uses of native memory to ensure no gc preassure from the library at all. RNet is easily extendable and made with simplicity in mind. RNet was designed for an in production MMO and is guarenteed to stay up to date and free of vulnerabilities.

# Features
* Connection Managment
* Server to server communication
* Clients can connect to multiple servers
* Blazing fast serialization
* Custom extensions to easily add features
* Lock free thread to thread communication
* Server/Client code stripping. No server code will be included with client builds.


# Getting Started With Unity
First install the needed packages to run RNet. In the unity editor go to ```Window/Package Manager``` then click "add package from git url" in the drop down.\
\
![image](https://github.com/user-attachments/assets/aa8f8637-f77f-480e-a9d4-137ed93d5e50)



paste the following in the input box. ```https://github.com/rybakatya/RapidNet.git?path=/Unity/ENet```

Click "add package from git url" again this time pasting ```https://github.com/rybakatya/RapidNet.git?path=/Unity/RapidNetworkLibrary```

You will now see a new menu item in the unity menu. Navigate to ```RNet/SwitchTarget/Server``` to tag the build as server.


![image](https://github.com/user-attachments/assets/d1af4aa8-9cfd-409f-84f9-ccf734510c2b)


Next we need to make sure our game runs when alt tabbed to do this navigate to Edit/Project Settings/Player and make sure "Run in background" is enabled.



![image](https://github.com/user-attachments/assets/ed37c7b6-c183-4b8c-8970-97b1a836b0cd)


Create a new C# script called ```NetworkHandler.cs``` and paste in the following.

```csharp
public class NetworkHandler : MonoBehaviour
{
    private void Start()
    {
        RNet.Init(onInit);
    }

    private void Update()
    {
        RNet.Tick();
    }

    private void onInit()
    {

    }
}
```

Attach this script to a gameobject in the scene and press play. You should see the following in your unity console.

![image](https://github.com/user-attachments/assets/7061e557-f3c6-4cdb-bd4d-33585c212abc)


You can disable the logger from printing to the console by calling ```RNet.DisableLoggerPrinting();``` 


Now lets initialize a server, to do this we are going to call ```RNet.InitializeServer(string ip, ushort port, byte maxChannels, ushort maxConnections);``` inside of ```onInit()```

```csharp
private void onInit()
{
    RNet.InitializeServer("127.0.0.1", 7777, 255, 1024);
}
```

You should now be able to see the following in the unity console.


![image](https://github.com/user-attachments/assets/d8fcab26-48ab-45da-b3e7-6fd8c2ee309b)


Before we make a console we need to make one change. We need to wrap the call to ```RNet.InitializeServer``` in an ```#if SERVER`` statement. This ensures that client builds do not include any server code, and should be used on any server only code you write.

```csharp
private void onInit()
{
#if SERVER
    RNet.InitializeServer("127.0.0.1", 7777, 255, 1024);
#elif CLIENT

#endif
}
```

You'll notice that if you try to write anycode within the client statement it will be greyed out.

![image](https://github.com/user-attachments/assets/e4fb3f2c-59ee-4d3c-a154-98b3a4d4ff4d)

To fix this navigate to ```RNet/SwitchTarget``` and select client. Unity will then refresh and if you go back to visual studio you will notice that server is now greyed out and client is not. Your ```onInit()``` method should now look like this. 

```csharp
private void onInit()
{
#if SERVER
    RNet.InitializeServer("127.0.0.1", 7777, 255, 1024);
#elif CLIENT
    RNet.InitializeClient(255);
    RNet.Connect("127.0.0.1", 7777);
#endif
}
```

Now include your scene into the build settings and build your client. In the unity toolbar navigate back to ```RNet/SwitchTarget``` and select server. Right now a client can connect but no event has been registered to inform us of the connection. ```RNet.RegisterOnSocketConnectEvent``` is the method we use to register to the connection event. It takes two parameters, an action to be called on the LogicThread when a client connects and an action to be called on the Game(main/unity) Thread. The following are all valid examples on how to use ```RNet.RegisterOnSocketConnectEvent```

```csharp
RNet.RegisterOnSocketConnectEvent(socketConnectLogicAction: LogicOnSocketConnect);
RNet.RegisterOnSocketConnectEvent(socketConnectGameAction: GameOnSocketConnect);
RNet.RegisterOnSocketConnectEvent(socketConnectLogicAction: LogicOnSocketConnect, socketConnectGameAction: GameOnSocketConnect);
```
**Tip don't forget to call RNet.UnregisterOnSocket**

Your ```NetworkHandler.cs``` script should now look like this.

```csharp
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
#elif CLIENT
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
```

Enter playmode in the editor, then launch the recently built client. You should see "[Logic Thread]: A connection has been formed between two sockets!" printed to your unity console.

