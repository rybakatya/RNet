public class WorkerThreadEventID
{
    public const ushort SendConnection =        ushort.MaxValue;
    public const ushort SendDisconnection =     ushort.MaxValue - 1;
    public const ushort SendTimeout =           ushort.MaxValue - 2;
    public const ushort SendInitializeServer =  ushort.MaxValue - 3;
    public const ushort SendConnectToSocket =   ushort.MaxValue - 4;
    public const ushort SendInitClient =        ushort.MaxValue - 5;
    public const ushort SendDeserializeNetworkMessage = ushort.MaxValue - 6;
    public const ushort SendNetworkMessageToGameThread = ushort.MaxValue - 7;
    public const ushort SendPeerData = ushort.MaxValue - 8;
    public const ushort SendDisconnectionFromPeers = ushort.MaxValue - 9;
    public const ushort SendSerializeMessageEvent = ushort.MaxValue - 10;
    public const ushort SendRegisterThreadEvent = ushort.MaxValue - 11;
    public const ushort SendMaxConnections = ushort.MaxValue - 12;
}
