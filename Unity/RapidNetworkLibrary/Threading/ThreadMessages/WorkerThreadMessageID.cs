public enum WorkerThreadMessageID
{
    SendConnection,
    SendDisconnection,
    SendTimeout,

    SendInitializeServer,
    SendConnectToSocket,
    SendInitClient,
    SendSerializeNetworkMessage,
    SendDeserializeNetworkMessage,
    SendNetworkMessageToGameThread,
    SendPeerData,
    SendDisconnectionFromPeers
}
