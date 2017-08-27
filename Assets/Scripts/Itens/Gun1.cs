using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun1 : MonoBehaviour 
{
    [SerializeField]
    Player.playerGun currGun;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("AddPowerUp", currGun);
            Destroy(gameObject);
        }
    }
}
