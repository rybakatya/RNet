using RapidNetworkLibrary.Serialization;
using RapidNetworkLibrary.Memory;
using System;

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
