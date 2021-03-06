﻿using ProtoBuf;
using Serialization;
using UnityEngine;

namespace Networking.Packages
{
    /// <summary>
    /// Commands an object to be instantiated across the network
    /// </summary>
    [ProtoContract]
    public class InstantiatePackage : NetworkPackage
    {
        public InstantiatePackage() { }
        public InstantiatePackage(Object obj, Vector3 position, Quaternion rotation, ulong networkID)
        {
            _objectID = ObjectReferenceManifest.GetNetworkID(obj);
            _position = position;
            _rotation = rotation;
            _networkID = networkID;

            Data = ByteConverter.Serialize(this);
        }

        public override ushort ID { get { return (ushort)PackageIdentification.Instantiate; } }

        public ulong NetworkID { get { return _networkID; } }
        public ushort ObjectID { get { return _objectID; } }
        public Vector3 Position { get { return _position; } }
        public Quaternion Rotation { get { return _rotation; } }

        /// <summary>
        /// The network ID of the object we wish to instantiate
        /// </summary>
        [ProtoMember(1)]
        private ushort _objectID;

        /// <summary>
        /// The position of the object
        /// </summary>
        [ProtoMember(2)]
        private Vector3 _position;

        /// <summary>
        /// The rotation of the object
        /// </summary>
        [ProtoMember(3)]
        private Quaternion _rotation;

        /// <summary>
        /// ID we use to sync the object across the network
        /// </summary>
        [ProtoMember(4)]
        private ulong _networkID;
    }
}