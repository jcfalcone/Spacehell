using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour {

    [SerializeField]
    EnemyTemplate[] units;

    public void SetItem(GameObject item)
    {
        for (int count = 0; count < this.units.Length; count++)
        {
            this.units[count].SetItem(item);
        }
    }

    public void SetWave(int wave)
    {
        for (int count = 0; count < this.units.Length; count++)
        {
            this.units[count].SetWave(wave);
        }
    }
}
