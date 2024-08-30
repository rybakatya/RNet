
using System.Threading;
using System.Runtime.CompilerServices;


namespace RapidNet.Buffers
{
    internal abstract class ArrayPool<T>
    {
#if NET_4_6 || NET_STANDARD_2_0
			private static ArrayPool<T> s_sharedInstance = null;
#else
        private static volatile ArrayPool<T> s_sharedInstance = null;
#endif

        public static ArrayPool<T> Shared
        {
#if NET_4_6 || NET_STANDARD_2_0
				[MethodImpl(256)]
				get {
					return Volatile.Read(ref s_sharedInstance) ?? EnsureSharedCreated();
				}
#else
            [MethodImpl(256)]
            get
            {
                return s_sharedInstance ?? EnsureSharedCreated();
            }
#endif
        }

#pragma warning disable 420

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArrayPool<T> EnsureSharedCreated()
        {
            Interlocked.CompareExchange(ref s_sharedInstance, Create(), null);

            return s_sharedInstance;
        }

        public static ArrayPool<T> Create()
        {
            return new DefaultArrayPool<T>();
        }

        public static ArrayPool<T> Create(int maxArrayLength, int maxArraysPerBucket)
        {
            return new DefaultArrayPool<T>(maxArrayLength, maxArraysPerBucket);
        }

        public abstract T[] Rent(int minimumLength);

        public abstract void Return(T[] array, bool clearArray = false);
    }
}