using System;
using System.Collections.Concurrent;
using System.Threading;



#if ENABLE_MONO || ENABLE_IL2CPP
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
#endif

namespace RapidNet.Memory
{

    /// <summary>
    /// Can inherit from MemoryAllocator class to create your very own high performance allocator.
    /// <seealso cref="RNetAllocator"/>
    /// </summary>
    public abstract class MemoryAllocator
    {
#if TRACK
        private ConcurrentDictionary<IntPtr, string> allocations = new ConcurrentDictionary<IntPtr, string>(4, 2048);
#endif
        private static MemoryAllocator alloc;

        /// <summary>
        /// Allocates native memory of size.
        /// </summary>
        /// <param name="size">How large of a slice of memory to allocate.</param>
        /// <returns>a pointer to the memory allocated.</returns>
        public abstract IntPtr Malloc(int size);


        /// <summary>
        /// Frees native memory.
        /// </summary>
        /// <param name="ptr">A pointer to the memory you would like to free.</param>
        public abstract void Free(IntPtr ptr);

#if TRACK
        protected void TrackAllocation(IntPtr pointer, string trace)
        {
            if (alloc == null)
                alloc = this;
            allocations.TryAdd(pointer, trace);
        }


        private static MemoryAllocator instance { get { return alloc; } }
        protected void UntrackAllocation(IntPtr pointer)
        {
            allocations.TryRemove(pointer, out var val);
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                Thread.Sleep(2000);
                UnityEngine.Debug.Log("Exiting Play mode");
                foreach (var kvp in instance.allocations)
                {
                    instance.Free(kvp.Key);
                    UnityEngine.Debug.LogWarningFormat("Detected memory leak, allocated at {0}.", kvp.Value);
                    
                }

                instance.allocations.Clear();
            }
        }
#endif
#endif
    }
}