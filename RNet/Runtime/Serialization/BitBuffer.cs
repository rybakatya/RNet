using System;
using System.Runtime.CompilerServices;
using System.Text;

#if ENABLE_MONO || ENABLE_IL2CPP
	using UnityEngine.Assertions;
#endif

namespace RapidNetworkLibrary.Serialization
{
    /// <summary>
    /// Class used to write values to a buffer that can be converted to a span  of bytes to send over the network. Can also read data from a span of bytes.
    /// </summary>
    public class BitBuffer
    {
        private const int defaultCapacity = 375; // 375 * 4 = 1500 bytes
        private const int stringLengthBits = 8;
        private const int stringLengthMax = (1 << stringLengthBits) - 1; // 255
        private const int bitsASCII = 7;
        private const int growFactor = 2;
        private const int minGrow = 1;
        private int readPosition;
        private int nextPosition;
        private uint[] chunks;

        /// <summary>
        /// Creates a new buffer, each bucket of the buffer is 4 bytes meaning passing 375 as the numberOfBuckets will create a buffer that can hold 1500 bytes of data.
        /// </summary>
        /// <param name="numberOfBuckets">Number of buffer buckets</param>
        public BitBuffer(int numberOfBuckets = defaultCapacity)
        {
            readPosition = 0;
            nextPosition = 0;
            chunks = new uint[numberOfBuckets];
        }
       

        /// <summary>
        /// Returns the length of the buffer in bytes.
        /// </summary>
        public int Length
        {
            get
            {
                return (nextPosition - 1 >> 3) + 1;
            }
        }

