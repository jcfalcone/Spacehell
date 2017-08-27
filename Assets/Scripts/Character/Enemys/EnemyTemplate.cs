﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTemplate : CharacterTemplate 
{
    [SerializeField]
    [Range(0f, 1000f)]
    int unitScore;

    [SerializeField]
    protected ParticleSystem bullet;

    [SerializeField]
    Renderer[] enemyRenders;

    [SerializeField]
    protected Vector3 destinePos;

    [SerializeField]
    GameObject itemEffect;

    protected bool initMove = true;

    protected Vector3 startPos;
    protected float startDistance;
    protected float startTime = 0;

    GameObject item;

	// Use this for initialization
    virtual protected void Start () 
    {
        base.Start();

        this.destinePos.z = transform.position.z;
        Vector3 newPos = this.destinePos;//Camera.main.ViewportToWorldPoint(new Vector3(Screen.width, 0, 0));
        newPos.x += 30;

        transform.position = newPos;
        this.startDistance = Vector3.Distance(newPos, this.destinePos);
    }

    override protected void OnParticleCollision (GameObject obj) 
    {   
        if (obj.transform.parent.CompareTag("Player"))
        {
            if (!this.startFlash)
            {
                this.ApplyDamage(1); 
                ControlMaster.instance.addScore(this.unitScore);
                this.flashCount = this.flashTimes;
            }

            if (!this.startFlash && this.flashCount > 0)
            {
                this.startFlash = true;
                StartCoroutine(flashChar());
            }
        }

    }

    override protected void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            this.Die();
        }
    }

    override protected IEnumerator flashChar()
    {
        while (this.flashCount > 0)
        {
            for (int count = 0; count < this.enemyRenders.Length; count++)
            {
                this.enemyRenders[count].material.color = this.flashColor;
            }
            yield return new WaitForSeconds(0.1f);

            for (int count = 0; count < this.enemyRenders.Length; count++)
            {
                this.enemyRenders[count].material.color = this.originalColor;
            }

            yield return new WaitForSeconds(0.1f);

            this.flashCount--;
        }

        this.startFlash = false;
    }

    public override void Die()
    {
        base.Die();

        EnemyMaster.instance.addDeadEnemy();

        if (this.item != null)
        {
            Instantiate(this.item, transform.position, Quaternion.identity);
        }
    }

    public void SetItem(GameObject item)
    {
        if (item != null)
        {
            this.item = item;
            this.itemEffect.SetActive(true);
        }
    }

    abstract protected void initialMovement();
}
