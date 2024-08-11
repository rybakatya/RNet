#if ENABLE_MONO || ENABLE_IL2CPP

using RapidNetworkLibrary.Buffers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace RapidNetworkLibrary.Replication.Culling
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Cell : IEquatable<Cell>
    {
        internal sbyte _x;
        internal sbyte _z;
        internal Rect rect;

            
        public bool Equals(Cell other)
        {
           if(_x ==  other._x && _z == other._z)
                return true;
           return false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return UniqueNumber(_x, _z);
        }
        private int UniqueNumber(int x, int y)
        {
            // Shift the range from [-20, 20] to [0, 40]
            int xShifted = x + 20;
            int yShifted = y + 20;

            // Map the shifted coordinates to a unique number
            int uniqueNum = xShifted * 41 + yShifted;

            return uniqueNum;
        }

        public override string ToString()
        {
            return "Cell { x: " + _x + ", y:" + _z + "}";
        }
    }



    public interface ISpatialItem
    {
        public Vector3 position { get; set; }
        public float size { get; set; }
    }

    public class RNetSpatialHash<T> where T : ISpatialItem
    {
        private Dictionary<Cell, List<T>> _cells;
        private readonly int _cellSize;
        private ArrayPool<Cell> _cellPool;

        public void Draw()
        {
            foreach(var cell in _cells)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(new Vector3(cell.Key.rect.xMin, 0, cell.Key.rect.yMin), new Vector3(cell.Key.rect.xMax, 0, cell.Key.rect.yMin));
                Gizmos.DrawLine(new Vector3(cell.Key.rect.xMin, 0, cell.Key.rect.yMin), new Vector3(cell.Key.rect.xMin, 0, cell.Key.rect.yMax));

                Gizmos.DrawLine(new Vector3(cell.Key.rect.xMax, 0, cell.Key.rect.yMax), new Vector3(cell.Key.rect.xMax,0, cell.Key.rect.yMin));
                Gizmos.DrawLine(new Vector3(cell.Key.rect.xMax, 0, cell.Key.rect.yMax), new Vector3(cell.Key.rect.xMin,0, cell.Key.rect.yMax));
            }
        }
        public RNetSpatialHash(int minX, int minY, int width, int height, int cellSize, int cellBucketSize) 
        {
            if ((width / cellSize) >= 20 || (height / cellSize) >= 20)
                throw new Exception("width/cellsize must be greater than zero or less than 250");

            _cellSize = cellSize;
            _cellPool = ArrayPool<Cell>.Create(9, 32);
            _cells = new Dictionary<Cell, List<T>>();
            for(int x = minX; x < minX + width; x += cellSize)
            {
                for (int y = minY; y < minY + height; y += cellSize)
                {
                    var cell = new Cell()
                    {
                        _x = ((sbyte)(x / cellSize)),
                        _z = ((sbyte)(y / cellSize)),
                        rect = new Rect(x, y, cellSize, cellSize)
                    };
                   
                    _cells.Add(cell, new List<T>());
                   
                }

            }
        }
        public Cell ToCell(Vector3 pos)
        {
            sbyte x = (sbyte)(pos.x / _cellSize);
            sbyte z = (sbyte)(pos.z / _cellSize);
            return new Cell()
            {
                _x = x,
                _z = z,
                rect = new Rect(x, z, _cellSize, _cellSize)
            };
        }
        public Cell Add(Vector3 position, T item)
        {
            var cell = ToCell(position);
            if(_cells.ContainsKey(cell) == true)
            {
                _cells[cell].Add(item);
            }
            return cell;
        }
        

        
        public void GetRelevantCells(Vector3 position, float viewDistance, ref Cell[] cells)
        {
            var viewRect = new Rect(position.x - (viewDistance/2), position.z - (viewDistance /2), viewDistance, viewDistance);
            var cell = ToCell(position);
            int currentCellIndex = 0;
            for (sbyte x = -1; x <= 1; x++)
            {
                for (sbyte y = -1; y <= 1; y++)
                {
                    
                    
                    var thisCell = new Cell()
                    {
                        _x = (sbyte)(cell._x + x),
                        _z = (sbyte)(cell._z + y)
                    };
                    var rect = new Rect(thisCell._x * _cellSize, thisCell._z * _cellSize, _cellSize, _cellSize);
                    if(_cells.ContainsKey(thisCell) == true)
                    {
                        if (viewRect.Overlaps(rect) == true)
                        {
                            cells[currentCellIndex] = thisCell;
                            currentCellIndex++;
                        }

                    }
                }
            }
           
        }



        public void Query(Vector3 position, int viewDistance, ref List<T> items)
        {
            var cell = ToCell(position);
            
            for (sbyte x = -1; x <= 1; x++)
            {
                for (sbyte y = -1; y <= 1; y++)
                {
                    var c = new Cell()
                    {
                        _x = (sbyte)(cell._x + x),
                        _z = (sbyte)(cell._z + y),
                        rect = new Rect(cell._x + x, cell._z + y, _cellSize, _cellSize)
                    };

                    if (_cells.ContainsKey(c) == true)
                    {
                        foreach (var item in _cells[c])
                        {
                            if (new Rect(cell._x * _cellSize, cell._z * _cellSize, viewDistance, viewDistance).Overlaps(
                                new Rect(item.position.x - (item.size/2), item.position.y -(item.size/2), item.size, item.size)) == true)
                            {
                                items.Add(item);
                            }
                        }
                    }
                }
            }
        }

        public void Query(Vector3 position, ref List<T> items)
        {
            var cell = ToCell(position);
            for (sbyte x = -1; x <= 1; x++)
            {
                for (sbyte y = -1; y <= 1; y++)
                {
                    var c = new Cell()
                    {
                        _x = (sbyte)(cell._x + x),
                        _z = (sbyte)(cell._z + y),
                        rect = new Rect(cell._x + x, cell._z + y, _cellSize, _cellSize)
                    };

                    if(_cells.ContainsKey(c) == true)
                    {
                        foreach(var item in _cells[c])
                        {

                            items.Add(item);
                        }
                    }
                }
            }
        }

        public void Remove(Cell cell, T item)
        {
            if (_cells.ContainsKey(cell) == false)
                return;

            _cells[cell].Remove(item);
        }
    }
}
#endif