﻿using Networking;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utility
{
    public const int IPC_PORT = 14768;
    public const int INACTIVE_NETWORK_ID = -1;

    private static System.Random _random = new System.Random();
    
    /// <summary>
    /// Close the standardinput of a process
    /// </summary>
    public static void CloseInput(Process process)
    {
        process.StandardInput.Flush();
        process.StandardInput.Close();
    }
    /// <summary>
    /// Creates a new process that allows you to write input to a batch script
    /// </summary>
    public static Process CreateNewBatch(string workingDirectory)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.WorkingDirectory = workingDirectory;
        startInfo.CreateNoWindow = true;
        startInfo.FileName = "cmd.exe";
        startInfo.RedirectStandardInput = true;
        startInfo.UseShellExecute = false;

        return Process.Start(startInfo);
    }
    /// <summary>
    /// Runs a batch script with the correct working directory
    /// </summary>
    public static Process RunBatchFile(string fullpath)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = fullpath;
        startInfo.WorkingDirectory = Path.GetDirectoryName(fullpath);
        startInfo.CreateNoWindow = true;

        return Process.Start(startInfo);
    }
    public static void InitializeNetworkBehaviours(Object obj, int id)
    {
        if (obj is GameObject)
        {
            GameObject gameObject = obj as GameObject;

            foreach (Component comp in gameObject.GetComponents<Component>())
            {
                if (comp is INetworkedObject)
                {
                    INetworkedObject networkObject = comp as INetworkedObject;

                    networkObject.Initialize(id);
                }
            }
        }
    }
    public static int RandomInt()
    {
        return _random.Next();
    }
    public static short RandomShort()
    {
        byte[] randomBytes = new byte[2];
        _random.NextBytes(randomBytes);

        return (short)(randomBytes[0] + (randomBytes[1] << 8));
    }
    public static ulong RandomULong()
    {
        byte[] buffer = new byte[8];
        _random.NextBytes(buffer);
        return System.BitConverter.ToUInt64(buffer, 0);
    }
    public static bool SceneLoaded(string name)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == name)
                return true;
        }

        return false;
    }
}