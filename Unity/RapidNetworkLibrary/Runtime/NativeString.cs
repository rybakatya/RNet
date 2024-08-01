using System;
using System.Runtime.InteropServices;


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
            value = Marshal.AllocHGlobal(characters.Length * Marshal.SizeOf<char>());
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
            Marshal.FreeHGlobal(value);
        }
    }
}



