﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public sealed class ModManifestPackage : NetworkPackage
{
    public override ushort ID { get { return (ushort)PackageIdentification.ModManifest; } }

    public ModManifestPackage(IEnumerable<Guid> guids)
    {
        AssignData(guids.ToArray());
    }
    public ModManifestPackage(Guid[] guids)
    {
        AssignData(guids);
    }

    private void AssignData(Guid[] guids)
    {
        Data = guids.ToByteArray();
    }
    
    public static List<Guid> Process(byte[] data)
    {
        return Utility.ByteArrayToGUID(data);
    }
}
