# RNet

RNet is an engine agnostic lock free multithreaded reliable udp library designed for games. RNet uses native C sockets internally for minmized latency, zig zag encoding and float quantization for low bandwidth usage. RNet is blazing fast and makes uses of native memory to ensure no gc preassure from the library at all. RNet is easily extendable and made with simplicity in mind. 
<p align="center">
    <img src="https://github.com/user-attachments/assets/32f73a50-2407-42da-ab9d-8a32d6e46c40" />
</p>
# Features
* Connection Managment
* Server to server communication
* Clients can connect to multiple servers
* Blazing fast abstract data serialization
* Custom extensions to easily add features
* Lock free thread to thread communication
* Server/Client code stripping. No server code will be included with client builds.


# Starting a server
```csharp
RNet.InitializeServer("127.0.0.1", 7777, 255, 1024);
```

# Starting a client and connecting to a server

```csharp
RNet.InitializeClient(255);
RNet.Connect("127.0.0.1", 7777);
```

# Sending a message

```csharp
public struct TestMessageData : IMessageObject
{
    public int someInt;
    public float someFloat;
}
RNet.SendReliable(connection, NetworkMessageIDS.SendTestMessage, testMessageChannel, new TestMessageData()
{
    someInt = 342325,
    someFloat = 3423.343f
});
```

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
# Managing  Connections
Include your scene into the build settings and build your client. In the unity toolbar navigate back to ```RNet/SwitchTarget``` and select server. Right now a client can connect but no event has been registered to inform us of the connection. ```RNet.RegisterOnSocketConnectEvent``` is the method we use to register to the connection event. It takes two parameters, an action to be called on the LogicThread when a client connects and an action to be called on the Game(main/unity) Thread. The following are all valid examples on how to use ```RNet.RegisterOnSocketConnectEvent```

```csharp
RNet.RegisterOnSocketConnectEvent(socketConnectLogicAction: LogicOnSocketConnect);
RNet.RegisterOnSocketConnectEvent(socketConnectGameAction: GameOnSocketConnect);
RNet.RegisterOnSocketConnectEvent(socketConnectLogicAction: LogicOnSocketConnect, socketConnectGameAction: GameOnSocketConnect);
```

**Tip don't forget to call RNet.UnregisterOnSocket**

Your ```NetworkHandler.cs``` script should now look like this.

```csharp
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

# Sending Messages

In the unity editor toolbar navigate  to ```RNet/Main```, click the messages button on the left hand side. Type ```PlayerUpdateNetMessage``` then click create.

![image](https://github.com/user-attachments/assets/170fd27e-17e9-4c49-a1a4-454005827bc0)

Expand the ```PlayerUpdateNetMessage``` popout and expand the ```fields``` popout. You will notice, id and name are readonly and can not be edited. We are going to add 3 fields, press the + button 3 times. And fill  them out as follows.

![image](https://github.com/user-attachments/assets/97e70df9-8b87-4566-b288-088b8b6577a5)
**Tip ensure ```Generate Serializer``` is checked**

Press save, this is going to generate 3 files, in ```Assets/_code/generated/messages``` if you open them and inspect their values you will see the following 

```csharp
public class NetworkMessageIDS
{
    public const ushort PlayerUpdateNetMessage = 1;
}

public struct PlayerUpdateNetMessage : IMessageObject
{
    public int gold;
    public byte health;
    public byte armor;
}

