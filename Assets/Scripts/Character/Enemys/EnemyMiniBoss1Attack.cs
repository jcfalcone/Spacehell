using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiniBoss1Attack : MonoBehaviour {
    [SerializeField]
    ParticleSystem chargingParticle;

    [SerializeField]
    ParticleSystem shockWaveParticle;

    [SerializeField]
    GameObject laserBean;

    bool laserOn = false;

    bool startAttack = true;

	// Use this for initialization
	void Start () 
    {
        this.chargingParticle.Play();	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (startAttack)
        {
            if (!this.chargingParticle.isEmitting)
            {
                if (!this.laserOn)
                {
                    this.shockWaveParticle.Play();
                    this.laserBean.SetActive(true);

                    this.laserOn = true;
                }
            }
        }
	}

    void StartAttack()
    {
        this.startAttack = true;
    }
}
