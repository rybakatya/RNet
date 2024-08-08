
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;


#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
#endif

namespace RapidNetworkLibrary.Memory
{
    static unsafe class AllocationTracker
    {
        static readonly ConcurrentDictionary<IntPtr, string> s_Allocations = new ConcurrentDictionary<IntPtr, string>();
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                UnityEngine.Debug.Log("Exiting Play mode");
                foreach (string trace in s_Allocations.Values)
                {
                    UnityEngine.Debug.LogWarningFormat("Detected memory leak, allocated at {0}.", trace);
                }

                s_Allocations.Clear();
            }
        }
#endif

        public static void TrackAllocation(void* pointer, string trace)
        {
            s_Allocations.TryAdd((IntPtr)pointer, trace);
        }

        public static void UntrackAllocation(void* pointer)
        {
            s_Allocations.TryRemove((IntPtr)pointer, out var val);
        }
    }


    public abstract class MemoryAllocator
    {
        private ConcurrentDictionary<IntPtr, string> allocations = new ConcurrentDictionary<IntPtr, string>();

        public abstract IntPtr Malloc(int size);
        public abstract void Free(IntPtr ptr);

        protected void TrackAllocation(IntPtr pointer, string trace)
        {
            allocations.TryAdd(pointer, trace);
        }

        protected void UntrackAllocation(IntPtr pointer)
        {
            allocations.TryRemove(pointer, out var val);
        }
    }
#if ENABLE_MONO || ENABLE_IL2CPP
    public class UnityAllocator : MemoryAllocator
    {
        public override unsafe void Free(IntPtr ptr)
        {
            UntrackAllocation(ptr);
            UnsafeUtility.Free(ptr.ToPointer(), Unity.Collections.Allocator.Persistent);

        }

        public override unsafe IntPtr Malloc(int size)
        {
            var ptr =  (IntPtr)UnsafeUtility.Malloc(size, 0, Unity.Collections.Allocator.Persistent);
            TrackAllocation(ptr, Environment.StackTrace);
            return ptr;
        }
    }
#endif
    public class RNetAllocator : MemoryAllocator
    {
        public override IntPtr Malloc(int size)
        {
            var ptr = Marshal.AllocHGlobal(size);
            TrackAllocation(ptr, Environment.StackTrace);
            return ptr;
        }

        public override void Free(IntPtr ptr)
        {
            UntrackAllocation(ptr);
            Marshal.FreeHGlobal(ptr);
        }
    }
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