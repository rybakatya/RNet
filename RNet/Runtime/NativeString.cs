using RapidNetworkLibrary.Memory;
using System;
using System.Runtime.InteropServices;

#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
#endif 

namespace RapidNetworkLibrary
{
    /// <summary>
    /// An unmanaged class used to represnt string in a blittable way.
    /// </summary>
    public unsafe struct NativeString
    {
        internal int length;
        internal IntPtr value;

        /// <summary>
        /// Creates a new instance of the NativeString type allocated 2 * string.Length bytes of memory which must be manually freed.
        /// </summary>
        /// <param name="str">The managed string to store into the the NativeString</param>
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


        /// <summary>
        /// returns a managed string  from the native string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var span = new ReadOnlySpan<char>(value.ToPointer(), length);
            var s = new string(span);
            return s;
        }

        /// <summary>
        /// Frees the native memory used for this NativeString.
        /// </summary>
        public void Free()
        {
            MemoryHelper.Free(value);
        }
    }
}



