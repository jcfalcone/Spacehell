using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyBossRule 
{
    public enum bossStatus
    {
        None,
        StartBoss,
        EndBoss
    }

    public int alertAtWave = -1;
    public GameObject bossGameObject;
    public bool keepWave = false;
    public bool stopAfterSignal = true;
    public bool endAfterBoss = false;

    public Vector3 startPos = Vector3.zero;
}
