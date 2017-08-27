using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
    [SerializeField]
    [Range(0, 10)]
    int amount;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("AddLife", this.amount);
            Destroy(gameObject);
        }
    }
}
