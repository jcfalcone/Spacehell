﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLine : EnemyTemplate 
{
    Vector3 nextMove;

    [Header("Life Time")]
    [SerializeField]
    [Range(0f, 10f)]
    float lifeTime;

    [SerializeField]
    Vector3 endPos;

    bool leaveLevel = false;

	// Use this for initialization
	override protected void Start () 
    {
        this.destinePos = transform.parent.position;

        this.endPos = destinePos;

        this.endPos.x -= 80;
        this.endPos.z  = transform.position.z;

        base.Start();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (initMove)
        {
            this.initialMovement();
        }
        else if (this.leaveLevel)
        {
            this.leaveMovement();
        }
    }

    override protected void initialMovement()
    {
        if (this.startTime == 0)
        {
            this.startTime = Time.time;
        }

        float distCover = (Time.time - this.startTime) * 3;

        if (this.startDistance == 0)
        {
            this.startDistance = 0.1f;
        }

        float pathJorney = distCover / this.startDistance;

        if (pathJorney > 0.2f)
        {
            this.startTime = 0;
            initMove = false;
            StartCoroutine(endUnit());
            this.bullet.Play();
        }

        transform.position = Vector3.Lerp(transform.position, this.destinePos, (pathJorney  * Time.deltaTime) * 10);
    }

    void leaveMovement()
    {
        if (this.startTime == 0)
        {
            this.startTime = Time.time;
            this.startDistance = Vector3.Distance(transform.position, this.endPos);
        }

        float distCover = (Time.time - this.startTime) * 3f;

        if (this.startDistance == 0)
        {
            this.startDistance = 0.1f;
        }

        float pathJorney = distCover / this.startDistance;

//        Debug.Log(pathJorney);

        if (pathJorney > 0.01f && pathJorney < 0.05f)
        {
            if (!this.enemyMasterDieAdded)
            {
                EnemyMaster.instance.addEnemyWave(this.wave);
                this.enemyMasterDieAdded = true;
            }
        }
        else if (pathJorney > 0.2f)
        {
            this.startTime = 0;
            Destroy(transform.parent.gameObject);
        }

        transform.position = Vector3.Lerp(transform.position, this.endPos, (pathJorney  * Time.deltaTime) * 10);
    }

    void nextMovement()
    {
        if (this.startTime == 0)
        {
            this.startTime = Time.time;
            this.nextMove = this.destinePos;
            this.nextMove.y += Random.Range(-4, 4);
            this.startDistance = Vector3.Distance(transform.position, this.nextMove);
        }

        float distCover = (Time.time - this.startTime) * 1.5f;

        if (this.startDistance == 0)
        {
            this.startDistance = 0.1f;
        }

        float pathJorney = distCover / this.startDistance;

        Debug.Log(pathJorney);

        if (pathJorney > 1f)
        {
            this.startTime = 0;
        }

        transform.position = Vector3.Lerp(transform.position, this.nextMove, pathJorney);
    }

    IEnumerator endUnit()
    {
        yield return new WaitForSeconds(lifeTime);
        this.leaveLevel = true;
        this.bullet.Stop();
    }

    public override void Die()
    {
        base.Die();

        Destroy(transform.parent.gameObject);
    }
}
