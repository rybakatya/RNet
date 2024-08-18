
using System;
using System.Runtime.InteropServices;


#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
#endif

namespace RapidNet.Memory
{
    /// <summary>
    /// Utility class used to make working with pointers a little  easier, also a wrapper for the allocator you call in RNet.Init
    /// <seealso cref="RNet.Init(Action, MemoryAllocator)"/>
    /// </summary>
    public static class MemoryHelper
    {
        private static MemoryAllocator allocator;

        /// <summary>
        /// Allocates a block of memory equal to the size of T, then stores <paramref name="value"/> into  the block.
        /// </summary>
        /// <param name="value">an unmanaged value type to  store to the memory allocated.</param>
        /// <returns></returns>
        public static unsafe IntPtr Write<T>(T value) where T : unmanaged
        {
#if ENABLE_MONO || ENABLE_IL2CPP
            var ptr = Alloc(UnsafeUtility.SizeOf<T>());
            var span = new Span<T>(ptr.ToPointer(), UnsafeUtility.SizeOf<T>());
#else
        var ptr = Alloc(Marshal.SizeOf<T>());
        var span = new Span<T>(ptr.ToPointer(), Marshal.SizeOf<T>());
#endif
            span[0] = value;


            return ptr;
        }


        /// <summary>
        /// Reads a pointer returning <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">An unmanaged type to read out of the pointer.</typeparam>
        /// <param name="ptr">The pointer to the memory holding the value.</param>
        /// <returns></returns>
        public static unsafe T Read<T>(IntPtr ptr) where T : unmanaged
        {
            var s = new ReadOnlySpan<T>(ptr.ToPointer(), 1);
            return s[0];
        }




        internal static void SetMalloc(MemoryAllocator alloc)
        {
            allocator = alloc;
        }


        /// <summary>
        /// Allocates native memory of size.
        /// </summary>
        /// <returns></returns>
        public static unsafe IntPtr Alloc(int size)
        {
            return allocator.Malloc(size);
        }


        /// <summary>
        /// Frees memory allocated at pointer.
        /// </summary>
        /// <param name="ptr"></param>
        public static unsafe void Free(IntPtr ptr)
        {
            allocator.Free(ptr);
        }
    }
}