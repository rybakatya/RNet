using RapidNetworkLibrary.Serialization;
public struct PlayerUpdateNetMessage : IMessageObject
{
    public int gold;
    public byte health;
    public byte armor;
}
