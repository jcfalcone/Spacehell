using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircle : EnemyTemplate 
{

    bool leaveLevel = false;

	// Use this for initialization
	override protected void Start () 
    {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (initMove)
        {
            this.initialMovement();
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
            this.bullet.Play();
        }

        transform.position = Vector3.Lerp(transform.position, this.destinePos, (pathJorney  * Time.deltaTime) * 10);
    }
}
