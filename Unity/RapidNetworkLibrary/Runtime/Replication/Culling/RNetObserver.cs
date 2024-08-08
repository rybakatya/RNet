#if ENABLE_MONO || ENABLE_IL2CPP
using RapidNetworkLibrary.Memory;
using RapidNetworkLibrary.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RapidNetworkLibrary.Replication.Culling
{
    public struct UpdateWorldObserverMessage : IMessageObject
    {
        public Vector2 position;
    }

    public struct WorldObserver : IEquatable<WorldObserver>
    {
        
        public Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

#if SERVER
        public uint ownerID;

        public uint OwnerID { get { return ownerID; }  set { ownerID = value; } }
      

        public bool Equals(WorldObserver other)
        {
            if(ownerID == other.ownerID)
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int)ownerID;
        }

        public void SetPosition(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
#endif
    }

    public abstract class WorldObserverHandlerBase
    {

#if SERVER
        public abstract void SetPosition(ushort owner,  Vector2 position);
#endif
        public abstract Vector2 GetPosition(ushort owner);
    }


    internal class RNetWorldObserverHandler : WorldObserverHandlerBase
    {
#if SERVER
        private readonly Dictionary<uint, WorldObserver> observers;
#elif CLIENT
#endif

        private readonly WorkerCollection _workers;
        public RNetWorldObserverHandler(WorkerCollection workers) 
        {
#if SERVER
            observers = new Dictionary<uint, WorldObserver>(4096);
#elif CLIENT

#endif
            _workers = workers;
        }

#if SERVER
        public void Create(uint ownerID, Vector2 position)
#elif CLIENT
        public void Create(Vector2 position)
#endif
        {
#if SERVER
            observers.Add(ownerID, new WorldObserver()
            {

                ownerID = ownerID,

                position = position
            });
#elif CLIENT

#endif

#if SERVER
            RNet.SendMessage(ownerID, NetworkMessages.spawnWorldObserver, 133, ENet.PacketFlags.Reliable | ENet.PacketFlags.NoAllocate, new UpdateWorldObserverMessage()
            {
                position = position
            });
#endif
        }

        private WorldObserver CreateObserver(uint ownerID, Vector2 position)
        {
            return new WorldObserver()
            {
                ownerID = ownerID,
                position = position
            };
        }
        public override void SetPosition(ushort ownerID, Vector2 position)
        {
            observers[ownerID] = new WorldObserver()
            {
                ownerID = ownerID,
                position = position
            };
        }

        public override Vector2 GetPosition(ushort owner)
        {
            return new Vector2(observers[owner].position.x, observers[owner].position.y);
        }
    }

}
#endif