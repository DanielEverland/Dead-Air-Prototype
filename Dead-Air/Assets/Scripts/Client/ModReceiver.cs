﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UMS;

/// <summary>
/// Handles communication with server regarding mods
/// </summary>
public static class ModReceiver {

    public static void Initialize()
    {
        Client.EventListener.RegisterCallback((ushort)PackageIdentification.ModManifest, ReceiveModManifest);
        Client.EventListener.RegisterCallback((ushort)PackageIdentification.ModDownload, ReceiveModFile);
        Client.EventListener.RegisterCallback((ushort)PackageIdentification.ObjectIDManifest, ReceiveObjectIDManifest);
    }

    private static void ReceiveModManifest(Peer peer, byte[] data)
    {
        List<System.Guid> guids = data.Deserialize<List<System.Guid>>();
        List<System.Guid> toDownload = new List<System.Guid>();
        HashSet<System.Guid> loadedGuids = new HashSet<System.Guid>(Client.LoadedModFiles.Select(x => x.GUID));

        foreach (System.Guid guid in guids)
        {
            if (!loadedGuids.Contains(guid) && !toDownload.Contains(guid))
            {
                ClientOutput.Line("Missing mod " + guid);

                toDownload.Add(guid);
            }                
        }

        if(toDownload.Count > 0)
        {
            ClientOutput.Line("Sending Download Request");

            peer.SendReliableOrdered(new NetworkPackage(PackageIdentification.ModDownloadRequest, toDownload));
        }
        else
        {
            ClientOutput.Line("All mods matching server");

            peer.SendReliableOrdered(new NetworkPackage(PackageIdentification.RequestObjectIDManifest));
        }
    }
    private static void ReceiveModFile(Peer peer, byte[] data)
    {
        ModFile file = data.Deserialize<ModFile>();

        ClientOutput.Line("Received " + file);

        Client.AddModFile(file);
    }
    private static void ReceiveObjectIDManifest(Peer peer, byte[] data)
    {
        Dictionary<string, ushort> ids = ByteConverter.Deserialize<Dictionary<string, ushort>>(data);
                
        ObjectReferenceManifest.InitializeAsClient(Client.LoadedModFiles, ids);
    }
}
