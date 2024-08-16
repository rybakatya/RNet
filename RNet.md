<a name='assembly'></a>
# RNet

## Contents

- [BitBuffer](#T-RapidNetworkLibrary-Serialization-BitBuffer 'RapidNetworkLibrary.Serialization.BitBuffer')
  - [#ctor(numberOfBuckets)](#M-RapidNetworkLibrary-Serialization-BitBuffer-#ctor-System-Int32- 'RapidNetworkLibrary.Serialization.BitBuffer.#ctor(System.Int32)')
  - [IsFinished](#P-RapidNetworkLibrary-Serialization-BitBuffer-IsFinished 'RapidNetworkLibrary.Serialization.BitBuffer.IsFinished')
  - [Length](#P-RapidNetworkLibrary-Serialization-BitBuffer-Length 'RapidNetworkLibrary.Serialization.BitBuffer.Length')
  - [Add(numBits,value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-Add-System-Int32,System-UInt32- 'RapidNetworkLibrary.Serialization.BitBuffer.Add(System.Int32,System.UInt32)')
  - [AddBool(value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-AddBool-System-Boolean- 'RapidNetworkLibrary.Serialization.BitBuffer.AddBool(System.Boolean)')
  - [AddByte(value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-AddByte-System-Byte- 'RapidNetworkLibrary.Serialization.BitBuffer.AddByte(System.Byte)')
  - [AddInt(value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-AddInt-System-Int32- 'RapidNetworkLibrary.Serialization.BitBuffer.AddInt(System.Int32)')
  - [AddLong(value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-AddLong-System-Int64- 'RapidNetworkLibrary.Serialization.BitBuffer.AddLong(System.Int64)')
  - [AddShort(value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-AddShort-System-Int16- 'RapidNetworkLibrary.Serialization.BitBuffer.AddShort(System.Int16)')
  - [AddUInt(value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-AddUInt-System-UInt32- 'RapidNetworkLibrary.Serialization.BitBuffer.AddUInt(System.UInt32)')
  - [AddULong(value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-AddULong-System-UInt64- 'RapidNetworkLibrary.Serialization.BitBuffer.AddULong(System.UInt64)')
  - [AddUShort(value)](#M-RapidNetworkLibrary-Serialization-BitBuffer-AddUShort-System-UInt16- 'RapidNetworkLibrary.Serialization.BitBuffer.AddUShort(System.UInt16)')
  - [Clear()](#M-RapidNetworkLibrary-Serialization-BitBuffer-Clear 'RapidNetworkLibrary.Serialization.BitBuffer.Clear')
  - [FromArray(data,length)](#M-RapidNetworkLibrary-Serialization-BitBuffer-FromArray-System-Byte[],System-Int32- 'RapidNetworkLibrary.Serialization.BitBuffer.FromArray(System.Byte[],System.Int32)')
  - [FromSpan(data,length)](#M-RapidNetworkLibrary-Serialization-BitBuffer-FromSpan-System-ReadOnlySpan{System-Byte}@,System-Int32- 'RapidNetworkLibrary.Serialization.BitBuffer.FromSpan(System.ReadOnlySpan{System.Byte}@,System.Int32)')
  - [Peek(numBits)](#M-RapidNetworkLibrary-Serialization-BitBuffer-Peek-System-Int32- 'RapidNetworkLibrary.Serialization.BitBuffer.Peek(System.Int32)')
  - [PeekBool()](#M-RapidNetworkLibrary-Serialization-BitBuffer-PeekBool 'RapidNetworkLibrary.Serialization.BitBuffer.PeekBool')
  - [PeekByte()](#M-RapidNetworkLibrary-Serialization-BitBuffer-PeekByte 'RapidNetworkLibrary.Serialization.BitBuffer.PeekByte')
  - [PeekInt()](#M-RapidNetworkLibrary-Serialization-BitBuffer-PeekInt 'RapidNetworkLibrary.Serialization.BitBuffer.PeekInt')
  - [PeekLong()](#M-RapidNetworkLibrary-Serialization-BitBuffer-PeekLong 'RapidNetworkLibrary.Serialization.BitBuffer.PeekLong')
  - [PeekShort()](#M-RapidNetworkLibrary-Serialization-BitBuffer-PeekShort 'RapidNetworkLibrary.Serialization.BitBuffer.PeekShort')
  - [PeekUInt()](#M-RapidNetworkLibrary-Serialization-BitBuffer-PeekUInt 'RapidNetworkLibrary.Serialization.BitBuffer.PeekUInt')
  - [PeekULong()](#M-RapidNetworkLibrary-Serialization-BitBuffer-PeekULong 'RapidNetworkLibrary.Serialization.BitBuffer.PeekULong')
  - [PeekUShort()](#M-RapidNetworkLibrary-Serialization-BitBuffer-PeekUShort 'RapidNetworkLibrary.Serialization.BitBuffer.PeekUShort')
  - [Read(numBits)](#M-RapidNetworkLibrary-Serialization-BitBuffer-Read-System-Int32- 'RapidNetworkLibrary.Serialization.BitBuffer.Read(System.Int32)')
  - [ReadBool()](#M-RapidNetworkLibrary-Serialization-BitBuffer-ReadBool 'RapidNetworkLibrary.Serialization.BitBuffer.ReadBool')
  - [ReadByte()](#M-RapidNetworkLibrary-Serialization-BitBuffer-ReadByte 'RapidNetworkLibrary.Serialization.BitBuffer.ReadByte')
  - [ReadInt()](#M-RapidNetworkLibrary-Serialization-BitBuffer-ReadInt 'RapidNetworkLibrary.Serialization.BitBuffer.ReadInt')
  - [ReadLong()](#M-RapidNetworkLibrary-Serialization-BitBuffer-ReadLong 'RapidNetworkLibrary.Serialization.BitBuffer.ReadLong')
  - [ReadShort()](#M-RapidNetworkLibrary-Serialization-BitBuffer-ReadShort 'RapidNetworkLibrary.Serialization.BitBuffer.ReadShort')
  - [ReadUInt()](#M-RapidNetworkLibrary-Serialization-BitBuffer-ReadUInt 'RapidNetworkLibrary.Serialization.BitBuffer.ReadUInt')
  - [ReadULong()](#M-RapidNetworkLibrary-Serialization-BitBuffer-ReadULong 'RapidNetworkLibrary.Serialization.BitBuffer.ReadULong')
  - [ReadUShort()](#M-RapidNetworkLibrary-Serialization-BitBuffer-ReadUShort 'RapidNetworkLibrary.Serialization.BitBuffer.ReadUShort')
  - [ToArray(data)](#M-RapidNetworkLibrary-Serialization-BitBuffer-ToArray-System-Byte[]- 'RapidNetworkLibrary.Serialization.BitBuffer.ToArray(System.Byte[])')
  - [ToSpan(data)](#M-RapidNetworkLibrary-Serialization-BitBuffer-ToSpan-System-Span{System-Byte}@- 'RapidNetworkLibrary.Serialization.BitBuffer.ToSpan(System.Span{System.Byte}@)')
- [Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection')
  - [BytesReceived](#P-RapidNetworkLibrary-Connections-Connection-BytesReceived 'RapidNetworkLibrary.Connections.Connection.BytesReceived')
  - [BytesSent](#P-RapidNetworkLibrary-Connections-Connection-BytesSent 'RapidNetworkLibrary.Connections.Connection.BytesSent')
  - [ID](#P-RapidNetworkLibrary-Connections-Connection-ID 'RapidNetworkLibrary.Connections.Connection.ID')
  - [IpAddress](#P-RapidNetworkLibrary-Connections-Connection-IpAddress 'RapidNetworkLibrary.Connections.Connection.IpAddress')
  - [LastReceiveTime](#P-RapidNetworkLibrary-Connections-Connection-LastReceiveTime 'RapidNetworkLibrary.Connections.Connection.LastReceiveTime')
  - [LastRoundTripTime](#P-RapidNetworkLibrary-Connections-Connection-LastRoundTripTime 'RapidNetworkLibrary.Connections.Connection.LastRoundTripTime')
  - [LastSendTime](#P-RapidNetworkLibrary-Connections-Connection-LastSendTime 'RapidNetworkLibrary.Connections.Connection.LastSendTime')
  - [Mtu](#P-RapidNetworkLibrary-Connections-Connection-Mtu 'RapidNetworkLibrary.Connections.Connection.Mtu')
  - [PacketsLost](#P-RapidNetworkLibrary-Connections-Connection-PacketsLost 'RapidNetworkLibrary.Connections.Connection.PacketsLost')
  - [PacketsSent](#P-RapidNetworkLibrary-Connections-Connection-PacketsSent 'RapidNetworkLibrary.Connections.Connection.PacketsSent')
  - [Port](#P-RapidNetworkLibrary-Connections-Connection-Port 'RapidNetworkLibrary.Connections.Connection.Port')
  - [Equals(other)](#M-RapidNetworkLibrary-Connections-Connection-Equals-RapidNetworkLibrary-Connections-Connection- 'RapidNetworkLibrary.Connections.Connection.Equals(RapidNetworkLibrary.Connections.Connection)')
  - [Equals(obj)](#M-RapidNetworkLibrary-Connections-Connection-Equals-System-Object- 'RapidNetworkLibrary.Connections.Connection.Equals(System.Object)')
  - [GetHashCode()](#M-RapidNetworkLibrary-Connections-Connection-GetHashCode 'RapidNetworkLibrary.Connections.Connection.GetHashCode')
  - [ToString()](#M-RapidNetworkLibrary-Connections-Connection-ToString 'RapidNetworkLibrary.Connections.Connection.ToString')
- [Enumerator](#T-LocklessQueue-Queues-MPSCQueue`1-Enumerator 'LocklessQueue.Queues.MPSCQueue`1.Enumerator')
  - [Current](#P-LocklessQueue-Queues-MPSCQueue`1-Enumerator-Current 'LocklessQueue.Queues.MPSCQueue`1.Enumerator.Current')
  - [Dispose()](#M-LocklessQueue-Queues-MPSCQueue`1-Enumerator-Dispose 'LocklessQueue.Queues.MPSCQueue`1.Enumerator.Dispose')
  - [MoveNext()](#M-LocklessQueue-Queues-MPSCQueue`1-Enumerator-MoveNext 'LocklessQueue.Queues.MPSCQueue`1.Enumerator.MoveNext')
  - [Reset()](#M-LocklessQueue-Queues-MPSCQueue`1-Enumerator-Reset 'LocklessQueue.Queues.MPSCQueue`1.Enumerator.Reset')
- [GameWorker](#T-RapidNetworkLibrary-Workers-GameWorker 'RapidNetworkLibrary.Workers.GameWorker')
  - [#ctor()](#M-RapidNetworkLibrary-Workers-GameWorker-#ctor-RapidNetworkLibrary-WorkerCollection,RapidNetworkLibrary-Extensions-ExtensionManager- 'RapidNetworkLibrary.Workers.GameWorker.#ctor(RapidNetworkLibrary.WorkerCollection,RapidNetworkLibrary.Extensions.ExtensionManager)')
  - [OnDestroy()](#M-RapidNetworkLibrary-Workers-GameWorker-OnDestroy 'RapidNetworkLibrary.Workers.GameWorker.OnDestroy')
- [HalfPrecision](#T-RapidNetworkLibrary-Serialization-HalfPrecision 'RapidNetworkLibrary.Serialization.HalfPrecision')
  - [Dequantize(value)](#M-RapidNetworkLibrary-Serialization-HalfPrecision-Dequantize-System-UInt16- 'RapidNetworkLibrary.Serialization.HalfPrecision.Dequantize(System.UInt16)')
  - [Quantize(value)](#M-RapidNetworkLibrary-Serialization-HalfPrecision-Quantize-System-Single- 'RapidNetworkLibrary.Serialization.HalfPrecision.Quantize(System.Single)')
- [HashHelpers](#T-LocklessQueue-Sets-HashHelpers 'LocklessQueue.Sets.HashHelpers')
  - [FastMod()](#M-LocklessQueue-Sets-HashHelpers-FastMod-System-UInt32,System-UInt32,System-UInt64- 'LocklessQueue.Sets.HashHelpers.FastMod(System.UInt32,System.UInt32,System.UInt64)')
  - [GetFastModMultiplier()](#M-LocklessQueue-Sets-HashHelpers-GetFastModMultiplier-System-UInt32- 'LocklessQueue.Sets.HashHelpers.GetFastModMultiplier(System.UInt32)')
- [IConsumerQueue\`1](#T-LocklessQueue-IConsumerQueue`1 'LocklessQueue.IConsumerQueue`1')
  - [IsEmpty](#P-LocklessQueue-IConsumerQueue`1-IsEmpty 'LocklessQueue.IConsumerQueue`1.IsEmpty')
  - [IsMultiConsumer](#P-LocklessQueue-IConsumerQueue`1-IsMultiConsumer 'LocklessQueue.IConsumerQueue`1.IsMultiConsumer')
  - [CopyTo()](#M-LocklessQueue-IConsumerQueue`1-CopyTo-`0[],System-Int32- 'LocklessQueue.IConsumerQueue`1.CopyTo(`0[],System.Int32)')
  - [ToArray()](#M-LocklessQueue-IConsumerQueue`1-ToArray 'LocklessQueue.IConsumerQueue`1.ToArray')
  - [TryDequeue()](#M-LocklessQueue-IConsumerQueue`1-TryDequeue-`0@- 'LocklessQueue.IConsumerQueue`1.TryDequeue(`0@)')
  - [TryPeek()](#M-LocklessQueue-IConsumerQueue`1-TryPeek-`0@- 'LocklessQueue.IConsumerQueue`1.TryPeek(`0@)')
- [IMessageObject](#T-IMessageObject 'IMessageObject')
- [IProducerConsumerQueue\`1](#T-LocklessQueue-IProducerConsumerQueue`1 'LocklessQueue.IProducerConsumerQueue`1')
- [IProducerQueue\`1](#T-LocklessQueue-IProducerQueue`1 'LocklessQueue.IProducerQueue`1')
  - [IsMultiProducer](#P-LocklessQueue-IProducerQueue`1-IsMultiProducer 'LocklessQueue.IProducerQueue`1.IsMultiProducer')
  - [TryEnqueue()](#M-LocklessQueue-IProducerQueue`1-TryEnqueue-`0- 'LocklessQueue.IProducerQueue`1.TryEnqueue(`0)')
- [LogLevel](#T-RapidNetworkLibrary-Logging-LogLevel 'RapidNetworkLibrary.Logging.LogLevel')
  - [Error](#F-RapidNetworkLibrary-Logging-LogLevel-Error 'RapidNetworkLibrary.Logging.LogLevel.Error')
  - [Exception](#F-RapidNetworkLibrary-Logging-LogLevel-Exception 'RapidNetworkLibrary.Logging.LogLevel.Exception')
  - [Info](#F-RapidNetworkLibrary-Logging-LogLevel-Info 'RapidNetworkLibrary.Logging.LogLevel.Info')
  - [Warning](#F-RapidNetworkLibrary-Logging-LogLevel-Warning 'RapidNetworkLibrary.Logging.LogLevel.Warning')
- [Logger](#T-RapidNetworkLibrary-Logging-Logger 'RapidNetworkLibrary.Logging.Logger')
  - [Log(level,msg)](#M-RapidNetworkLibrary-Logging-Logger-Log-RapidNetworkLibrary-Logging-LogLevel,System-String- 'RapidNetworkLibrary.Logging.Logger.Log(RapidNetworkLibrary.Logging.LogLevel,System.String)')
- [LogicWorkerThread](#T-RapidNetworkLibrary-Workers-LogicWorkerThread 'RapidNetworkLibrary.Workers.LogicWorkerThread')
  - [onLogicInit](#F-RapidNetworkLibrary-Workers-LogicWorkerThread-onLogicInit 'RapidNetworkLibrary.Workers.LogicWorkerThread.onLogicInit')
  - [Destroy()](#M-RapidNetworkLibrary-Workers-LogicWorkerThread-Destroy 'RapidNetworkLibrary.Workers.LogicWorkerThread.Destroy')
  - [Init()](#M-RapidNetworkLibrary-Workers-LogicWorkerThread-Init 'RapidNetworkLibrary.Workers.LogicWorkerThread.Init')
  - [Tick()](#M-RapidNetworkLibrary-Workers-LogicWorkerThread-Tick 'RapidNetworkLibrary.Workers.LogicWorkerThread.Tick')
- [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1')
  - [#ctor(capacity)](#M-LocklessQueue-Queues-MPSCQueue`1-#ctor-System-Int32- 'LocklessQueue.Queues.MPSCQueue`1.#ctor(System.Int32)')
  - [#ctor(collection)](#M-LocklessQueue-Queues-MPSCQueue`1-#ctor-System-Collections-Generic-ICollection{`0}- 'LocklessQueue.Queues.MPSCQueue`1.#ctor(System.Collections.Generic.ICollection{`0})')
  - [Capacity](#P-LocklessQueue-Queues-MPSCQueue`1-Capacity 'LocklessQueue.Queues.MPSCQueue`1.Capacity')
  - [Count](#P-LocklessQueue-Queues-MPSCQueue`1-Count 'LocklessQueue.Queues.MPSCQueue`1.Count')
  - [IsEmpty](#P-LocklessQueue-Queues-MPSCQueue`1-IsEmpty 'LocklessQueue.Queues.MPSCQueue`1.IsEmpty')
  - [Clear()](#M-LocklessQueue-Queues-MPSCQueue`1-Clear 'LocklessQueue.Queues.MPSCQueue`1.Clear')
  - [CopyTo(array,index)](#M-LocklessQueue-Queues-MPSCQueue`1-CopyTo-`0[],System-Int32- 'LocklessQueue.Queues.MPSCQueue`1.CopyTo(`0[],System.Int32)')
  - [GetEnumerator()](#M-LocklessQueue-Queues-MPSCQueue`1-GetEnumerator 'LocklessQueue.Queues.MPSCQueue`1.GetEnumerator')
  - [System#Collections#IEnumerable#GetEnumerator()](#M-LocklessQueue-Queues-MPSCQueue`1-System#Collections#IEnumerable#GetEnumerator 'LocklessQueue.Queues.MPSCQueue`1.System#Collections#IEnumerable#GetEnumerator')
  - [ToArray()](#M-LocklessQueue-Queues-MPSCQueue`1-ToArray 'LocklessQueue.Queues.MPSCQueue`1.ToArray')
  - [TryDequeue()](#M-LocklessQueue-Queues-MPSCQueue`1-TryDequeue-`0@- 'LocklessQueue.Queues.MPSCQueue`1.TryDequeue(`0@)')
  - [TryEnqueue()](#M-LocklessQueue-Queues-MPSCQueue`1-TryEnqueue-`0- 'LocklessQueue.Queues.MPSCQueue`1.TryEnqueue(`0)')
  - [TryPeek()](#M-LocklessQueue-Queues-MPSCQueue`1-TryPeek-`0@- 'LocklessQueue.Queues.MPSCQueue`1.TryPeek(`0@)')
- [MemoryAllocator](#T-RapidNetworkLibrary-Memory-MemoryAllocator 'RapidNetworkLibrary.Memory.MemoryAllocator')
  - [Free(ptr)](#M-RapidNetworkLibrary-Memory-MemoryAllocator-Free-System-IntPtr- 'RapidNetworkLibrary.Memory.MemoryAllocator.Free(System.IntPtr)')
  - [Malloc(size)](#M-RapidNetworkLibrary-Memory-MemoryAllocator-Malloc-System-Int32- 'RapidNetworkLibrary.Memory.MemoryAllocator.Malloc(System.Int32)')
- [MemoryHelper](#T-RapidNetworkLibrary-Memory-MemoryHelper 'RapidNetworkLibrary.Memory.MemoryHelper')
  - [Alloc()](#M-RapidNetworkLibrary-Memory-MemoryHelper-Alloc-System-Int32- 'RapidNetworkLibrary.Memory.MemoryHelper.Alloc(System.Int32)')
  - [Free(ptr)](#M-RapidNetworkLibrary-Memory-MemoryHelper-Free-System-IntPtr- 'RapidNetworkLibrary.Memory.MemoryHelper.Free(System.IntPtr)')
  - [Read\`\`1(ptr)](#M-RapidNetworkLibrary-Memory-MemoryHelper-Read``1-System-IntPtr- 'RapidNetworkLibrary.Memory.MemoryHelper.Read``1(System.IntPtr)')
  - [Write\`\`1(value)](#M-RapidNetworkLibrary-Memory-MemoryHelper-Write``1-``0- 'RapidNetworkLibrary.Memory.MemoryHelper.Write``1(``0)')
- [NativeString](#T-RapidNetworkLibrary-NativeString 'RapidNetworkLibrary.NativeString')
  - [#ctor(str)](#M-RapidNetworkLibrary-NativeString-#ctor-System-String- 'RapidNetworkLibrary.NativeString.#ctor(System.String)')
  - [Free()](#M-RapidNetworkLibrary-NativeString-Free 'RapidNetworkLibrary.NativeString.Free')
  - [ToString()](#M-RapidNetworkLibrary-NativeString-ToString 'RapidNetworkLibrary.NativeString.ToString')
- [RNet](#T-RapidNetworkLibrary-RNet 'RapidNetworkLibrary.RNet')
  - [BytesReceived](#P-RapidNetworkLibrary-RNet-BytesReceived 'RapidNetworkLibrary.RNet.BytesReceived')
  - [BytesSent](#P-RapidNetworkLibrary-RNet-BytesSent 'RapidNetworkLibrary.RNet.BytesSent')
  - [LastReceiveTime](#P-RapidNetworkLibrary-RNet-LastReceiveTime 'RapidNetworkLibrary.RNet.LastReceiveTime')
  - [LastRoundTripTime](#P-RapidNetworkLibrary-RNet-LastRoundTripTime 'RapidNetworkLibrary.RNet.LastRoundTripTime')
  - [LastSendTime](#P-RapidNetworkLibrary-RNet-LastSendTime 'RapidNetworkLibrary.RNet.LastSendTime')
  - [Mtu](#P-RapidNetworkLibrary-RNet-Mtu 'RapidNetworkLibrary.RNet.Mtu')
  - [PacketsLost](#P-RapidNetworkLibrary-RNet-PacketsLost 'RapidNetworkLibrary.RNet.PacketsLost')
  - [PacketsSent](#P-RapidNetworkLibrary-RNet-PacketsSent 'RapidNetworkLibrary.RNet.PacketsSent')
  - [BroadcastReliable\`\`1(messageID,channel,message)](#M-RapidNetworkLibrary-RNet-BroadcastReliable``1-System-UInt16,System-Byte,``0- 'RapidNetworkLibrary.RNet.BroadcastReliable``1(System.UInt16,System.Byte,``0)')
  - [BroadcastReliable\`\`1(messageID,message)](#M-RapidNetworkLibrary-RNet-BroadcastReliable``1-System-UInt16,``0- 'RapidNetworkLibrary.RNet.BroadcastReliable``1(System.UInt16,``0)')
  - [BroadcastUnreliable\`\`1(messageID,channel,message)](#M-RapidNetworkLibrary-RNet-BroadcastUnreliable``1-System-UInt16,System-Byte,``0- 'RapidNetworkLibrary.RNet.BroadcastUnreliable``1(System.UInt16,System.Byte,``0)')
  - [BroadcastUnreliable\`\`1(messageID,message)](#M-RapidNetworkLibrary-RNet-BroadcastUnreliable``1-System-UInt16,``0- 'RapidNetworkLibrary.RNet.BroadcastUnreliable``1(System.UInt16,``0)')
  - [Connect(ip,port)](#M-RapidNetworkLibrary-RNet-Connect-System-String,System-UInt16- 'RapidNetworkLibrary.RNet.Connect(System.String,System.UInt16)')
  - [Disconnect(connection)](#M-RapidNetworkLibrary-RNet-Disconnect-RapidNetworkLibrary-Connections-Connection- 'RapidNetworkLibrary.RNet.Disconnect(RapidNetworkLibrary.Connections.Connection)')
  - [Disconnect()](#M-RapidNetworkLibrary-RNet-Disconnect 'RapidNetworkLibrary.RNet.Disconnect')
  - [Init(initAction,alloc)](#M-RapidNetworkLibrary-RNet-Init-System-Action,RapidNetworkLibrary-Memory-MemoryAllocator- 'RapidNetworkLibrary.RNet.Init(System.Action,RapidNetworkLibrary.Memory.MemoryAllocator)')
  - [InitializeServer(ip,port,maxChannels,maxConnections)](#M-RapidNetworkLibrary-RNet-InitializeServer-System-String,System-UInt16,System-Byte,System-UInt16- 'RapidNetworkLibrary.RNet.InitializeServer(System.String,System.UInt16,System.Byte,System.UInt16)')
  - [RegisterExtension\`\`1()](#M-RapidNetworkLibrary-RNet-RegisterExtension``1 'RapidNetworkLibrary.RNet.RegisterExtension``1')
  - [RegisterOnSocketConnectEvent(socketConnectLogicAction,socketConnectGameAction)](#M-RapidNetworkLibrary-RNet-RegisterOnSocketConnectEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}- 'RapidNetworkLibrary.RNet.RegisterOnSocketConnectEvent(System.Action{RapidNetworkLibrary.Connections.Connection},System.Action{RapidNetworkLibrary.Connections.Connection})')
  - [RegisterOnSocketDisconnectEvent(socketDisconnectLogicAction,socketDisconnectGameAction)](#M-RapidNetworkLibrary-RNet-RegisterOnSocketDisconnectEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}- 'RapidNetworkLibrary.RNet.RegisterOnSocketDisconnectEvent(System.Action{RapidNetworkLibrary.Connections.Connection},System.Action{RapidNetworkLibrary.Connections.Connection})')
  - [RegisterOnSocketTimeoutEvent(socketTimeoutLogicAction,socketTimeoutGameAction)](#M-RapidNetworkLibrary-RNet-RegisterOnSocketTimeoutEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}- 'RapidNetworkLibrary.RNet.RegisterOnSocketTimeoutEvent(System.Action{RapidNetworkLibrary.Connections.Connection},System.Action{RapidNetworkLibrary.Connections.Connection})')
  - [RegisterReceiveEvent(logicReceiveAction,gameReceiveAction)](#M-RapidNetworkLibrary-RNet-RegisterReceiveEvent-System-Action{RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr},System-Action{RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr}- 'RapidNetworkLibrary.RNet.RegisterReceiveEvent(System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr},System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr})')
  - [SendReliable\`\`1(target,messageID,channel,message)](#M-RapidNetworkLibrary-RNet-SendReliable``1-RapidNetworkLibrary-Connections-Connection,System-UInt16,System-Byte,``0- 'RapidNetworkLibrary.RNet.SendReliable``1(RapidNetworkLibrary.Connections.Connection,System.UInt16,System.Byte,``0)')
  - [SendReliable\`\`1(target,messageID,message)](#M-RapidNetworkLibrary-RNet-SendReliable``1-RapidNetworkLibrary-Connections-Connection,System-UInt16,``0- 'RapidNetworkLibrary.RNet.SendReliable``1(RapidNetworkLibrary.Connections.Connection,System.UInt16,``0)')
  - [SendUnreliable\`\`1(target,messageID,channel,message)](#M-RapidNetworkLibrary-RNet-SendUnreliable``1-RapidNetworkLibrary-Connections-Connection,System-UInt16,System-Byte,``0- 'RapidNetworkLibrary.RNet.SendUnreliable``1(RapidNetworkLibrary.Connections.Connection,System.UInt16,System.Byte,``0)')
  - [SendUnreliable\`\`1(target,messageID,message)](#M-RapidNetworkLibrary-RNet-SendUnreliable``1-RapidNetworkLibrary-Connections-Connection,System-UInt16,``0- 'RapidNetworkLibrary.RNet.SendUnreliable``1(RapidNetworkLibrary.Connections.Connection,System.UInt16,``0)')
  - [TearDown()](#M-RapidNetworkLibrary-RNet-TearDown 'RapidNetworkLibrary.RNet.TearDown')
  - [Tick()](#M-RapidNetworkLibrary-RNet-Tick 'RapidNetworkLibrary.RNet.Tick')
  - [UnRegisterOnSocketConnectEvent(socketConnectLogicAction,socketConnectGameAction)](#M-RapidNetworkLibrary-RNet-UnRegisterOnSocketConnectEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}- 'RapidNetworkLibrary.RNet.UnRegisterOnSocketConnectEvent(System.Action{RapidNetworkLibrary.Connections.Connection},System.Action{RapidNetworkLibrary.Connections.Connection})')
  - [UnRegisterOnSocketDisconnectEvent(socketDisconnectLogicAction,socketDisconnectGameAction)](#M-RapidNetworkLibrary-RNet-UnRegisterOnSocketDisconnectEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}- 'RapidNetworkLibrary.RNet.UnRegisterOnSocketDisconnectEvent(System.Action{RapidNetworkLibrary.Connections.Connection},System.Action{RapidNetworkLibrary.Connections.Connection})')
  - [UnRegisterReceiveEvent(logicReceiveAction,gameReceiveAction)](#M-RapidNetworkLibrary-RNet-UnRegisterReceiveEvent-System-Action{RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr},System-Action{RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr}- 'RapidNetworkLibrary.RNet.UnRegisterReceiveEvent(System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr},System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr})')
  - [UnregisterOnSocketTimeoutEvent(socketTimeoutLogicAction,socketTimeoutGameAction)](#M-RapidNetworkLibrary-RNet-UnregisterOnSocketTimeoutEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}- 'RapidNetworkLibrary.RNet.UnregisterOnSocketTimeoutEvent(System.Action{RapidNetworkLibrary.Connections.Connection},System.Action{RapidNetworkLibrary.Connections.Connection})')
- [RNetExtension](#T-RapidNetworkLibrary-Extensions-RNetExtension 'RapidNetworkLibrary.Extensions.RNetExtension')
  - [#ctor(workers)](#M-RapidNetworkLibrary-Extensions-RNetExtension-#ctor-RapidNetworkLibrary-WorkerCollection- 'RapidNetworkLibrary.Extensions.RNetExtension.#ctor(RapidNetworkLibrary.WorkerCollection)')
  - [OnSocketConnect(threadType,connection)](#M-RapidNetworkLibrary-Extensions-RNetExtension-OnSocketConnect-RapidNetworkLibrary-Extensions-ThreadType,RapidNetworkLibrary-Connections-Connection- 'RapidNetworkLibrary.Extensions.RNetExtension.OnSocketConnect(RapidNetworkLibrary.Extensions.ThreadType,RapidNetworkLibrary.Connections.Connection)')
  - [OnSocketDisconnect(threadType,connection)](#M-RapidNetworkLibrary-Extensions-RNetExtension-OnSocketDisconnect-RapidNetworkLibrary-Extensions-ThreadType,RapidNetworkLibrary-Connections-Connection- 'RapidNetworkLibrary.Extensions.RNetExtension.OnSocketDisconnect(RapidNetworkLibrary.Extensions.ThreadType,RapidNetworkLibrary.Connections.Connection)')
  - [OnSocketReceive(threadType,sender,messageID,messageData)](#M-RapidNetworkLibrary-Extensions-RNetExtension-OnSocketReceive-RapidNetworkLibrary-Extensions-ThreadType,RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr- 'RapidNetworkLibrary.Extensions.RNetExtension.OnSocketReceive(RapidNetworkLibrary.Extensions.ThreadType,RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr)')
  - [OnSocketTimeout(threadType,connection)](#M-RapidNetworkLibrary-Extensions-RNetExtension-OnSocketTimeout-RapidNetworkLibrary-Extensions-ThreadType,RapidNetworkLibrary-Connections-Connection- 'RapidNetworkLibrary.Extensions.RNetExtension.OnSocketTimeout(RapidNetworkLibrary.Extensions.ThreadType,RapidNetworkLibrary.Connections.Connection)')
  - [OnThreadMessageReceived(threadType,id,messageData)](#M-RapidNetworkLibrary-Extensions-RNetExtension-OnThreadMessageReceived-RapidNetworkLibrary-Extensions-ThreadType,System-UInt16,System-IntPtr- 'RapidNetworkLibrary.Extensions.RNetExtension.OnThreadMessageReceived(RapidNetworkLibrary.Extensions.ThreadType,System.UInt16,System.IntPtr)')
- [Serializer](#T-RapidNetworkLibrary-Serialization-Serializer 'RapidNetworkLibrary.Serialization.Serializer')
  - [messageID](#F-RapidNetworkLibrary-Serialization-Serializer-messageID 'RapidNetworkLibrary.Serialization.Serializer.messageID')
  - [Deserialize(buffer)](#M-RapidNetworkLibrary-Serialization-Serializer-Deserialize-RapidNetworkLibrary-Serialization-BitBuffer- 'RapidNetworkLibrary.Serialization.Serializer.Deserialize(RapidNetworkLibrary.Serialization.BitBuffer)')
  - [Serialize(buffer,data)](#M-RapidNetworkLibrary-Serialization-Serializer-Serialize-RapidNetworkLibrary-Serialization-BitBuffer,System-IntPtr- 'RapidNetworkLibrary.Serialization.Serializer.Serialize(RapidNetworkLibrary.Serialization.BitBuffer,System.IntPtr)')
- [SerializerAttribute](#T-RapidNetworkLibrary-Serialization-SerializerAttribute 'RapidNetworkLibrary.Serialization.SerializerAttribute')
  - [#ctor()](#M-RapidNetworkLibrary-Serialization-SerializerAttribute-#ctor-System-UInt16- 'RapidNetworkLibrary.Serialization.SerializerAttribute.#ctor(System.UInt16)')
  - [messageID](#F-RapidNetworkLibrary-Serialization-SerializerAttribute-messageID 'RapidNetworkLibrary.Serialization.SerializerAttribute.messageID')
- [SocketWorkerThread](#T-RapidNetworkLibrary-Workers-SocketWorkerThread 'RapidNetworkLibrary.Workers.SocketWorkerThread')
  - [Destroy()](#M-RapidNetworkLibrary-Workers-SocketWorkerThread-Destroy 'RapidNetworkLibrary.Workers.SocketWorkerThread.Destroy')
  - [Init()](#M-RapidNetworkLibrary-Workers-SocketWorkerThread-Init 'RapidNetworkLibrary.Workers.SocketWorkerThread.Init')
  - [Tick()](#M-RapidNetworkLibrary-Workers-SocketWorkerThread-Tick 'RapidNetworkLibrary.Workers.SocketWorkerThread.Tick')
- [ThreadType](#T-RapidNetworkLibrary-Extensions-ThreadType 'RapidNetworkLibrary.Extensions.ThreadType')
  - [Game](#F-RapidNetworkLibrary-Extensions-ThreadType-Game 'RapidNetworkLibrary.Extensions.ThreadType.Game')
  - [Logic](#F-RapidNetworkLibrary-Extensions-ThreadType-Logic 'RapidNetworkLibrary.Extensions.ThreadType.Logic')
  - [Network](#F-RapidNetworkLibrary-Extensions-ThreadType-Network 'RapidNetworkLibrary.Extensions.ThreadType.Network')
- [Worker](#T-RapidNetworkLibrary-Threading-Worker 'RapidNetworkLibrary.Threading.Worker')
  - [Consume()](#M-RapidNetworkLibrary-Threading-Worker-Consume 'RapidNetworkLibrary.Threading.Worker.Consume')
  - [Enqueue(threadMessageID)](#M-RapidNetworkLibrary-Threading-Worker-Enqueue-System-UInt16- 'RapidNetworkLibrary.Threading.Worker.Enqueue(System.UInt16)')
  - [Enqueue\`\`1(threadMessageID,data)](#M-RapidNetworkLibrary-Threading-Worker-Enqueue``1-System-UInt16,``0- 'RapidNetworkLibrary.Threading.Worker.Enqueue``1(System.UInt16,``0)')
  - [Flush()](#M-RapidNetworkLibrary-Threading-Worker-Flush 'RapidNetworkLibrary.Threading.Worker.Flush')
  - [OnDestroy()](#M-RapidNetworkLibrary-Threading-Worker-OnDestroy 'RapidNetworkLibrary.Threading.Worker.OnDestroy')
- [WorkerCollection](#T-RapidNetworkLibrary-WorkerCollection 'RapidNetworkLibrary.WorkerCollection')
  - [gameWorker](#F-RapidNetworkLibrary-WorkerCollection-gameWorker 'RapidNetworkLibrary.WorkerCollection.gameWorker')
  - [logicWorker](#F-RapidNetworkLibrary-WorkerCollection-logicWorker 'RapidNetworkLibrary.WorkerCollection.logicWorker')
  - [socketWorker](#F-RapidNetworkLibrary-WorkerCollection-socketWorker 'RapidNetworkLibrary.WorkerCollection.socketWorker')
- [WorkerThread](#T-RapidNetworkLibrary-Threading-WorkerThread 'RapidNetworkLibrary.Threading.WorkerThread')
  - [#ctor()](#M-RapidNetworkLibrary-Threading-WorkerThread-#ctor 'RapidNetworkLibrary.Threading.WorkerThread.#ctor')
  - [thread](#F-RapidNetworkLibrary-Threading-WorkerThread-thread 'RapidNetworkLibrary.Threading.WorkerThread.thread')
  - [Destroy()](#M-RapidNetworkLibrary-Threading-WorkerThread-Destroy 'RapidNetworkLibrary.Threading.WorkerThread.Destroy')
  - [GetThreadID()](#M-RapidNetworkLibrary-Threading-WorkerThread-GetThreadID 'RapidNetworkLibrary.Threading.WorkerThread.GetThreadID')
  - [Init()](#M-RapidNetworkLibrary-Threading-WorkerThread-Init 'RapidNetworkLibrary.Threading.WorkerThread.Init')
  - [OnDestroy()](#M-RapidNetworkLibrary-Threading-WorkerThread-OnDestroy 'RapidNetworkLibrary.Threading.WorkerThread.OnDestroy')
  - [Tick()](#M-RapidNetworkLibrary-Threading-WorkerThread-Tick 'RapidNetworkLibrary.Threading.WorkerThread.Tick')
- [WorkerThreadHeader](#T-RapidNetworkLibrary-Threading-WorkerThreadHeader 'RapidNetworkLibrary.Threading.WorkerThreadHeader')
  - [data](#F-RapidNetworkLibrary-Threading-WorkerThreadHeader-data 'RapidNetworkLibrary.Threading.WorkerThreadHeader.data')
  - [messageID](#F-RapidNetworkLibrary-Threading-WorkerThreadHeader-messageID 'RapidNetworkLibrary.Threading.WorkerThreadHeader.messageID')

<a name='T-RapidNetworkLibrary-Serialization-BitBuffer'></a>
## BitBuffer `type`

##### Namespace

RapidNetworkLibrary.Serialization

##### Summary

Class used to write values to a buffer that can be converted to a span  of bytes to send over the network. Can also read data from a span of bytes.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-#ctor-System-Int32-'></a>
### #ctor(numberOfBuckets) `constructor`

##### Summary

Creates a new buffer, each bucket of the buffer is 4 bytes meaning passing 375 as the numberOfBuckets will create a buffer that can hold 1500 bytes of data.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| numberOfBuckets | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Number of buffer buckets |

<a name='P-RapidNetworkLibrary-Serialization-BitBuffer-IsFinished'></a>
### IsFinished `property`

##### Summary

returns true if buffer is finished reading or writing.

<a name='P-RapidNetworkLibrary-Serialization-BitBuffer-Length'></a>
### Length `property`

##### Summary

Returns the length of the buffer in bytes.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-Add-System-Int32,System-UInt32-'></a>
### Add(numBits,value) `method`

##### Summary

Adds raw bits to the buffer.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| numBits | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| value | [System.UInt32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt32 'System.UInt32') |  |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-AddBool-System-Boolean-'></a>
### AddBool(value) `method`

##### Summary

Adds a bool to the buffer advancing the position by one byte.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | value to add to the buffer |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-AddByte-System-Byte-'></a>
### AddByte(value) `method`

##### Summary

Adds a byte to the buffer advancing the position one byte.

##### Returns

value peeked from the buffer

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | the value to add to the buffer |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-AddInt-System-Int32-'></a>
### AddInt(value) `method`

##### Summary

Adds an int to the buffer advancing the position 4 bytes.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | the value to add to the buffer |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-AddLong-System-Int64-'></a>
### AddLong(value) `method`

##### Summary

Adds a long to the buffer advancing the position 8 bytes.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Int64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int64 'System.Int64') | the value to add to the buffer. |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-AddShort-System-Int16-'></a>
### AddShort(value) `method`

##### Summary

Adds a short to the buffer advancing the position two bytes.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Int16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int16 'System.Int16') | value to add to the buffer |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-AddUInt-System-UInt32-'></a>
### AddUInt(value) `method`

##### Summary

Adds a uint to the buffer advancing the position 4 bytes.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.UInt32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt32 'System.UInt32') | The value to add to the buffer. |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-AddULong-System-UInt64-'></a>
### AddULong(value) `method`

##### Summary

Adds a ulong to the buffer advancing the position 8 bytes.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.UInt64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt64 'System.UInt64') | the value added to the buffer. |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-AddUShort-System-UInt16-'></a>
### AddUShort(value) `method`

##### Summary

Adds a ushort to the buffer advancing the position 2 bytes.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | The value to add to the buffer. |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-Clear'></a>
### Clear() `method`

##### Summary

clears all data from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-FromArray-System-Byte[],System-Int32-'></a>
### FromArray(data,length) `method`

##### Summary

Creates a buffer of data from the provided array

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [System.Byte[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte[] 'System.Byte[]') |  |
| length | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-FromSpan-System-ReadOnlySpan{System-Byte}@,System-Int32-'></a>
### FromSpan(data,length) `method`

##### Summary

Stores the data from the ReadOnlySpan into the buffer.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [System.ReadOnlySpan{System.Byte}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ReadOnlySpan 'System.ReadOnlySpan{System.Byte}@') |  |
| length | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-Peek-System-Int32-'></a>
### Peek(numBits) `method`

##### Summary

Peeks raw bits from the buffer without advancing the read position.

##### Returns

Value peeked from the buffer

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| numBits | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-PeekBool'></a>
### PeekBool() `method`

##### Summary

Reads a bool from the buffer without advancing the position.

##### Returns



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-PeekByte'></a>
### PeekByte() `method`

##### Summary

Reads a byte from the buffer without advancing the position.

##### Returns

byte peeked from the buffer

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-PeekInt'></a>
### PeekInt() `method`

##### Summary

Reads an int from the buffer without advancing the position.

##### Returns

The value peeked from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-PeekLong'></a>
### PeekLong() `method`

##### Summary

Reads a long from the buffer without advancing the position.

##### Returns

the value peeked from the buffer

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-PeekShort'></a>
### PeekShort() `method`

##### Summary

Reads a short from the buffer without advancing the position.

##### Returns

the value peeked from the buffer

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-PeekUInt'></a>
### PeekUInt() `method`

##### Summary

Reads a uint from the buffer without advancing its position.

##### Returns

The value peeked from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-PeekULong'></a>
### PeekULong() `method`

##### Summary

Reads a ULong from the buffer without advancing the position.

##### Returns



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-PeekUShort'></a>
### PeekUShort() `method`

##### Summary

Reads a  ushort from the buffer without advancing the position.

##### Returns

the value peeked from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-Read-System-Int32-'></a>
### Read(numBits) `method`

##### Summary

reads raw bits from the buffer.+

##### Returns

Value read from the buffer

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| numBits | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ReadBool'></a>
### ReadBool() `method`

##### Summary

Reads a bool from the buffer advancing the position by one byte.

##### Returns

value read from the buffer

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ReadByte'></a>
### ReadByte() `method`

##### Summary

Reads a byte from the buffer advancing the position one byte.

##### Returns

value read from the buffer

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ReadInt'></a>
### ReadInt() `method`

##### Summary

Reads an int from the buffer advancing the position  4 bytes.

##### Returns

the value read from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ReadLong'></a>
### ReadLong() `method`

##### Summary

Reads a long from the buffer advancing the position by 8 bytes.

##### Returns

The value read from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ReadShort'></a>
### ReadShort() `method`

##### Summary

Reads a short from the buffer advancing the position two bytes.

##### Returns

the value read from the buffer

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ReadUInt'></a>
### ReadUInt() `method`

##### Summary

Reads a uint from the buffer advancing the position 4 bytes.

##### Returns

the value read from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ReadULong'></a>
### ReadULong() `method`

##### Summary

Reads a ulong from the buffer advancing the position 8 bytes.

##### Returns

The value read from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ReadUShort'></a>
### ReadUShort() `method`

##### Summary

Reads a ushort from the buffer advancing  the position  2 bytes.

##### Returns

the value read from the buffer.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ToArray-System-Byte[]-'></a>
### ToArray(data) `method`

##### Summary

Stores the data in the buffer to the provided array.

##### Returns

array length

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [System.Byte[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte[] 'System.Byte[]') | array to store the buffer data into. |

<a name='M-RapidNetworkLibrary-Serialization-BitBuffer-ToSpan-System-Span{System-Byte}@-'></a>
### ToSpan(data) `method`

##### Summary

Stores the data in the buffer to the provided Span.

##### Returns

Length of the span.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [System.Span{System.Byte}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Span 'System.Span{System.Byte}@') |  |

<a name='T-RapidNetworkLibrary-Connections-Connection'></a>
## Connection `type`

##### Namespace

RapidNetworkLibrary.Connections

##### Summary

Contains a managed pointer to the connection instance and a cached ID.

<a name='P-RapidNetworkLibrary-Connections-Connection-BytesReceived'></a>
### BytesReceived `property`

##### Summary

returns the total number of bytes received on this connection.

<a name='P-RapidNetworkLibrary-Connections-Connection-BytesSent'></a>
### BytesSent `property`

##### Summary

returns the total number of bytes sent from this connection.

<a name='P-RapidNetworkLibrary-Connections-Connection-ID'></a>
### ID `property`

##### Summary

Returns a numeric id representing this connection.

<a name='P-RapidNetworkLibrary-Connections-Connection-IpAddress'></a>
### IpAddress `property`

##### Summary

returns this connections IPAddress as a NativeString

<a name='P-RapidNetworkLibrary-Connections-Connection-LastReceiveTime'></a>
### LastReceiveTime `property`

##### Summary

returns the last time a packet was received on this connection.

<a name='P-RapidNetworkLibrary-Connections-Connection-LastRoundTripTime'></a>
### LastRoundTripTime `property`

##### Summary

returns the last known round trip time for this connection.

<a name='P-RapidNetworkLibrary-Connections-Connection-LastSendTime'></a>
### LastSendTime `property`

##### Summary

returns the last time a packet  was sent on this connection.

<a name='P-RapidNetworkLibrary-Connections-Connection-Mtu'></a>
### Mtu `property`

##### Summary

returns the connections MTU;

<a name='P-RapidNetworkLibrary-Connections-Connection-PacketsLost'></a>
### PacketsLost `property`

##### Summary

returns the total number of packets received by this connection.

<a name='P-RapidNetworkLibrary-Connections-Connection-PacketsSent'></a>
### PacketsSent `property`

##### Summary

returns the total number of packets sent by this connection.

<a name='P-RapidNetworkLibrary-Connections-Connection-Port'></a>
### Port `property`

##### Summary

returns the port this connection is using.

<a name='M-RapidNetworkLibrary-Connections-Connection-Equals-RapidNetworkLibrary-Connections-Connection-'></a>
### Equals(other) `method`

##### Summary

returns true if this connection equals other connection.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| other | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') |  |

<a name='M-RapidNetworkLibrary-Connections-Connection-Equals-System-Object-'></a>
### Equals(obj) `method`

##### Summary



##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| obj | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') |  |

<a name='M-RapidNetworkLibrary-Connections-Connection-GetHashCode'></a>
### GetHashCode() `method`

##### Summary

Used by Dictionary to determine if this connection is a key.>

##### Returns

id of the connection

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Connections-Connection-ToString'></a>
### ToString() `method`

##### Summary



##### Returns



##### Parameters

This method has no parameters.

<a name='T-LocklessQueue-Queues-MPSCQueue`1-Enumerator'></a>
## Enumerator `type`

##### Namespace

LocklessQueue.Queues.MPSCQueue`1

##### Summary

Defines an enumerator for [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1')

<a name='P-LocklessQueue-Queues-MPSCQueue`1-Enumerator-Current'></a>
### Current `property`

##### Summary

Gets the current object.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-Enumerator-Dispose'></a>
### Dispose() `method`

##### Summary

Disposes the enumerator.

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-Enumerator-MoveNext'></a>
### MoveNext() `method`

##### Summary

Moves the enumerator to the next position.

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-Enumerator-Reset'></a>
### Reset() `method`

##### Summary

Resets the enumerator.

##### Parameters

This method has no parameters.

<a name='T-RapidNetworkLibrary-Workers-GameWorker'></a>
## GameWorker `type`

##### Namespace

RapidNetworkLibrary.Workers

##### Summary



<a name='M-RapidNetworkLibrary-Workers-GameWorker-#ctor-RapidNetworkLibrary-WorkerCollection,RapidNetworkLibrary-Extensions-ExtensionManager-'></a>
### #ctor() `constructor`

##### Parameters

This constructor has no parameters.

<a name='M-RapidNetworkLibrary-Workers-GameWorker-OnDestroy'></a>
### OnDestroy() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='T-RapidNetworkLibrary-Serialization-HalfPrecision'></a>
## HalfPrecision `type`

##### Namespace

RapidNetworkLibrary.Serialization

##### Summary

Helper class used to convert floats to ushorts before sending over the network to preserve bandwidth

<a name='M-RapidNetworkLibrary-Serialization-HalfPrecision-Dequantize-System-UInt16-'></a>
### Dequantize(value) `method`

##### Summary

Takes ushort returned by HalfPrecision.Quantize and returns the original float.

##### Returns

Dequantized Float

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | Quantized Float |

<a name='M-RapidNetworkLibrary-Serialization-HalfPrecision-Quantize-System-Single-'></a>
### Quantize(value) `method`

##### Summary

Converts a float to a ushort

##### Returns

converted ushort

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [System.Single](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Single 'System.Single') | float to convert |

<a name='T-LocklessQueue-Sets-HashHelpers'></a>
## HashHelpers `type`

##### Namespace

LocklessQueue.Sets

<a name='M-LocklessQueue-Sets-HashHelpers-FastMod-System-UInt32,System-UInt32,System-UInt64-'></a>
### FastMod() `method`

##### Summary

Performs a mod operation using the multiplier pre-computed with [GetFastModMultiplier](#M-LocklessQueue-Sets-HashHelpers-GetFastModMultiplier-System-UInt32- 'LocklessQueue.Sets.HashHelpers.GetFastModMultiplier(System.UInt32)').

##### Parameters

This method has no parameters.

##### Remarks

This should only be used on 64-bit.

<a name='M-LocklessQueue-Sets-HashHelpers-GetFastModMultiplier-System-UInt32-'></a>
### GetFastModMultiplier() `method`

##### Summary

Returns approximate reciprocal of the divisor: ceil(2**64 / divisor).

##### Parameters

This method has no parameters.

##### Remarks

This should only be used on 64-bit.

<a name='T-LocklessQueue-IConsumerQueue`1'></a>
## IConsumerQueue\`1 `type`

##### Namespace

LocklessQueue

##### Summary

A common interface that represents the consumer side of a concurrent queue.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | Specifies the type of elements in the queue. |

<a name='P-LocklessQueue-IConsumerQueue`1-IsEmpty'></a>
### IsEmpty `property`

##### Summary

Gets a value that indicates whether the [IConsumerQueue\`1](#T-LocklessQueue-IConsumerQueue`1 'LocklessQueue.IConsumerQueue`1') is empty.

<a name='P-LocklessQueue-IConsumerQueue`1-IsMultiConsumer'></a>
### IsMultiConsumer `property`

##### Summary

Gets a value that indicates whether the [IConsumerQueue\`1](#T-LocklessQueue-IConsumerQueue`1 'LocklessQueue.IConsumerQueue`1') can be used by multiple threads.

<a name='M-LocklessQueue-IConsumerQueue`1-CopyTo-`0[],System-Int32-'></a>
### CopyTo() `method`

##### Summary

Copies the [IConsumerQueue\`1](#T-LocklessQueue-IConsumerQueue`1 'LocklessQueue.IConsumerQueue`1') elements to an existing one-dimensional [Array](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Array 'System.Array'), starting at the specified array index.

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-IConsumerQueue`1-ToArray'></a>
### ToArray() `method`

##### Summary

Copies the elements stored in the [IConsumerQueue\`1](#T-LocklessQueue-IConsumerQueue`1 'LocklessQueue.IConsumerQueue`1') to a new array.

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-IConsumerQueue`1-TryDequeue-`0@-'></a>
### TryDequeue() `method`

##### Summary

Attempts to add the object at the end of the [IConsumerQueue\`1](#T-LocklessQueue-IConsumerQueue`1 'LocklessQueue.IConsumerQueue`1').

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-IConsumerQueue`1-TryPeek-`0@-'></a>
### TryPeek() `method`

##### Summary

Attempts to return an object from the beginning of the [IConsumerQueue\`1](#T-LocklessQueue-IConsumerQueue`1 'LocklessQueue.IConsumerQueue`1')
without removing it.

##### Parameters

This method has no parameters.

<a name='T-IMessageObject'></a>
## IMessageObject `type`

##### Namespace



##### Summary

Interface that all NetworkMessage structs should implement.

<a name='T-LocklessQueue-IProducerConsumerQueue`1'></a>
## IProducerConsumerQueue\`1 `type`

##### Namespace

LocklessQueue

##### Summary

A common interface used for concurrent queues.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | Specifies the type of elements in the queue. |

<a name='T-LocklessQueue-IProducerQueue`1'></a>
## IProducerQueue\`1 `type`

##### Namespace

LocklessQueue

##### Summary

A common interface that represents the producer side of a concurrent collection.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | Specifies the type of elements in the collection. |

<a name='P-LocklessQueue-IProducerQueue`1-IsMultiProducer'></a>
### IsMultiProducer `property`

##### Summary

Gets a value that indicates whether the [IProducerQueue\`1](#T-LocklessQueue-IProducerQueue`1 'LocklessQueue.IProducerQueue`1') can be used by multiple threads.

<a name='M-LocklessQueue-IProducerQueue`1-TryEnqueue-`0-'></a>
### TryEnqueue() `method`

##### Summary

Attempts to add the object at the end of the [IProducerQueue\`1](#T-LocklessQueue-IProducerQueue`1 'LocklessQueue.IProducerQueue`1').

##### Parameters

This method has no parameters.

<a name='T-RapidNetworkLibrary-Logging-LogLevel'></a>
## LogLevel `type`

##### Namespace

RapidNetworkLibrary.Logging

##### Summary

Used to determine the severity level when logging text.

<a name='F-RapidNetworkLibrary-Logging-LogLevel-Error'></a>
### Error `constants`

##### Summary

displays an error. Red text.

<a name='F-RapidNetworkLibrary-Logging-LogLevel-Exception'></a>
### Exception `constants`

##### Summary

displays an exception. Red text.

<a name='F-RapidNetworkLibrary-Logging-LogLevel-Info'></a>
### Info `constants`

##### Summary

just used to  display information. White text.

<a name='F-RapidNetworkLibrary-Logging-LogLevel-Warning'></a>
### Warning `constants`

##### Summary

used  to display a warning. Yellow text.

<a name='T-RapidNetworkLibrary-Logging-Logger'></a>
## Logger `type`

##### Namespace

RapidNetworkLibrary.Logging

##### Summary

Used to log strings of text to the console window.

<a name='M-RapidNetworkLibrary-Logging-Logger-Log-RapidNetworkLibrary-Logging-LogLevel,System-String-'></a>
### Log(level,msg) `method`

##### Summary

Logs a string of text to the console.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| level | [RapidNetworkLibrary.Logging.LogLevel](#T-RapidNetworkLibrary-Logging-LogLevel 'RapidNetworkLibrary.Logging.LogLevel') |  |
| msg | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |

<a name='T-RapidNetworkLibrary-Workers-LogicWorkerThread'></a>
## LogicWorkerThread `type`

##### Namespace

RapidNetworkLibrary.Workers

##### Summary

Used internally by RNet to manage the  logic thread.

<a name='F-RapidNetworkLibrary-Workers-LogicWorkerThread-onLogicInit'></a>
### onLogicInit `constants`

##### Summary

Called from the logic thread before the update loop begins.

<a name='M-RapidNetworkLibrary-Workers-LogicWorkerThread-Destroy'></a>
### Destroy() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Workers-LogicWorkerThread-Init'></a>
### Init() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Workers-LogicWorkerThread-Tick'></a>
### Tick() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='T-LocklessQueue-Queues-MPSCQueue`1'></a>
## MPSCQueue\`1 `type`

##### Namespace

LocklessQueue.Queues

##### Summary

Represents a thread-safe first-in, first-out collection of objects.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | Specifies the type of elements in the queue. |

##### Remarks

Can be used with multiple producer threads and one consumer thread.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-#ctor-System-Int32-'></a>
### #ctor(capacity) `constructor`

##### Summary

Initializes a new instance of the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1') class. Capacity will be set to a power of 2.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| capacity | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The fixed-capacity of this [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1') |

<a name='M-LocklessQueue-Queues-MPSCQueue`1-#ctor-System-Collections-Generic-ICollection{`0}-'></a>
### #ctor(collection) `constructor`

##### Summary

Initializes a new instance of the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1') class that contains elements copied
from the specified collection.
Capacity will be set to a power of 2.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| collection | [System.Collections.Generic.ICollection{\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.ICollection 'System.Collections.Generic.ICollection{`0}') | The collection whose elements are copied to the new [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1'). |

<a name='P-LocklessQueue-Queues-MPSCQueue`1-Capacity'></a>
### Capacity `property`

##### Summary

Gets the capacity of this [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1').

<a name='P-LocklessQueue-Queues-MPSCQueue`1-Count'></a>
### Count `property`

##### Summary

Gets the number of elements contained in the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1').
Value becomes stale after more enqueue or dequeue operations.

<a name='P-LocklessQueue-Queues-MPSCQueue`1-IsEmpty'></a>
### IsEmpty `property`

##### Summary

Gets a value that indicates whether the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1') is empty.
Value becomes stale after more enqueue or dequeue operations.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-Clear'></a>
### Clear() `method`

##### Summary

Removes all objects from the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1').
This method is NOT thread-safe!

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-CopyTo-`0[],System-Int32-'></a>
### CopyTo(array,index) `method`

##### Summary

Copies the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1') elements to an existing [Array](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Array 'System.Array'), starting at the specified array index.
Consumer-Threadsafe

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| array | [\`0[]](#T-`0[] '`0[]') | The one-dimensional [Array](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Array 'System.Array') that is the destination of the elements copied from the
[MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1'). The [Array](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Array 'System.Array') must have zero-based indexing. |
| index | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The zero-based index in array at which copying begins. |

<a name='M-LocklessQueue-Queues-MPSCQueue`1-GetEnumerator'></a>
### GetEnumerator() `method`

##### Summary

Returns an enumerator that iterates through the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1').

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-System#Collections#IEnumerable#GetEnumerator'></a>
### System#Collections#IEnumerable#GetEnumerator() `method`

##### Summary

Returns an enumerator that iterates through a collection.

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-ToArray'></a>
### ToArray() `method`

##### Summary

Copies the elements stored in the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1') to a new array.
Consumer-Threadsafe

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-TryDequeue-`0@-'></a>
### TryDequeue() `method`

##### Summary

Attempts to remove and return the object at the beginning of the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1').
Returns false if the queue is empty.

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-TryEnqueue-`0-'></a>
### TryEnqueue() `method`

##### Summary

Attempts to add the object at the end of the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1').
Returns false if the queue is full.

##### Parameters

This method has no parameters.

<a name='M-LocklessQueue-Queues-MPSCQueue`1-TryPeek-`0@-'></a>
### TryPeek() `method`

##### Summary

Attempts to return an object from the beginning of the [MPSCQueue\`1](#T-LocklessQueue-Queues-MPSCQueue`1 'LocklessQueue.Queues.MPSCQueue`1') without removing it.
Returns false if the queue if empty.

##### Parameters

This method has no parameters.

<a name='T-RapidNetworkLibrary-Memory-MemoryAllocator'></a>
## MemoryAllocator `type`

##### Namespace

RapidNetworkLibrary.Memory

##### Summary

Can inherit from MemoryAllocator class to create your very own high performance allocator.

<a name='M-RapidNetworkLibrary-Memory-MemoryAllocator-Free-System-IntPtr-'></a>
### Free(ptr) `method`

##### Summary

Frees native memory.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ptr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | A pointer to the memory you would like to free. |

<a name='M-RapidNetworkLibrary-Memory-MemoryAllocator-Malloc-System-Int32-'></a>
### Malloc(size) `method`

##### Summary

Allocates native memory of size.

##### Returns

a pointer to the memory allocated.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| size | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | How large of a slice of memory to allocate. |

<a name='T-RapidNetworkLibrary-Memory-MemoryHelper'></a>
## MemoryHelper `type`

##### Namespace

RapidNetworkLibrary.Memory

##### Summary

Utility class used to make working with pointers a little  easier, also a wrapper for the allocator you call in RNet.Init

<a name='M-RapidNetworkLibrary-Memory-MemoryHelper-Alloc-System-Int32-'></a>
### Alloc() `method`

##### Summary

Allocates native memory of size.

##### Returns



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Memory-MemoryHelper-Free-System-IntPtr-'></a>
### Free(ptr) `method`

##### Summary

Frees memory allocated at pointer.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ptr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') |  |

<a name='M-RapidNetworkLibrary-Memory-MemoryHelper-Read``1-System-IntPtr-'></a>
### Read\`\`1(ptr) `method`

##### Summary

Reads a pointer returning `T`

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ptr | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The pointer to the memory holding the value. |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | An unmanaged type to read out of the pointer. |

<a name='M-RapidNetworkLibrary-Memory-MemoryHelper-Write``1-``0-'></a>
### Write\`\`1(value) `method`

##### Summary

Allocates a block of memory equal to the size of T, then stores `value` into  the block.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [\`\`0](#T-``0 '``0') | an unmanaged value type to  store to the memory allocated. |

<a name='T-RapidNetworkLibrary-NativeString'></a>
## NativeString `type`

##### Namespace

RapidNetworkLibrary

##### Summary

An unmanaged class used to represnt string in a blittable way.

<a name='M-RapidNetworkLibrary-NativeString-#ctor-System-String-'></a>
### #ctor(str) `constructor`

##### Summary

Creates a new instance of the NativeString type allocated 2 * string.Length bytes of memory which must be manually freed.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| str | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The managed string to store into the the NativeString |

<a name='M-RapidNetworkLibrary-NativeString-Free'></a>
### Free() `method`

##### Summary

Frees the native memory used for this NativeString.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-NativeString-ToString'></a>
### ToString() `method`

##### Summary

returns a managed string  from the native string.

##### Returns



##### Parameters

This method has no parameters.

<a name='T-RapidNetworkLibrary-RNet'></a>
## RNet `type`

##### Namespace

RapidNetworkLibrary

##### Summary

Static class used to initialize and manage RNet. Everything in this class is safe in all threads at all times.

<a name='P-RapidNetworkLibrary-RNet-BytesReceived'></a>
### BytesReceived `property`

##### Summary

Returns the number of bytes received on this machine.

<a name='P-RapidNetworkLibrary-RNet-BytesSent'></a>
### BytesSent `property`

##### Summary

Returns the number of bytes sent from this machine.

<a name='P-RapidNetworkLibrary-RNet-LastReceiveTime'></a>
### LastReceiveTime `property`

##### Summary

Returns the time that a packet was last received on this machine.

<a name='P-RapidNetworkLibrary-RNet-LastRoundTripTime'></a>
### LastRoundTripTime `property`

##### Summary

Returns the Round Trip Time for the connection on this machine.

<a name='P-RapidNetworkLibrary-RNet-LastSendTime'></a>
### LastSendTime `property`

##### Summary

Returns the time that a packet was last sent on this machine.

<a name='P-RapidNetworkLibrary-RNet-Mtu'></a>
### Mtu `property`

##### Summary

Returns this machines MTU

<a name='P-RapidNetworkLibrary-RNet-PacketsLost'></a>
### PacketsLost `property`

##### Summary

Returns the number of packets lost by this machine.

<a name='P-RapidNetworkLibrary-RNet-PacketsSent'></a>
### PacketsSent `property`

##### Summary

Returns the number of packets sent by this machine.

<a name='M-RapidNetworkLibrary-RNet-BroadcastReliable``1-System-UInt16,System-Byte,``0-'></a>
### BroadcastReliable\`\`1(messageID,channel,message) `method`

##### Summary

Broadcasts a reliable message to every connection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | MessageID of the messasge being sent. |
| channel | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | Channel to send the message on. |
| message | [\`\`0](#T-``0 '``0') | MessageObject to send. |

<a name='M-RapidNetworkLibrary-RNet-BroadcastReliable``1-System-UInt16,``0-'></a>
### BroadcastReliable\`\`1(messageID,message) `method`

##### Summary

Broadcasts a reliable message to every connection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | MessageID of the messasge being sent. |
| message | [\`\`0](#T-``0 '``0') | MessageObject to send. |

<a name='M-RapidNetworkLibrary-RNet-BroadcastUnreliable``1-System-UInt16,System-Byte,``0-'></a>
### BroadcastUnreliable\`\`1(messageID,channel,message) `method`

##### Summary

Broadcasts an unreliable message to every connection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | MessageID of the messasge being sent. |
| channel | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | Channel to send the message on. |
| message | [\`\`0](#T-``0 '``0') | MessageObject to send. |

<a name='M-RapidNetworkLibrary-RNet-BroadcastUnreliable``1-System-UInt16,``0-'></a>
### BroadcastUnreliable\`\`1(messageID,message) `method`

##### Summary

Broadcasts an unreliable message to every connection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | MessageID of the messasge being sent. |
| message | [\`\`0](#T-``0 '``0') | MessageObject to send. |

<a name='M-RapidNetworkLibrary-RNet-Connect-System-String,System-UInt16-'></a>
### Connect(ip,port) `method`

##### Summary

Connects this socket to a server. Can be called from clients or servers.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | IpAddress of the server to connect to. |
| port | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | Port of the server to connect to. |

<a name='M-RapidNetworkLibrary-RNet-Disconnect-RapidNetworkLibrary-Connections-Connection-'></a>
### Disconnect(connection) `method`

##### Summary

Disconnects the passed connection from the server.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| connection | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') |  |

<a name='M-RapidNetworkLibrary-RNet-Disconnect'></a>
### Disconnect() `method`

##### Summary

Disconnects this machine from everything.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-RNet-Init-System-Action,RapidNetworkLibrary-Memory-MemoryAllocator-'></a>
### Init(initAction,alloc) `method`

##### Summary

Initializes RNet,  must be called and initAction  must be invoked before any other methods in RNet are called.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| initAction | [System.Action](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action') | The method invoked when RNet is finished initializing. |
| alloc | [RapidNetworkLibrary.Memory.MemoryAllocator](#T-RapidNetworkLibrary-Memory-MemoryAllocator 'RapidNetworkLibrary.Memory.MemoryAllocator') | Optional parameter used to pass a custom memory allocator to be used by RNet. |

<a name='M-RapidNetworkLibrary-RNet-InitializeServer-System-String,System-UInt16,System-Byte,System-UInt16-'></a>
### InitializeServer(ip,port,maxChannels,maxConnections) `method`

##### Summary

Initializes a server to receive connections.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ip | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ip address the server is bound to. |
| port | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | The port used to establish socket connections |
| maxChannels | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | Maximum amount of channels used by this connection, must match number called in RNet.InitializeClient |
| maxConnections | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') |  |

<a name='M-RapidNetworkLibrary-RNet-RegisterExtension``1'></a>
### RegisterExtension\`\`1() `method`

##### Summary

Registers an RNet extension to be loaded, needs to be called prior to RNet.InitializeServer or RNet.InitializeClient.

##### Parameters

This method has no parameters.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type inheriting from RNetExtension |

<a name='M-RapidNetworkLibrary-RNet-RegisterOnSocketConnectEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}-'></a>
### RegisterOnSocketConnectEvent(socketConnectLogicAction,socketConnectGameAction) `method`

##### Summary

Registers methods to be invoked  after  an incoming connection has been established.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| socketConnectLogicAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the method invoked on the logic thread after a socket connects. |
| socketConnectGameAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the method invoked on the game thread after a socket  connects. |

<a name='M-RapidNetworkLibrary-RNet-RegisterOnSocketDisconnectEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}-'></a>
### RegisterOnSocketDisconnectEvent(socketDisconnectLogicAction,socketDisconnectGameAction) `method`

##### Summary

Registers methods to be invoked after a socket connection has ended gracefully.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| socketDisconnectLogicAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the method to invoke on the logic thread. |
| socketDisconnectGameAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the method to  invoke on the game thread. |

<a name='M-RapidNetworkLibrary-RNet-RegisterOnSocketTimeoutEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}-'></a>
### RegisterOnSocketTimeoutEvent(socketTimeoutLogicAction,socketTimeoutGameAction) `method`

##### Summary

Registers methods to be invoked after a socket connection has not ended gracefully.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| socketTimeoutLogicAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the method to invoke on the logic thread. |
| socketTimeoutGameAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the method to  invoke on the game thread. |

<a name='M-RapidNetworkLibrary-RNet-RegisterReceiveEvent-System-Action{RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr},System-Action{RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr}-'></a>
### RegisterReceiveEvent(logicReceiveAction,gameReceiveAction) `method`

##### Summary

Registers methods to be invoked after a packet has been deserialized into a network message

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| logicReceiveAction | [System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr}') | the method to be invoked on the logic thread. |
| gameReceiveAction | [System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr}') | the method to be invoked on the game thread. |

<a name='M-RapidNetworkLibrary-RNet-SendReliable``1-RapidNetworkLibrary-Connections-Connection,System-UInt16,System-Byte,``0-'></a>
### SendReliable\`\`1(target,messageID,channel,message) `method`

##### Summary

Sends a reliable message to the target connection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| target | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') | Target connection to send the message to. |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | MessageID of the messasge being sent. |
| channel | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | Channel to send the message on. |
| message | [\`\`0](#T-``0 '``0') | MessageObject to send. |

<a name='M-RapidNetworkLibrary-RNet-SendReliable``1-RapidNetworkLibrary-Connections-Connection,System-UInt16,``0-'></a>
### SendReliable\`\`1(target,messageID,message) `method`

##### Summary

Sends an unreliable message to the target connection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| target | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') | Target connection to send the message to. |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | MessageID of the messasge being sent. |
| message | [\`\`0](#T-``0 '``0') | MessageObject to send. |

<a name='M-RapidNetworkLibrary-RNet-SendUnreliable``1-RapidNetworkLibrary-Connections-Connection,System-UInt16,System-Byte,``0-'></a>
### SendUnreliable\`\`1(target,messageID,channel,message) `method`

##### Summary

Sends an unreliable message to the target connection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| target | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') | Target connection to send the message to. |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | MessageID of the messasge being sent. |
| channel | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | Channel to send the message on. |
| message | [\`\`0](#T-``0 '``0') | MessageObject to send. |

<a name='M-RapidNetworkLibrary-RNet-SendUnreliable``1-RapidNetworkLibrary-Connections-Connection,System-UInt16,``0-'></a>
### SendUnreliable\`\`1(target,messageID,message) `method`

##### Summary

Sends an unreliable message to the target connection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| target | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') | Target connection to send the message to. |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | MessageID of the messasge being sent. |
| message | [\`\`0](#T-``0 '``0') | MessageObject to send. |

<a name='M-RapidNetworkLibrary-RNet-TearDown'></a>
### TearDown() `method`

##### Summary

Deinitializes the network and logic thread.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-RNet-Tick'></a>
### Tick() `method`

##### Summary

Called once per frame to keep main thread events flowing from logic thread.

##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-RNet-UnRegisterOnSocketConnectEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}-'></a>
### UnRegisterOnSocketConnectEvent(socketConnectLogicAction,socketConnectGameAction) `method`

##### Summary

Unregisters methods for socket connect event.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| socketConnectLogicAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the logic method to unregister. |
| socketConnectGameAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the game method to unregister. |

<a name='M-RapidNetworkLibrary-RNet-UnRegisterOnSocketDisconnectEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}-'></a>
### UnRegisterOnSocketDisconnectEvent(socketDisconnectLogicAction,socketDisconnectGameAction) `method`

##### Summary

Unregisters socket disconnect methods

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| socketDisconnectLogicAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the logic method to unregister |
| socketDisconnectGameAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the game method to unregister |

<a name='M-RapidNetworkLibrary-RNet-UnRegisterReceiveEvent-System-Action{RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr},System-Action{RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr}-'></a>
### UnRegisterReceiveEvent(logicReceiveAction,gameReceiveAction) `method`

##### Summary

Unregisters receive methods.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| logicReceiveAction | [System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr}') | the logic method to unregister. |
| gameReceiveAction | [System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection,System.UInt16,System.IntPtr}') | the game method to unregister. |

<a name='M-RapidNetworkLibrary-RNet-UnregisterOnSocketTimeoutEvent-System-Action{RapidNetworkLibrary-Connections-Connection},System-Action{RapidNetworkLibrary-Connections-Connection}-'></a>
### UnregisterOnSocketTimeoutEvent(socketTimeoutLogicAction,socketTimeoutGameAction) `method`

##### Summary

Unregisters socket timeout methods

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| socketTimeoutLogicAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the logic method to unregister |
| socketTimeoutGameAction | [System.Action{RapidNetworkLibrary.Connections.Connection}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{RapidNetworkLibrary.Connections.Connection}') | the game method to unregister |

<a name='T-RapidNetworkLibrary-Extensions-RNetExtension'></a>
## RNetExtension `type`

##### Namespace

RapidNetworkLibrary.Extensions

##### Summary

Base class used to implement extensions for RNet.

<a name='M-RapidNetworkLibrary-Extensions-RNetExtension-#ctor-RapidNetworkLibrary-WorkerCollection-'></a>
### #ctor(workers) `constructor`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| workers | [RapidNetworkLibrary.WorkerCollection](#T-RapidNetworkLibrary-WorkerCollection 'RapidNetworkLibrary.WorkerCollection') |  |

<a name='M-RapidNetworkLibrary-Extensions-RNetExtension-OnSocketConnect-RapidNetworkLibrary-Extensions-ThreadType,RapidNetworkLibrary-Connections-Connection-'></a>
### OnSocketConnect(threadType,connection) `method`

##### Summary

called by RNet automatically after a client has connected.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| threadType | [RapidNetworkLibrary.Extensions.ThreadType](#T-RapidNetworkLibrary-Extensions-ThreadType 'RapidNetworkLibrary.Extensions.ThreadType') | The thread that this method was called on. |
| connection | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') | The connection that has connected. |

<a name='M-RapidNetworkLibrary-Extensions-RNetExtension-OnSocketDisconnect-RapidNetworkLibrary-Extensions-ThreadType,RapidNetworkLibrary-Connections-Connection-'></a>
### OnSocketDisconnect(threadType,connection) `method`

##### Summary

called by RNet automatically after a client has disconnected.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| threadType | [RapidNetworkLibrary.Extensions.ThreadType](#T-RapidNetworkLibrary-Extensions-ThreadType 'RapidNetworkLibrary.Extensions.ThreadType') | The thread that this method was called on. |
| connection | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') | The connection that has disconnected. |

<a name='M-RapidNetworkLibrary-Extensions-RNetExtension-OnSocketReceive-RapidNetworkLibrary-Extensions-ThreadType,RapidNetworkLibrary-Connections-Connection,System-UInt16,System-IntPtr-'></a>
### OnSocketReceive(threadType,sender,messageID,messageData) `method`

##### Summary

Called by RNet automatically after a message  has been received.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| threadType | [RapidNetworkLibrary.Extensions.ThreadType](#T-RapidNetworkLibrary-Extensions-ThreadType 'RapidNetworkLibrary.Extensions.ThreadType') | the thread that this method is being called from |
| sender | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') |  |
| messageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') |  |
| messageData | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') |  |

<a name='M-RapidNetworkLibrary-Extensions-RNetExtension-OnSocketTimeout-RapidNetworkLibrary-Extensions-ThreadType,RapidNetworkLibrary-Connections-Connection-'></a>
### OnSocketTimeout(threadType,connection) `method`

##### Summary

called by RNet automatically after a client has timedout.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| threadType | [RapidNetworkLibrary.Extensions.ThreadType](#T-RapidNetworkLibrary-Extensions-ThreadType 'RapidNetworkLibrary.Extensions.ThreadType') | The thread that this method was called on. |
| connection | [RapidNetworkLibrary.Connections.Connection](#T-RapidNetworkLibrary-Connections-Connection 'RapidNetworkLibrary.Connections.Connection') | The connection that has timedout. |

<a name='M-RapidNetworkLibrary-Extensions-RNetExtension-OnThreadMessageReceived-RapidNetworkLibrary-Extensions-ThreadType,System-UInt16,System-IntPtr-'></a>
### OnThreadMessageReceived(threadType,id,messageData) `method`

##### Summary

Called by RNet automatically after a thread message was received. This is not related to the network messages in anyway.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| threadType | [RapidNetworkLibrary.Extensions.ThreadType](#T-RapidNetworkLibrary-Extensions-ThreadType 'RapidNetworkLibrary.Extensions.ThreadType') |  |
| id | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') |  |
| messageData | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') |  |

<a name='T-RapidNetworkLibrary-Serialization-Serializer'></a>
## Serializer `type`

##### Namespace

RapidNetworkLibrary.Serialization

##### Summary

Base class used by the logic thread for serialization

<a name='F-RapidNetworkLibrary-Serialization-Serializer-messageID'></a>
### messageID `constants`

##### Summary

the message id that this serializer is serving

<a name='M-RapidNetworkLibrary-Serialization-Serializer-Deserialize-RapidNetworkLibrary-Serialization-BitBuffer-'></a>
### Deserialize(buffer) `method`

##### Summary

Called on the logic thread when an incoming network message is deserialized.

##### Returns

a pointer to a struct holding the deserialized data, must implement IMessageObject

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| buffer | [RapidNetworkLibrary.Serialization.BitBuffer](#T-RapidNetworkLibrary-Serialization-BitBuffer 'RapidNetworkLibrary.Serialization.BitBuffer') | the buffer to read from |

<a name='M-RapidNetworkLibrary-Serialization-Serializer-Serialize-RapidNetworkLibrary-Serialization-BitBuffer,System-IntPtr-'></a>
### Serialize(buffer,data) `method`

##### Summary

Called on the logic thread when an outoing network message is serialized

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| buffer | [RapidNetworkLibrary.Serialization.BitBuffer](#T-RapidNetworkLibrary-Serialization-BitBuffer 'RapidNetworkLibrary.Serialization.BitBuffer') | The allocated buffer to write to. |
| data | [System.IntPtr](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IntPtr 'System.IntPtr') | The IMessageObject that was passed to RNet.SendMessage pointer. |

<a name='T-RapidNetworkLibrary-Serialization-SerializerAttribute'></a>
## SerializerAttribute `type`

##### Namespace

RapidNetworkLibrary.Serialization

##### Summary

Attribute used on serializers to link them with specific message IDS.

<a name='M-RapidNetworkLibrary-Serialization-SerializerAttribute-#ctor-System-UInt16-'></a>
### #ctor() `constructor`

##### Summary



##### Parameters

This constructor has no parameters.

<a name='F-RapidNetworkLibrary-Serialization-SerializerAttribute-messageID'></a>
### messageID `constants`

##### Summary



<a name='T-RapidNetworkLibrary-Workers-SocketWorkerThread'></a>
## SocketWorkerThread `type`

##### Namespace

RapidNetworkLibrary.Workers

##### Summary



<a name='M-RapidNetworkLibrary-Workers-SocketWorkerThread-Destroy'></a>
### Destroy() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Workers-SocketWorkerThread-Init'></a>
### Init() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Workers-SocketWorkerThread-Tick'></a>
### Tick() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='T-RapidNetworkLibrary-Extensions-ThreadType'></a>
## ThreadType `type`

##### Namespace

RapidNetworkLibrary.Extensions

##### Summary

Enum used to tag  what thread is sending data.

<a name='F-RapidNetworkLibrary-Extensions-ThreadType-Game'></a>
### Game `constants`

##### Summary

The game thread.

<a name='F-RapidNetworkLibrary-Extensions-ThreadType-Logic'></a>
### Logic `constants`

##### Summary

The logic thread.

<a name='F-RapidNetworkLibrary-Extensions-ThreadType-Network'></a>
### Network `constants`

##### Summary

The network thread.

<a name='T-RapidNetworkLibrary-Threading-Worker'></a>
## Worker `type`

##### Namespace

RapidNetworkLibrary.Threading

##### Summary

Base class for workers, are just classes you can enque events into.

<a name='M-RapidNetworkLibrary-Threading-Worker-Consume'></a>
### Consume() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Threading-Worker-Enqueue-System-UInt16-'></a>
### Enqueue(threadMessageID) `method`

##### Summary

Enqueues a event  into the queue. This is thread safe and can be called from any thread always.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| threadMessageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | the event id for the event you're passing. |

<a name='M-RapidNetworkLibrary-Threading-Worker-Enqueue``1-System-UInt16,``0-'></a>
### Enqueue\`\`1(threadMessageID,data) `method`

##### Summary

Enqueues a event  into the queue. This is thread safe and can be called from any thread always.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| threadMessageID | [System.UInt16](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt16 'System.UInt16') | the event id for the event you're passing. |
| data | [\`\`0](#T-``0 '``0') | the event data you're enqueue must be unmanaged |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T |  |

<a name='M-RapidNetworkLibrary-Threading-Worker-Flush'></a>
### Flush() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='M-RapidNetworkLibrary-Threading-Worker-OnDestroy'></a>
### OnDestroy() `method`

##### Summary



##### Parameters

This method has no parameters.

<a name='T-RapidNetworkLibrary-WorkerCollection'></a>
## WorkerCollection `type`

##### Namespace

RapidNetworkLibrary

##### Summary

Holds references to the game, logic, and network  threads.

<a name='F-RapidNetworkLibrary-WorkerCollection-gameWorker'></a>
### gameWorker `constants`

##### Summary

reference to game worker thread.

<a name='F-RapidNetworkLibrary-WorkerCollection-logicWorker'></a>
### logicWorker `constants`

##### Summary

reference to logic worker  thread.

<a name='F-RapidNetworkLibrary-WorkerCollection-socketWorker'></a>
### socketWorker `constants`

##### Summary

refrence to network worker thread.

<a name='T-RapidNetworkLibrary-Threading-WorkerThread'></a>
## WorkerThread `type`

##### Namespace

RapidNetworkLibrary.Threading

##### Summary

A worker that is invoked on a seperate thread.

<a name='M-RapidNetworkLibrary-Threading-WorkerThread-#ctor'></a>
### #ctor() `constructor`

##### Summary



