
using System;
using System.Runtime.InteropServices;


#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
#endif

namespace RapidNetworkLibrary.Memory
{
#if ENABLE_MONO || ENABLE_IL2CPP
    internal class UnityAllocator : MemoryAllocator
    {
        public override unsafe void Free(IntPtr ptr)
        {
#if TRACK
            UntrackAllocation(ptr);
#endif
            UnsafeUtility.Free(ptr.ToPointer(), Unity.Collections.Allocator.Persistent);

        }

        public override unsafe IntPtr Malloc(int size)
        {
            var ptr =  (IntPtr)UnsafeUtility.Malloc(size, 0, Unity.Collections.Allocator.Persistent);
#if TRACK
            TrackAllocation(ptr, Environment.StackTrace);
#endif
            return ptr;
        }
    }
#endif
    internal class RNetAllocator : MemoryAllocator
    {
        public override IntPtr Malloc(int size)
        {
            var ptr = Marshal.AllocHGlobal(size);
#if TRACK
            TrackAllocation(ptr, Environment.StackTrace);
#endif
            return ptr;
        }

        public override void Free(IntPtr ptr)
        {
#if TRACK
            UntrackAllocation(ptr);
#endif
            Marshal.FreeHGlobal(ptr);
        }
    }
}