        /// <summary>
        /// returns true if buffer is finished reading or writing.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return nextPosition == readPosition;
            }
        }


        /// <summary>
        /// clears all data from the buffer.
        /// </summary>
        [MethodImpl(256)]
        public void Clear()
        {
            readPosition = 0;
            nextPosition = 0;
        }


        /// <summary>
        /// Adds raw bits to the buffer.
        /// </summary>
        /// <param name="numBits"></param>
        /// <param name="value"></param>
        
        [MethodImpl(256)]
        public BitBuffer Add(int numBits, uint value)
        {

            int index = nextPosition >> 5;
            int used = nextPosition & 0x0000001F;

            if (index + 1 >= chunks.Length)
                ExpandArray();

            ulong chunkMask = (1UL << used) - 1;
            ulong scratch = chunks[index] & chunkMask;
            ulong result = scratch | (ulong)value << used;

            chunks[index] = (uint)result;
            chunks[index + 1] = (uint)(result >> 32);
            nextPosition += numBits;

            return this;
        }


        /// <summary>
        /// reads raw bits from the buffer.+
        /// </summary>
        /// <param name="numBits"></param>
        /// <returns>Value read from the buffer</returns>
        [MethodImpl(256)]
        public uint Read(int numBits)
        {
            uint result = Peek(numBits);

            readPosition += numBits;

            return result;
        }


        /// <summary>
        /// Peeks raw bits from the buffer without advancing the read position.
        /// </summary>
        /// <param name="numBits"></param>
        /// <returns>Value peeked from the buffer</returns>
        [MethodImpl(256)]
        public uint Peek(int numBits)
        {


            int index = readPosition >> 5;
            int used = readPosition & 0x0000001F;

            ulong chunkMask = (1UL << numBits) - 1 << used;
            ulong scratch = chunks[index];

            if (index + 1 < chunks.Length)
                scratch |= (ulong)chunks[index + 1] << 32;

            ulong result = (scratch & chunkMask) >> used;

            return (uint)result;
        }


        /// <summary>
        /// Stores the data in the buffer to the provided array.
        /// </summary>
        /// <param name="data">array to store the buffer data into.</param>
        /// <returns>array length</returns>
        public int ToArray(byte[] data)
        {
            Add(1, 1);

            int numChunks = (nextPosition >> 5) + 1;
            int length = data.Length;

            for (int i = 0; i < numChunks; i++)
            {
                int dataIdx = i * 4;
                uint chunk = chunks[i];

                if (dataIdx < length)
                    data[dataIdx] = (byte)chunk;

                if (dataIdx + 1 < length)
                    data[dataIdx + 1] = (byte)(chunk >> 8);

                if (dataIdx + 2 < length)
                    data[dataIdx + 2] = (byte)(chunk >> 16);

                if (dataIdx + 3 < length)
                    data[dataIdx + 3] = (byte)(chunk >> 24);
            }

            return Length;
        }


        /// <summary>
        /// Creates a buffer of data from the provided array
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public void FromArray(byte[] data, int length)
        {
            int numChunks = length / 4 + 1;

            if (chunks.Length < numChunks)
                chunks = new uint[numChunks];

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

                chunks[i] = chunk;
            }

            int positionInByte = FindHighestBitPosition(data[length - 1]);

            nextPosition = (length - 1) * 8 + (positionInByte - 1);
            readPosition = 0;
        }

        /// <summary>
        /// Stores the data in the buffer to the provided Span.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Length of the span.</returns>
        public int ToSpan(ref Span<byte> data) {
            Add(1, 1);

            int numChunks = (nextPosition >> 5) + 1;
            int length = data.Length;

            for (int i = 0; i < numChunks; i++) {
                int dataIdx = i * 4;
                uint chunk = chunks[i];

                if (dataIdx < length)
                    data[dataIdx] = (byte)(chunk);

                if (dataIdx + 1 < length)
                    data[dataIdx + 1] = (byte)(chunk >> 8);

                if (dataIdx + 2 < length)
                    data[dataIdx + 2] = (byte)(chunk >> 16);

                if (dataIdx + 3 < length)
                    data[dataIdx + 3] = (byte)(chunk >> 24);
            }

            return Length;
        }

        /// <summary>
        /// Stores the data from the ReadOnlySpan into the buffer.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
		public void FromSpan(ref ReadOnlySpan<byte> data, int length) 
        {
				int numChunks = (length / 4) + 1;

				if (chunks.Length < numChunks)
					chunks = new uint[numChunks];

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

					chunks[i] = chunk;
				}

				int positionInByte = FindHighestBitPosition(data[length - 1]);

				nextPosition = ((length - 1) * 8) + (positionInByte - 1);
				readPosition = 0;
			}


        /// <summary>
        /// Adds a bool to the buffer advancing the position by one byte.
        /// </summary>
        /// <param name="value">value to add to the buffer</param>

        [MethodImpl(256)]
        public BitBuffer AddBool(bool value)
        {
            Add(1, value ? 1U : 0U);

            return this;
        }


        /// <summary>
        /// Reads a bool from the buffer advancing the position by one byte.
        /// </summary>
        /// <returns>value read from the buffer</returns>
        [MethodImpl(256)]
        public bool ReadBool()
        {
            return Read(1) > 0;
        }

        /// <summary>
        /// Reads a bool from the buffer without advancing the position.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(256)]
        public bool PeekBool()
        {
            return Peek(1) > 0;
        }


        /// <summary>
        /// Adds a byte to the buffer advancing the position one byte.
        /// </summary>
        /// <param name="value">the value to add to the buffer</param>
        /// <returns>value peeked from the buffer</returns>
        [MethodImpl(256)]
        public BitBuffer AddByte(byte value)
        {
            Add(8, value);

            return this;
        }


        /// <summary>
        /// Reads a byte from the buffer advancing the position one byte.
        /// </summary>
        /// <returns>value read from the buffer</returns>
        [MethodImpl(256)]
        public byte ReadByte()
        {
            return (byte)Read(8);
        }


        /// <summary>
        /// Reads a byte from the buffer without advancing the position.
        /// </summary>
        /// <returns>byte peeked from the buffer</returns>
        [MethodImpl(256)]
        public byte PeekByte()
        {
            return (byte)Peek(8);
        }

        /// <summary>
        /// Adds a short to the buffer advancing the position two bytes.
        /// </summary>
        /// <param name="value">value to add to the buffer</param>
        /// <returns></returns>

        [MethodImpl(256)]
        public BitBuffer AddShort(short value)
        {
            AddInt(value);

            return this;
        }


        /// <summary>
        /// Reads a short from the buffer advancing the position two bytes.
        /// </summary>
        /// <returns>the value read from the buffer</returns>
        [MethodImpl(256)]
        public short ReadShort()
        {
            return (short)ReadInt();
        }


        /// <summary>
        /// Reads a short from the buffer without advancing the position.
        /// </summary>
        /// <returns>the value peeked from the buffer</returns>
        [MethodImpl(256)]
        public short PeekShort()
        {
            return (short)PeekInt();
        }


        /// <summary>
        /// Adds a ushort to the buffer advancing the position 2 bytes.
        /// </summary>
        /// <param name="value">The value to add to the buffer.</param>
        [MethodImpl(256)]
        public BitBuffer AddUShort(ushort value)
        {
            AddUInt(value);

            return this;
        }

        /// <summary>
        /// Reads a ushort from the buffer advancing  the position  2 bytes.
        /// </summary>
        /// <returns>the value read from the buffer.</returns>
        [MethodImpl(256)]
        public ushort ReadUShort()
        {
            return (ushort)ReadUInt();
        }

        /// <summary>
        /// Reads a  ushort from the buffer without advancing the position.
        /// </summary>
        /// <returns>the value peeked from the buffer.</returns>
        [MethodImpl(256)]
        public ushort PeekUShort()
        {
            return (ushort)PeekUInt();
        }

        /// <summary>
        /// Adds an int to the buffer advancing the position 4 bytes.
        /// </summary>
        /// <param name="value">the value to add to the buffer</param>
        [MethodImpl(256)]
        public BitBuffer AddInt(int value)
        {
            uint zigzag = (uint)(value << 1 ^ value >> 31);

            AddUInt(zigzag);

            return this;
        }

        /// <summary>
        /// Reads an int from the buffer advancing the position  4 bytes.
        /// </summary>
        /// <returns>the value read from the buffer.</returns>
        [MethodImpl(256)]
        public int ReadInt()
        {
            uint value = ReadUInt();
            int zagzig = (int)(value >> 1 ^ -(int)(value & 1));

            return zagzig;
        }


        /// <summary>
        /// Reads an int from the buffer without advancing the position.
        /// </summary>
        /// <returns>The value peeked from the buffer.</returns>
        [MethodImpl(256)]
        public int PeekInt()
        {
            uint value = PeekUInt();
            int zagzig = (int)(value >> 1 ^ -(int)(value & 1));

            return zagzig;
        }


        /// <summary>
        /// Adds a uint to the buffer advancing the position 4 bytes.
        /// </summary>
        /// <param name="value">The value to add to the buffer.</param>
       
        [MethodImpl(256)]
        public BitBuffer AddUInt(uint value)
        {
            uint buffer = 0x0u;

            do
            {
                buffer = value & 0x7Fu;
                value >>= 7;

                if (value > 0)
                    buffer |= 0x80u;

                Add(8, buffer);
            }

            while (value > 0);

            return this;
        }

        /// <summary>
        /// Reads a uint from the buffer advancing the position 4 bytes.
        /// </summary>
        /// <returns>the value read from the buffer.</returns>
        [MethodImpl(256)]
        public uint ReadUInt()
        {
            uint buffer = 0x0u;
            uint value = 0x0u;
            int shift = 0;

            do
            {
                buffer = Read(8);

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
        public uint PeekUInt()
        {
            int tempPosition = readPosition;
            uint value = ReadUInt();

            readPosition = tempPosition;

            return value;
        }

        /// <summary>
        /// Adds a long to the buffer advancing the position 8 bytes.
        /// </summary>
        /// <param name="value">the value to add to the buffer.</param>
        [MethodImpl(256)]
        public BitBuffer AddLong(long value)
        {
            AddInt((int)(value & uint.MaxValue));
            AddInt((int)(value >> 32));

            return this;
        }

        /// <summary>
        /// Reads a long from the buffer advancing the position by 8 bytes.
        /// </summary>
        /// <returns>The value read from the buffer.</returns>
        [MethodImpl(256)]
        public long ReadLong()
        {
            int low = ReadInt();
            int high = ReadInt();
            long value = high;

            return value << 32 | (uint)low;
        }

        /// <summary>
        /// Reads a long from the buffer without advancing the position.
        /// </summary>
        /// <returns>the value peeked from the buffer</returns>

        [MethodImpl(256)]
        public long PeekLong()
        {
            int tempPosition = readPosition;
            long value = ReadLong();

            readPosition = tempPosition;

            return value;
        }


        /// <summary>
        /// Adds a ulong to the buffer advancing the position 8 bytes.
        /// </summary>
        /// <param name="value">the value added to the buffer.</param>
        [MethodImpl(256)]
        public BitBuffer AddULong(ulong value)
        {
            AddUInt((uint)(value & uint.MaxValue));
            AddUInt((uint)(value >> 32));

            return this;
        }

        /// <summary>
        /// Reads a ulong from the buffer advancing the position 8 bytes.
        /// </summary>
        /// <returns>The value read from the buffer.</returns>
        [MethodImpl(256)]
        public ulong ReadULong()
        {
            uint low = ReadUInt();
            uint high = ReadUInt();

            return (ulong)high << 32 | low;
        }

        /// <summary>
        /// Reads a ULong from the buffer without advancing the position.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(256)]
        public ulong PeekULong()
        {
            int tempPosition = readPosition;
            ulong value = ReadULong();

            readPosition = tempPosition;

            return value;
        }

       

       

        private void ExpandArray()
        {
            int newCapacity = chunks.Length * growFactor + minGrow;
            uint[] newChunks = new uint[newCapacity];

            Array.Copy(chunks, newChunks, chunks.Length);
            chunks = newChunks;
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
