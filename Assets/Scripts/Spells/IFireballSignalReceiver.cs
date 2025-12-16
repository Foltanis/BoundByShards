using UnityEngine;

public interface IFireballSignalReceiver
{
    void OnEnemySeen(GameObject enemy);
    void OnEnemyLost();
}
