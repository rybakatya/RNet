using RapidNetworkLibrary.Memory;
using System;
using System.Runtime.InteropServices;

#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
#endif 

namespace RapidNetworkLibrary
{
   
    public unsafe struct NativeString
    {
        public int length;
        public IntPtr value;


        public unsafe NativeString(string str)
        {
            var characters = str.AsSpan();           
            length = characters.Length;
            value = MemoryHelper.Alloc((characters.Length * Marshal.SizeOf<char>()) + 12);
           
            var span = new Span<char>(value.ToPointer(), length);
            for (int i = 0; i < characters.Length; i++)
            {
                span[i] = characters[i];
            }

        }

        public override string ToString()
        {
            var span = new ReadOnlySpan<char>(value.ToPointer(), length);
            var s = new string(span);
            return s;
        }
        public void Free()
        {
            MemoryHelper.Free(value);
        }
    }
}



