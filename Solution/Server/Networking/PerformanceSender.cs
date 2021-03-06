﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Components;

namespace Networking
{
    public static class PerformanceSender
    {
        private const float SEND_INTERVAL = 0.5f;

        private static float _timeSinceLastUpdate;

        public static void Update()
        {
            _timeSinceLastUpdate += Time.deltaTime;

            if(_timeSinceLastUpdate > SEND_INTERVAL)
            {
                Send();
            }
        }
        private static void Send()
        {
            foreach (Peer peer in Network.Peers)
            {
                ServerPerformance performance = new ServerPerformance(
                PerformanceCapture.FrameRate,
                (float)peer.Statistics.PacketLossPercent / 100);

                peer.SendReliableUnordered(new NetworkPackage(PackageIdentification.ServerPerformance, performance));
            }
        }
    }
}
