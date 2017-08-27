using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyWaveRule 
{
    public int          afterWaveRule;
    public int          waveCounts;
    public int          priority;
    public float        delay;
    public Vector3      distance;
    public GameObject[] prefabs;
    public int[]        amount;
    public int[]        totalEnemys;
    public Vector3[]    position;
}
