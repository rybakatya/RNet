
using System.Collections.Generic;


namespace RapidNetworkLibrary
{
    public class IDGenerator
    {
        public List<ushort> ids;
        public IDGenerator(ushort size)
        {
            ids = new List<ushort>(size);
            for (ushort i = size; i > 0; i--)
            {
                ids.Add(i);
            }
        }

        public void Reset()
        {
            int size = ids.Count;
            ids.Clear();
            for (ushort i = 0; i < size; i++)
            {
                ids.Add(i);
            }
        }
        public ushort Rent()
        {
            var idx = ids.Count - 1;
            var i = ids[idx];
            ids.RemoveAt(idx);
            return i;
        }


        /// <summary>
        /// This is slow on large pools of ids.
        /// </summary>
        /// <param name="id"></param>
        public void Take(ushort id)
        {
            ids.Remove(id);
        }

        public void Return(ushort id)
        {
            ids.Add(id);
        }
    }
}