[Serializer(NetworkMessageIDS.PlayerUpdateNetMessage)]
public class PlayerUpdateNetMessageSerializer : Serializer
{
    public override void Serialize(BitBuffer buffer, IntPtr messageData)
    {
        var msg = MemoryHelper.Read<PlayerUpdateNetMessage>(messageData);
        buffer.AddInt(msg.gold);
        buffer.AddByte(msg.health);
        buffer.AddByte(msg.armor);
    }
    public override IntPtr Deserialize(BitBuffer buffer)
    {
        var msg = new PlayerUpdateNetMessage()
        {
            gold = buffer.ReadInt(),
            health = buffer.ReadByte(),
            armor = buffer.ReadByte(),
        };
        return MemoryHelper.Write(msg);
    }
}
```
**Tip Never edit the generated files  directly always make your changes in the RNet menu**

A few things to note going on here,  ```NetworkMessageIDS``` holds the values that the network message id generate feeds. You do not have to manage these ids. Also note the attribute on ```PlayerUpdateNetMessageSerializer``` this is how  serializers are tied to specific messages internally.  You can uncheck generate serializer if you have some outlying cases that need hand written code. But it  advised to use the generated code as it is less prone to mistakes.


Sending the message is easy. Inside ```NetworkHandler.cs``` just  call ```RNet.SendReliable``` passing in our generated message id and our generated type. ```LogicOnSocketConnect``` should now look like this.

```csharp
    private void LogicOnSocketConnect(Connection connection)
    {
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
```
Navigate to  ```RNet/SwitchTarget/Client``` and set the target to client again. Now we are going to register the receive  event, by making a call to ```RNet.RegisterReceiveEvent```. Your ```onInit()``` method should look like this.

```csharp
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
```

# API
#### BitBuffer
Class used to write values to a buffer that can be converted to a span of bytes to send over the network.

```BitBuffer.AddBool(bool value)``` writes a bool to the end of the buffer advancing the position by 1 byte.

```BitBuffer.AddByte(byte value)``` writes a byte to the end of the buffer advancing the position by 1 byte.

```BitBuffer.AddInt(int value)``` writes an int to the end of the buffer advancing the position by 4 bytes.

```BitBuffer.AddLong(long value)``` writes a  long to  the end of the buffer advancing the position by 8 bytes.

```BitBuffer.AddShort(short value)``` writes a short to the end of the buffer advancing the position by 2 bytes.

```BitBuffer.AddUInt(uint value)``` writes an uint to the end of the buffer advancing the position by 4 bytes.

```BitBuffer.AddULong(ulong value)``` writes a ulong to the end of the buffer advancing the position by 8 bytes.

```BitBuffer.AddUShort(ushort value)``` writes a ushort to the end of the buffer advancing the position by 2 bytes.

```BitBuffer.Clear()``` clears all data from the buffer, resetting its position back to zero.

```BitBuffer.FromArray(byte[] data, int length)``` populates the buffer with the data in the provided array.

```BitBuffer.FromSpan(ref ReadOnlySpan<byte> span, int length``` populates the buffer with the data in the provided ReadOnlySpan.

```BitBuffer.PeekBool()``` returns a bool from the buffer without advancing the position.

```BitBuffer.PeekByte()``` returns a byte from the buffer without advancing the position.

```BitBuffer.PeekInt()``` returns an int from the buffer without advancing the position.

```BitBuffer.PeekLong()``` returns a long from the buffer without advancing the position.

```BitBuffer.PeekShort()``` returns a short from the buffer without advancing the position.

```BitBuffer.PeekUInt()``` returns a uint from the buffer without advancing the position.

```BitBuffer.PeekULong()``` returns a ulong from the buffer without advancing the position.

```BitBuffer.PeekUShort()``` returns a ushort from the buffer without advancing the position.

```BitBuffer.ReadBool()``` returns a bool from the buffer advancing the position 1 byte.

```BitBuffer.ReadByte()``` returns a byte from the buffer advancing the position 1 byte.

```BitBuffer.ReadInt()``` returns an int from the buffer advancing the position 4 bytes.

```BitBuffer.ReadLong()``` returns a long from the buffer advancing the position 8 bytes.

```BitBuffer.ReadShort()``` returns a short from the buffer advancing the position 2 bytes.

```BitBuffer.ReadUInt()``` readsa a uint from the buffer advancing the position 4 bytes.

```BitBuffer.ReadULong()``` reads a ulong from the buffer advancing the position 8 bytes.

```BitBuffer.ReadUShort()``` reads a ushort from the buffer advancing the position 2 bytes.

```BitBuffer.ToArray(byte[] data)``` copies data in the buffer into the provided array returning the length.

```BitBuffer.ToSpan(Span<byte> span)``` copies data in the buffer into the span returning the length.

#### Connection
Contains a managed pointer to the connection instance and a cached ID.

```Connection.BytesSent``` returns the total number of bytes sent during the connection.

```Connection.BytesReceived``` returns the total number of bytes received during the connection.

```Connection.ID```  returns a numeric ID for the connection. Always 0 on the client.

```Connection.IpAddress``` returns a native string that represents the IPAddress of the connection.

```Connection.Port``` returns the port the connection is using.

```Connection.LastReceiveTime``` returns the last time a packet has been received.

```Connection.LastRoundTripTime``` returns the round trip time at the time of the last update.

```Connection.Mtu``` returns an MTU

```Connection.PacketsSent``` returns  the total number of packets sent during the connection.

```Connections.PacketsLost``` returns the total number of packets that were considered lost during the connection based on retransmission logic.

#### HalfPrecision
Helper class used to convert floats to ushorts and back again before sending over the network.

```HalfPrecision.Dequantize(ushort value)``` returns a float  from the provided ushort. **Tip value passed into HalfPrecision.Dequantize must be a value that was returned by HalfPrecision.Quantize**

```HalfPrecision.Quantize(flaot value)``` returns a ushort from the provided float.

#### MemoryAllocator
Abstract class used to implement custom memory allocator. Passed to ```RNet.Init``` as an optional parameter. If no allocator is passed to ```RNetInit``` then ```RNetAllocator``` is used by default, except for on unity where 

```UnityAllocator``` is used instead.

```MemoryAllocator.Free(IntPtr ptr)``` Frees the native memory for the pointer provided.

```MemoryAllocator.Malloc(int size)``` returns pointer to of native memory of provided size.

#### MemoryHelper

```MemoryHelper.Alloc(int size)``` returns pointer of native memory of provided size.

```MemoryHelper.Free(IntPtr ptr)``` Frees the native memory for the pointer provided.

```MemoryHelper.Read<T>(IntPtr ptr)``` returns type `T` read from pointer.

```MemoryHelper.Write<T>(T value)``` Allocates native memory, storing `value` into the memory and returning a pointer to the memory.

#### NativeString
An unmanaged type used to create  strings without any heap allocations. Memory allocated must be manually freed.

```NativeString(string value)``` Creates a new instance of  ```NativeString``` allocating 2 *  string.Length  bytes of memory which must be manually freed.

```NativeString.Free()```  frees the native memory allocated.

```NativeString.ToString()``` return a managed string from the native memory.

#### RNet
Static class  used to initialize and manage RNet. All methods and properties in this class are safe to call from any thread at anytime.

```RNet.BytesReceived``` returns the total number of bytes received on this machine during the span of the current connection.

```RNet.BytesSent``` returns the total number of bytes sent on this machine during the span of the current connection.

```RNet.LastReceiveTime``` returns the time in milliseconds that this machine received its last packet.

```RNet.LastRoundTripTime``` returns the last received round trip time for this machine.

```RNet.LastSendTime``` returns the last time in miliseconds that this machine sent its last packet.

```RNet.Mtu``` returns this machines current MTU.

```RNet.PacketsLost``` returns the total number of packets that failed to aknowledge being received during the span of the current connection.

```RNet.PacketsSent``` returns the total number rof packets that were sent on this machine during the span of the current connection.

```RNet.BroadcastReliable<IMessageObject>(ushort messageID, byte channel, T messageData)``` broadcasts a reliable message to all connections.

```RNet.BroadcastReliable<IMessageObject>(ushort messageID, T messageData)``` broadcasts a reliable message to all connections.

```RNet.BroadcastUnreliable<IMessageObject>(ushort messageID, byte channel, T messageData)``` broadcasts an unreliable messsage to all connections.

```RNet.BroadcastUnreliable<IMessageObject>(ushort messageID, T messageData)``` broadcasts an unreliable message to all connections.

```RNet.Connect(string ip, ushort port)``` connects this socket to a server, must be called only after ```RNet.InitializeServer``` or ```RNet.InitializeClient```.

```RNet.Disconnect()``` disconnects this machine from all other connections.

```RNet.Disconnect(Connection connection)``` disconnects the passed connection from this machine.

```RNet.Init(Action initAction, MemoryAllocator allocator=null)``` initializes RNet, invoking initAction after. Must be called, then any other RNet methods cannot be used until initAction is invoked. Optionally you can pass an instance of a MemoryAllocator to be used by RNet.

```RNet.InitializeClient(byte maxChannels)```  initializes an  RNet client, must not be called until ```initAction``` of  ```RNet.Init``` is called. ```maxChannels``` must be the same on every server this this client plans on connecting to.

```RNet.InitializeServer(string ip, ushort port, byte maxChannels, uint maxConnections)``` initializes an RNet Server, must not be called until ```initAction``` of ```RNet.Init``` is called. ```maxChannels``` must be the same on every client and server planning to connect to this server.

```RNet.RegisterExtension<T>()``` registers an RNet extension to be loaded, needs to be called prior to ```RNet.InitializeServer``` or ```RNet.InitializeClient```.

```RNet.RegisterOnSocketConnectEvent(Action<Connection> socketConnectLogicAction = null, Action<Connection> socketConnectGameAction = null)``` registers methods to be invoked by RNet when a socket connects. socketConnectLogicAction is invoked on the logic thread, while socketConnectGameAction is invoked on the game thread. return true from the logic thread action to prevent socketConnectGameAction from being invoked. Can be called passing both actions,  just the game action, or just the logic action.

```RNet.RegisterOnSocketDisconnectEvent(Action<Connection> socketDisconnectLogicAction = null, Action<Connection> socketDisconnectGameAction = null)``` registers methods to be invoked by RNet when a socket connection has ended gracefully. socketDisconnectLogicAction is invoked from the logic thread, while socketDisconnectGameAction is invoked from the game thread. return true from the logic thread to prevent socketDisconnectGameAction from being invoked. Can be called passing both actions, just the game action, or just the logic action.

```RNet.RegisterOnSocketTimeoutEvent(Action<Connection> socketTimeoutLogicAction = null, Action<Connection> socketTimeoutGameAction = null)``` registers methods to be invoked by RNet when a socket connection has not ended gracefully. socketTimeoutLogicAction is invoked from the logic thread, while socketTimeoutGameAction is invoked from the game thread. return true from the logic thread to prevent socketTimeoutGameAction from being invoked. Can be called passing both actions, just the game action, or just the logic action.

```RNet.RegisterReceiveEvent(Action<Connection, ushort messageID, IntPtr messageData> logicReceiveAction, Action<Connection, ushort messageID, IntPtr messageData> gameReceiveAction``` registers methods to be invoked by RNet after an incoming packet has been deserialized into a network message. logiceReceiveAction is invoked from  the  logic thread while gameReceiveAction is invoked from the game thread. return true from logiceReceiveAction to prevent gameReceiveAction from being invoked. Can be called passing both actions, just the game action, or just the logic action.

```RNet.SendReliable<IMessageObject>(Connection target, ushort messageID, byte channel, T messageData)``` sends a reliable message to the target ```Connection``` on the specified ```channel```.

```RNet.SendReliable<IMessageObject>(Connection target, ushortMessageID, T messageData)``` sends a reliable message to the target ```Connection```.

```RNet.SendUnreliable<IMessageObject>(Connection target, ushort messageID, byte channel, T messageData)``` sends an unreliable message to the target ```Connection``` on the specified ```channel```.

```RNet.SendUnreliable<IMessageObject>(Connection target, ushortMessageID, T messageData)``` sends an unreliable message to the target ```Connection```.

```RNet.TearDown()``` Deinitializes RNet and all threads being used deallocating all native memory allocated, must always be called especially in unity projects or editor may crash after entering and exiting playmode several times.

```RNet.UnRegisterOnSocketConnectEvent(Action<Connection> socketConnectLogicAction = null, Action<Connection> socketConnectGameAction = null)``` unregisters methods to be invoked by RNet when a socket connects. Can be called passing both actions, just the game action, or just the logic action.

```RNet.UnRegisterOnSocketDisconnectEvent(Action<Connection> socketDisconnectLogicAction = null, Action<Connection> socketDisconnectGameAction = null)``` unregisters methods to be invoked by RNet when a socket connection has ended gracefully. Can be called passing both actions, just the game action, or just the logic action.

```RNet.RegisterOnSocketTimeoutEvent(Action<Connection> socketTimeoutLogicAction = null, Action<Connection> socketTimeoutGameAction = null)``` unregisters methods to be invoked by RNet when a socket connection has not ended gracefully. Can be called passing both actions, just the game action, or just the logic action.

```RNet.RegisterReceiveEvent(Action<Connection, ushort messageID, IntPtr messageData> logicReceiveAction, Action<Connection, ushort messageID, IntPtr messageData> gameReceiveAction``` registers methods to be invoked by RNet after an incoming packet has been deserialized into a network message. Can be called passing both actions, just the game action, or just the logic action.

#### RNetExtension
Base class used to implement self contained, reusable extensions for RNet.

```RNetExtension.RNetExtension(WorkerCollection workers)``` called automatically by RNet when loading the extension.

```RNet.OnSocketConnect(ThreadType threadType, Connection connection)``` called automatically by RNet when a socket connects.

```RNet.OnSocketDisconnect(ThreadType threadType, Connection connection)``` called automatically by RNet when a socket disconnects.

```RNet.OnSocketReceive(ThreadType threadType, Connection connection, ushort messageID, IntPtr messageData)``` called automatically by RNet after a message has been deserialized. Return true from this method to prevent the methods registered by RNet.RegisterReceiveEvent from being invoked.

```RNet.OnSocketTimeout(ThreadType threadType, Connection connection)``` called automatically by RNet when a socket times out.

```RNet.OnThreadMessageReceived(ThreadType threadType, ushort threadMessageID, IntPtr threadMessageData)``` called automatically by RNet when a thread receives a message from another thread. This is not related to network messages in any way.




