public enum WorkerThreadMessageID
{
    SendConnection,
    SendDisconnection,
    SendTimeout,

    SendInitializeServer,
    SendConnectToSocket,
    SendInitClient,
    DontUse,
    SendDeserializeNetworkMessage,
    SendNetworkMessageToGameThread,
    SendPeerData,
    SendDisconnectionFromPeers,
    SendSerializeMessage
}
