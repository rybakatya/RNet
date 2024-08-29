using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

#if ENABLE_MONO || ENABLE_IL2CPP
	using UnityEngine.Assertions;
#endif

namespace RapidNet.Serialization
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BitBufferData
    {
        public int readPosition;
        public int nextPosition;
        public uint[] chunks;
    }
    public static class BitBuffer
    {
        private const int defaultCapacity = 375; // 375 * 4 = 1500 bytes
        private const int stringLengthBits = 8;
        private const int stringLengthMax = (1 << stringLengthBits) - 1; // 255
        private const int bitsASCII = 7;
        private const int growFactor = 2;
        private const int minGrow = 1;


        private static RapidNet.Buffers.ArrayPool<uint> bits = Buffers.ArrayPool<uint>.Create(2048, 32);
        public static BitBufferData Create(int  length)
        {
            return new BitBufferData()
            {
                readPosition = 0,
                nextPosition = 0,
                chunks = bits.Rent(length)
            };
        }
        
        public static void Destroy(ref BitBufferData buffer)
        {
            bits.Return(buffer.chunks, true);
        }

        /// <summary>
        /// Returns the length of the buffer in bytes.
        /// </summary>
        public static int GetLength(ref  BitBufferData buffer) 
        {
            return (buffer.nextPosition - 1 >> 3) + 1;
            
        }

        


        /// <summary>
        /// clears all data from the buffer.
        /// </summary>
        [MethodImpl(256)]
        public static void Clear(ref BitBufferData  buffer)
        {
            buffer.readPosition = 0;
            buffer.nextPosition = 0;
        }


        /// <summary>
        /// Adds raw bits to the buffer.
        /// </summary>
        /// <param name="numBits"></param>
        /// <param name="value"></param>
        
        [MethodImpl(256)]
        public static void Add(ref BitBufferData buffer, int numBits, uint value)
        {

            int index = buffer.nextPosition >> 5;
            int used = buffer.nextPosition & 0x0000001F;

            if (index + 1 >= buffer.chunks.Length)
                ExpandArray(ref buffer);

            ulong chunkMask = (1UL << used) - 1;
            ulong scratch = buffer.chunks[index] & chunkMask;
            ulong result = scratch | (ulong)value << used;

            buffer.chunks[index] = (uint)result;
            buffer.chunks[index + 1] = (uint)(result >> 32);
            buffer.nextPosition += numBits;
        }


        /// <summary>
        /// reads raw bits from the buffer.+
        /// </summary>
        /// <param name="numBits"></param>
        /// <returns>Value read from the buffer</returns>
        [MethodImpl(256)]
        public static uint Read(ref BitBufferData buffer, int numBits)
        {
            uint result = Peek(ref buffer, numBits);

            buffer.readPosition += numBits;

            return result;
        }


        /// <summary>
        /// Peeks raw bits from the buffer without advancing the read position.
        /// </summary>
        /// <param name="numBits"></param>
        /// <returns>Value peeked from the buffer</returns>
        [MethodImpl(256)]
        public static uint Peek(ref BitBufferData buffer, int numBits)
        {


            int index = buffer.readPosition >> 5;
            int used = buffer.readPosition & 0x0000001F;

            ulong chunkMask = (1UL << numBits) - 1 << used;
            ulong scratch = buffer.chunks[index];

            if (index + 1 < buffer.chunks.Length)
                scratch |= (ulong)buffer.chunks[index + 1] << 32;

            ulong result = (scratch & chunkMask) >> used;

            return (uint)result;
        }


        /// <summary>
        /// Stores the data in the buffer to the provided array.
        /// </summary>
        /// <param name="data">array to store the buffer data into.</param>
        /// <returns>array length</returns>
        public static int ToArray(ref BitBufferData buffer, byte[] data)
        {
            Add(ref buffer, 1, 1);

            int numChunks = (buffer.nextPosition >> 5) + 1;
            int length = data.Length;

            for (int i = 0; i < numChunks; i++)
            {
                int dataIdx = i * 4;
                uint chunk = buffer.chunks[i];

                if (dataIdx < length)
                    data[dataIdx] = (byte)chunk;

                if (dataIdx + 1 < length)
                    data[dataIdx + 1] = (byte)(chunk >> 8);

                if (dataIdx + 2 < length)
                    data[dataIdx + 2] = (byte)(chunk >> 16);

                if (dataIdx + 3 < length)
                    data[dataIdx + 3] = (byte)(chunk >> 24);
            }

            return GetLength(ref buffer);
        }


        /// <summary>
        /// Creates a buffer of data from the provided array
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public static void FromArray(ref BitBufferData buffer, byte[] data, int length)
        {
            int numChunks = length / 4 + 1;

            if (buffer.chunks.Length < numChunks)
                buffer.chunks = new uint[numChunks];

            for (int i = 0; i < numChunks; i++)
            {
                int dataIdx = i * 4;
                uint chunk = 0;

                if (dataIdx < length)
                    chunk = data[dataIdx];

                if (dataIdx + 1 < length)
                    chunk = chunk | (uint)data[dataIdx + 1] << 8;

                if (dataIdx + 2 < length)
                    chunk = chunk | (uint)data[dataIdx + 2] << 16;

                if (dataIdx + 3 < length)
                    chunk = chunk | (uint)data[dataIdx + 3] << 24;

                buffer.chunks[i] = chunk;
            }

            int positionInByte = FindHighestBitPosition(data[length - 1]);

            buffer.nextPosition = (length - 1) * 8 + (positionInByte - 1);
            buffer.readPosition = 0;
        }

        /// <summary>
        /// Stores the data in the buffer to the provided Span.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Length of the span.</returns>
        public static int ToSpan(ref BitBufferData buffer, ref Span<byte> data) {
            Add(ref buffer, 1, 1);

            int numChunks = (buffer.nextPosition >> 5) + 1;
            int length = data.Length;

            for (int i = 0; i < numChunks; i++) {
                int dataIdx = i * 4;
                uint chunk = buffer.chunks[i];

                if (dataIdx < length)
                    data[dataIdx] = (byte)(chunk);

                if (dataIdx + 1 < length)
                    data[dataIdx + 1] = (byte)(chunk >> 8);

                if (dataIdx + 2 < length)
                    data[dataIdx + 2] = (byte)(chunk >> 16);

                if (dataIdx + 3 < length)
                    data[dataIdx + 3] = (byte)(chunk >> 24);
            }

            return GetLength(ref buffer);
        }

        /// <summary>
        /// Stores the data from the ReadOnlySpan into the buffer.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
		public static void FromSpan(ref BitBufferData buffer, ref ReadOnlySpan<byte> data, int length) 
        {
				int numChunks = (length / 4) + 1;

				if (buffer.chunks.Length < numChunks)
					buffer.chunks = new uint[numChunks];

				for (int i = 0; i < numChunks; i++) {
					int dataIdx = i * 4;
					uint chunk = 0;

					if (dataIdx < length)
						chunk = (uint)data[dataIdx];

					if (dataIdx + 1 < length)
 						chunk = chunk | (uint)data[dataIdx + 1] << 8;

					if (dataIdx + 2 < length)
						chunk = chunk | (uint)data[dataIdx + 2] << 16;

					if (dataIdx + 3 < length)
						chunk = chunk | (uint)data[dataIdx + 3] << 24;

					buffer.chunks[i] = chunk;
				}

				int positionInByte = FindHighestBitPosition(data[length - 1]);

				buffer.nextPosition = ((length - 1) * 8) + (positionInByte - 1);
				buffer.readPosition = 0;
			}


        /// <summary>
        /// Adds a bool to the buffer advancing the position by one byte.
        /// </summary>
        /// <param name="value">value to add to the buffer</param>

        [MethodImpl(256)]
        public static void AddBool(ref BitBufferData buffer, bool value)
        {
            Add(ref buffer, 1, value ? 1U : 0U);
        }


        /// <summary>
        /// Reads a bool from the buffer advancing the position by one byte.
        /// </summary>
        /// <returns>value read from the buffer</returns>
        [MethodImpl(256)]
        public static bool ReadBool(ref BitBufferData buffer)
        {
            return Read(ref buffer, 1) > 0;
        }

        /// <summary>
        /// Reads a bool from the buffer without advancing the position.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(256)]
        public static bool PeekBool(ref BitBufferData buffer)
        {
            return Peek(ref buffer, 1) > 0;
        }


        /// <summary>
        /// Adds a byte to the buffer advancing the position one byte.
        /// </summary>
        /// <param name="value">the value to add to the buffer</param>
        /// <returns>value peeked from the buffer</returns>
        [MethodImpl(256)]
        public static void AddByte(ref BitBufferData buffer, byte value)
        {
            Add(ref buffer, 8, value);
        }


        /// <summary>
        /// Reads a byte from the buffer advancing the position one byte.
        /// </summary>
        /// <returns>value read from the buffer</returns>
        [MethodImpl(256)]
        public static byte ReadByte(ref BitBufferData buffer)
        {
            return (byte)Read(ref buffer, 8);
        }


        /// <summary>
        /// Reads a byte from the buffer without advancing the position.
        /// </summary>
        /// <returns>byte peeked from the buffer</returns>
        [MethodImpl(256)]
        public static byte PeekByte(ref BitBufferData buffer)
        {
            return (byte)Peek(ref buffer, 8);
        }

        /// <summary>
        /// Adds a short to the buffer advancing the position two bytes.
        /// </summary>
        /// <param name="value">value to add to the buffer</param>
        /// <returns></returns>

        [MethodImpl(256)]
        public static void AddShort(ref BitBufferData buffer, short value)
        {
            AddInt(ref buffer, value);
        }


        /// <summary>
        /// Reads a short from the buffer advancing the position two bytes.
        /// </summary>
        /// <returns>the value read from the buffer</returns>
        [MethodImpl(256)]
        public static short ReadShort(ref BitBufferData buffer)
        {
            return (short)ReadInt(ref buffer);
        }


        /// <summary>
        /// Reads a short from the buffer without advancing the position.
        /// </summary>
        /// <returns>the value peeked from the buffer</returns>
        [MethodImpl(256)]
        public static short PeekShort(ref BitBufferData buffer)
        {
            return (short)PeekInt(ref buffer);
        }


        /// <summary>
        /// Adds a ushort to the buffer advancing the position 2 bytes.
        /// </summary>
        /// <param name="value">The value to add to the buffer.</param>
        [MethodImpl(256)]
        public static void AddUShort(ref BitBufferData buffer, ushort value)
        {
            AddUInt(ref buffer, value);

        }

        /// <summary>
        /// Reads a ushort from the buffer advancing  the position  2 bytes.
        /// </summary>
        /// <returns>the value read from the buffer.</returns>
        [MethodImpl(256)]
        public static ushort ReadUShort(ref BitBufferData buffer)
        {
            return (ushort)ReadUInt(ref buffer);
        }

        /// <summary>
        /// Reads a  ushort from the buffer without advancing the position.
        /// </summary>
        /// <returns>the value peeked from the buffer.</returns>
        [MethodImpl(256)]
        public static ushort PeekUShort(ref BitBufferData buffer)
        {
            return (ushort)PeekUInt(ref buffer);
        }

        /// <summary>
        /// Adds an int to the buffer advancing the position 4 bytes.
        /// </summary>
        /// <param name="value">the value to add to the buffer</param>
        [MethodImpl(256)]
        public static void AddInt(ref BitBufferData buffer, int value)
        {
            uint zigzag = (uint)(value << 1 ^ value >> 31);

            AddUInt(ref buffer, zigzag);
        }

        /// <summary>
        /// Reads an int from the buffer advancing the position  4 bytes.
        /// </summary>
        /// <returns>the value read from the buffer.</returns>
        [MethodImpl(256)]
        public static int ReadInt(ref BitBufferData buffer)
        {
            uint value = ReadUInt(ref buffer);
            int zagzig = (int)(value >> 1 ^ -(int)(value & 1));

            return zagzig;
        }


        /// <summary>
        /// Reads an int from the buffer without advancing the position.
        /// </summary>
        /// <returns>The value peeked from the buffer.</returns>
        [MethodImpl(256)]
        public static int PeekInt(ref BitBufferData buffer)
        {
            uint value = PeekUInt(ref buffer);
            int zagzig = (int)(value >> 1 ^ -(int)(value & 1));

            return zagzig;
        }


        /// <summary>
        /// Adds a uint to the buffer advancing the position 4 bytes.
        /// </summary>
        /// <param name="value">The value to add to the buffer.</param>
       
        [MethodImpl(256)]
        public static void AddUInt(ref BitBufferData data, uint value)
        {
            uint buffer = 0x0u;

            do
            {
                buffer = value & 0x7Fu;
                value >>= 7;

                if (value > 0)
                    buffer |= 0x80u;

                Add(ref data, 8, buffer);
            }

            while (value > 0);
        }

        /// <summary>
        /// Reads a uint from the buffer advancing the position 4 bytes.
        /// </summary>
        /// <returns>the value read from the buffer.</returns>
        [MethodImpl(256)]
        public static uint ReadUInt(ref BitBufferData data)
        {
            uint buffer = 0x0u;
            uint value = 0x0u;
            int shift = 0;

            do
            {
                buffer = Read(ref data, 8);

                value |= (buffer & 0x7Fu) << shift;
                shift += 7;
            }

            while ((buffer & 0x80u) > 0);

            return value;
        }


        /// <summary>
        /// Reads a uint from the buffer without advancing its position.
        /// </summary>
        /// <returns>The value peeked from the buffer.</returns>
        [MethodImpl(256)]
        public static uint PeekUInt(ref BitBufferData buffer)
        {
            int tempPosition = buffer.readPosition;
            uint value = ReadUInt(ref buffer);

            buffer.readPosition = tempPosition;

            return value;
        }

        /// <summary>
        /// Adds a long to the buffer advancing the position 8 bytes.
        /// </summary>
        /// <param name="value">the value to add to the buffer.</param>
        [MethodImpl(256)]
        public static void  AddLong(ref BitBufferData buffer, long value)
        {
            AddInt(ref buffer, (int)(value & uint.MaxValue));
            AddInt(ref buffer, (int)(value >> 32));
        }

        /// <summary>
        /// Reads a long from the buffer advancing the position by 8 bytes.
        /// </summary>
        /// <returns>The value read from the buffer.</returns>
        [MethodImpl(256)]
        public static long ReadLong(ref BitBufferData buffer)
        {
            int low = ReadInt(ref buffer);
            int high = ReadInt(ref buffer);
            long value = high;

            return value << 32 | (uint)low;
        }

        /// <summary>
        /// Reads a long from the buffer without advancing the position.
        /// </summary>
        /// <returns>the value peeked from the buffer</returns>

        [MethodImpl(256)]
        public static long PeekLong(ref BitBufferData buffer)
        {
            int tempPosition = buffer.readPosition;
            long value = ReadLong(ref buffer);

            buffer.readPosition = tempPosition;

            return value;
        }


        /// <summary>
        /// Adds a ulong to the buffer advancing the position 8 bytes.
        /// </summary>
        /// <param name="value">the value added to the buffer.</param>
        [MethodImpl(256)]
        public static void AddULong(ref BitBufferData buffer, ulong value)
        {
            AddUInt(ref buffer, (uint)(value & uint.MaxValue));
            AddUInt(ref buffer, (uint)(value >> 32));
        }

        /// <summary>
        /// Reads a ulong from the buffer advancing the position 8 bytes.
        /// </summary>
        /// <returns>The value read from the buffer.</returns>
        [MethodImpl(256)]
        public static  ulong ReadULong(ref BitBufferData buffer)
        {
            uint low = ReadUInt(ref buffer);
            uint high = ReadUInt(ref buffer);

            return (ulong)high << 32 | low;
        }

        /// <summary>
        /// Reads a ULong from the buffer without advancing the position.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(256)]
        public static ulong PeekULong(ref BitBufferData buffer)
        {
            int tempPosition = buffer.readPosition;
            ulong value = ReadULong(ref buffer);

            buffer.readPosition = tempPosition;

            return value;
        }

       

       

        private static void ExpandArray(ref BitBufferData buffer)
        {
            int newCapacity = buffer.chunks.Length * growFactor + minGrow;
            uint[] newChunks = new uint[newCapacity];

            Array.Copy(buffer.chunks, newChunks, buffer.chunks.Length);
            buffer.chunks = newChunks;
        }

        [MethodImpl(256)]
        private static int FindHighestBitPosition(byte data)
        {
            int shiftCount = 0;

            while (data > 0)
            {
                data >>= 1;
                shiftCount++;
            }

            return shiftCount;
        }

        private static byte ToASCII(char character)
        {
            byte value = 0;

            try
            {
                value = Convert.ToByte(character);
            }

            catch (OverflowException)
            {
                throw new Exception("Cannot convert to ASCII: " + character);
            }

            if (value > 127)
                throw new Exception("Cannot convert to ASCII: " + character);

            return value;
        }
    }
}
