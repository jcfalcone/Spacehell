using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterTemplate {

	// Use this for initialization
	void Start () 
    {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    override protected void OnParticleCollision (GameObject obj) 
    {   
        //gameobject  is the emitter
        //obj is the object hitted (colided) with particle
        //but how to acces that exact particle what caused collision (and destroy it)  
        Debug.Log("Hit",obj);


        if (obj.transform.parent.CompareTag("Player"))
        {
            this.ApplyDamage(1); 
        }

    }

    override protected void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision");

        if (collision.transform.CompareTag("Player"))
        {
            this.Die();
        }
    }

    override protected IEnumerator flashChar()
    {
        yield return null;
    }
}
