internal enum WorkerThreadMessageID
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
    SendSerializeMessage
}
