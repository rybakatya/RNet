using System;
#if ENABLE_MONO || ENABLE_IL2CPP
using UnityEditor;
using UnityEngine;
#endif

namespace RapidNetworkLibrary.Runtime.Zones
{
    public struct Cell : IEquatable<Cell>
    {
        public int x;
        public int z;


        public static Cell FromFloats(float x, float z, int cellSize)
        {
            return new Cell()
            {
                x = (int)x / cellSize,
                z = (int)z / cellSize
            };
        }

        public bool Equals(Cell other)
        {
            if (x == other.x && z == other.z)
                return true;
            return false;
        }

        public override string ToString()
        {
            return "{ X: " + x + ", Z: " + z + "}";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (x * 37) + (z * 37);
        }
        public Cell(int xx, int zz)
        {
            this.x = xx;
            this.z = zz;
        }
    }
}
