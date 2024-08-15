
using System;
using System.Runtime.InteropServices;


#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
#endif

namespace RapidNetworkLibrary.Memory
{
    public static class MemoryHelper
    {
        private static MemoryAllocator allocator;
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

        public static unsafe T Read<T>(IntPtr ptr) where T : unmanaged
        {
            var s = new ReadOnlySpan<T>(ptr.ToPointer(), 1);
            return s[0];
        }




        internal static void SetMalloc(MemoryAllocator alloc)
        {
            allocator = alloc;
        }

        public static unsafe IntPtr Alloc(int size)
        {
            return allocator.Malloc(size);
        }

        public static unsafe void Free(IntPtr ptr)
        {
            allocator.Free(ptr);
        }
    }
}