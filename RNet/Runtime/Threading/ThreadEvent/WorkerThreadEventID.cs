internal enum WorkerThreadEventID
{
    SendConnection,
    SendDisconnection,
    SendTimeout,
    SendInitializeServer,
    SendConnectToSocket,
    SendInitClient,
    SendDeserializeNetworkMessage,
    SendNetworkMessageToGameThread,
    SendPeerData,
    SendDisconnectionFromPeers,
    SendSerializeMessageEvent
}
