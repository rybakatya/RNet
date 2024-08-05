#if SERVER
using System;
using System.Runtime.CompilerServices;

namespace RapidNetworkLibrary.Runtime.Zones
{
    public struct Rect : IEquatable<Rect>
    {
        
        private float m_XMin;

        private float m_YMin;

        private float m_Width;
 
        private float m_Height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rect(float x, float y, float width, float height)
        {
            m_XMin = x;
            m_YMin = y;
            m_Width = width;
            m_Height = height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Overlaps(Rect other)
        {
            return (other.xMax > xMin &&
                other.xMin < xMax &&
                other.yMax > yMin &&
                other.yMin < yMax);
        }

        public bool Equals(Rect other)
        {
            if (m_XMin == other.m_XMin && m_YMin == other.m_YMin && m_Height == other.m_Height && m_Width == other.m_Width)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return (((int)m_XMin ^ (int)m_YMin) ^ (int)m_Width ^ (int)m_Height);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return string.Format("{ X:{0} Z:{1} Width:{2} Height{3}", m_XMin, m_YMin, m_Width, m_Height).ToString();
        }
        public float xMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return m_XMin; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { float oldxmax = xMax; m_XMin = value; m_Width = oldxmax - m_XMin; }
        }
        public float yMin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return m_YMin; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { float oldymax = yMax; m_YMin = value; m_Height = oldymax - m_YMin; }
        }
        public float xMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return m_Width + m_XMin; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { m_Width = value - m_XMin; }
        }
        public float yMax
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return m_Height + m_YMin; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { m_Height = value - m_YMin; }
        }

        public float Width { get => m_Width; set => m_Width = value; }
        public float Height { get => m_Height; set => m_Height = value; }
    }
}
#endif