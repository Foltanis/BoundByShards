using System.Collections.Generic;
using UnityEngine;

public static class FireballSignalBroadcaster
{
    private static readonly List<IFireballSignalReceiver> receivers = new();

    public static void Register(IFireballSignalReceiver r)
    {
        if (!receivers.Contains(r))
            receivers.Add(r);
    }

    public static void Unregister(IFireballSignalReceiver r)
    {
        receivers.Remove(r);
    }

    public static void EnemySeen(GameObject enemy)
    {
        foreach (var r in receivers)
            r.OnEnemySeen(enemy);
    }

    public static void EnemyLost()
    {
        foreach (var r in receivers)
            r.OnEnemyLost();
    }
}
