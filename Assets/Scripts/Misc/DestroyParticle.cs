using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour 
{
    private ParticleSystem particle;

	// Use this for initialization
	void Start () 
    {
        this.particle = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!this.particle.IsAlive())
        {
            Destroy(gameObject);
        }
	}
}
