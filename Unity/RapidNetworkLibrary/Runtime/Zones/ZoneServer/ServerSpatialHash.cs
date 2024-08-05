
#if SERVER
using System.Collections.Generic;



namespace RapidNetworkLibrary.Runtime.Zones
{
    internal class ServerSpatialHash
    {
        public Dictionary<Cell, Dictionary<Rect, List<CellServerConnection>>> grid = new Dictionary<Cell, Dictionary<Rect, List<CellServerConnection>>>();

        private readonly int _minX;
        private readonly int _minZ;
        private readonly int _width;
        private readonly int _height;
        private readonly int _cellSize;

        private readonly CellServerManager _manager;

        public ServerSpatialHash(int minX, int minZ, int width, int height, int cellSize, CellServerManager cellManager)
        {
            var halfSize = cellSize / 2;
            _minX = minX;
            _minZ = minZ;
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _manager = cellManager;
            var maxX = minX + width;
            var maxZ = minZ + height;

            int i = 0;
            for (int x = minX; x < maxX; x += cellSize)
            {
                for (int z = minZ; z < maxZ; z += cellSize)
                {
                    i++;
                    

                    var c = Cell.FromFloats(x, z, cellSize);
                    
                    if (grid.ContainsKey(c) == false)
                        grid.Add(c, new Dictionary<Rect, List<CellServerConnection>>());

                    var r = new Rect(x, z, cellSize, cellSize);


                    grid[c].Add(r, new List<CellServerConnection>(5));
                }
            }
        }


        public void AddServer(float x, float z, CellServerConnection data)
        {
            var cell = Cell.FromFloats(x, z, _cellSize);
            if (grid.ContainsKey(cell) == true)
            {
                var r = new Rect(cell.x * _cellSize, cell.z * _cellSize, _cellSize, _cellSize);
                data.rect = r;
                if (grid[cell].ContainsKey(r) == true)
                {

                    grid[cell][r].Add(data);
                }
            }
        }

        public void PollAreaForServers(float xPos, float zPos, int size, ref List<CellServerConnection> l)
        {
            var viewRect = new Rect(xPos - size, zPos - size, size, size);
            var currentCell = Cell.FromFloats(xPos, zPos, _cellSize);
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    var cell = new Cell()
                    {
                        x = currentCell.x + x,
                        z = currentCell.z + y
                    };

                    if (grid.ContainsKey(cell) == true)
                    {
                        foreach (var rvp in grid[cell])
                        {
                            if (viewRect.Overlaps(rvp.Key) == true)
                            {
                                if (rvp.Value.Count > 0)
                                {
                                    var idx = rvp.Value.Count - 1;

                                    l.Add(rvp.Value[rvp.Value.Count - 1]);
                                }
                                else
                                {
                                    var con = _manager.RequestCellServer();
                                    AddServer(x, y, new CellServerConnection()
                                    {
                                        cell = cell,
                                        rect = rvp.Key,
                                        id = con.ID
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

#if ENABLE_IL2CPP || ENABLE_MONO
        public void Draw()
        {

            foreach (var c in grid)
            {

                foreach (var r in c.Value)
                {
                    UnityEditor.Handles.Label(new UnityEngine.Vector3(r.Key.xMin + (_cellSize / 2), 0, r.Key.yMin + (_cellSize / 3)), "Cell: X:" + c.Key.x + "Y:" + c.Key.z);
                    UnityEditor.Handles.Label(new UnityEngine.Vector3(r.Key.xMin + (_cellSize / 2), 0, r.Key.yMin + (_cellSize / 2)), "Depth: " + r.Value.Count);
                    UnityEngine.Gizmos.DrawLine(new UnityEngine.Vector3(r.Key.xMin, 0, r.Key.yMin), new UnityEngine.Vector3(r.Key.xMax, 0, r.Key.yMin));
                    UnityEngine.Gizmos.DrawLine(new UnityEngine.Vector3(r.Key.xMin, 0, r.Key.yMin), new UnityEngine.Vector3(r.Key.xMin, 0, r.Key.yMax));

                    UnityEngine.Gizmos.DrawLine(new UnityEngine.Vector3(r.Key.xMax, 0, r.Key.yMax), new UnityEngine.Vector3(r.Key.xMin, 0, r.Key.yMax));
                    UnityEngine.Gizmos.DrawLine(new UnityEngine.Vector3(r.Key.xMax, 0, r.Key.yMax), new UnityEngine.Vector3(r.Key.xMax, 0, r.Key.yMin));
                }
            }
        }
#endif

    }
}
#endif