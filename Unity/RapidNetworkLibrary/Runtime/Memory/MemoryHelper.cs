using RapidNetworkLibrary.Runtime.Memory;
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;


#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
#endif


static unsafe class AllocationTracker
{
    static readonly ConcurrentDictionary<IntPtr, string> s_Allocations = new ConcurrentDictionary<IntPtr, string>();
#if ENABLE_MONO || ENABLE_IL2CPP
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
public static class MemoryHelper
{
    private static SmmallocInstance smalloc;
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

    


   
    public static unsafe IntPtr Alloc(int size)
    {
#if ENABLE_MONO || ENABLE_IL2CPP
        var ptr = (IntPtr)UnsafeUtility.Malloc(size, 0, Unity.Collections.Allocator.Persistent);
#else
        if (size > smalloc.allocationLimit)
            throw new Exception("Cannot allocate memory so large using smmalloc");

        var ptr = smalloc.Malloc(size);
#endif
        AllocationTracker.TrackAllocation(ptr.ToPointer(), Environment.StackTrace);
        return ptr;
    }

    public static unsafe void Free(IntPtr ptr)
    {
        
        //smalloc.Free(ptr);
        AllocationTracker.UntrackAllocation(ptr.ToPointer());
#if ENABLE_MONO || ENABLE_IL2CPP
        UnsafeUtility.FreeTracked(ptr.ToPointer(), Unity.Collections.Allocator.Persistent);
#else
        smalloc.Free(ptr);
#endif

    }

    internal static void SetMalloc(SmmallocInstance malloc)
    {
        smalloc = malloc;
    }
}
