using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineMov : EnemyTemplate 
{
    [Header("Visual")]
    [SerializeField]
    Transform enemyVisual;

    [Header("Life Time")]
    [SerializeField]
    [Range(0f, 10f)]
    float lifeTime;

    [Header("Movement")]
    [SerializeField]
    Vector2 speed;

    [SerializeField]
    float distStartSideMov = 5;

    [SerializeField]
    Vector3 verticalRotationDown;

    [SerializeField]
    Vector3 verticalRotationUp;

    [SerializeField]
    float minMovY;

    [SerializeField]
    float maxMovY;

    Rigidbody rb;

    float distOrigin = 0;

    GameObject Player;

    Quaternion enemyVisualInitRot;

    bool dirVertical = true;

	// Use this for initialization
	void Start () 
    {
        this.rb = GetComponent<Rigidbody>();

        this.Player = GameObject.FindGameObjectWithTag("Player");

        this.enemyVisualInitRot = this.enemyVisual.localRotation;

        this.dirVertical = (EnemyMaster.instance.CurrWave() % 2 == 0) ? true : false;//(Random.Range(0, 1000) % 2 == 0) ? true : false;

        this.destinePos.y = (!this.dirVertical) ? this.minMovY : this.maxMovY;

        base.Start();

        this.startPos = destinePos;
	}

    void Update()
    {
        if (!initMove)
        {
            if (!this.bullet.isEmitting)
            {
                this.bullet.Play();
            }

            if (this.Player)
            {
                this.bullet.transform.LookAt(this.Player.transform);
            }
        }
    }
	
	// Update is called once per frame
    void FixedUpdate () 
    {
        if (initMove)
        {
            this.initialMovement();
        }
        else
        {
            this.moveEnemey();
        }
	}

    void moveEnemey()
    {
        Vector3 finalVelocity = transform.forward * this.speed.x * Time.fixedDeltaTime;

        if (this.distOrigin > this.distStartSideMov)
        {
            if (!this.enemyMasterDieAdded)
            {
                EnemyMaster.instance.addEnemyWave(this.wave);
                this.enemyMasterDieAdded = true;
            }

            if (this.dirVertical && transform.position.y < minMovY)
            {
                this.enemyVisual.localRotation = Quaternion.Lerp(this.enemyVisual.localRotation, Quaternion.Euler(verticalRotationUp), Time.deltaTime * 3);
                finalVelocity += transform.up * this.speed.y * Time.deltaTime;
            }
            else if (!this.dirVertical && transform.position.y > maxMovY)
            {
                this.enemyVisual.localRotation = Quaternion.Lerp(this.enemyVisual.localRotation, Quaternion.Euler(verticalRotationDown), Time.deltaTime * 3);
                finalVelocity += -transform.up * this.speed.y * Time.deltaTime;
            }
            else
            {

                this.enemyVisual.localRotation = Quaternion.Lerp(this.enemyVisual.localRotation, this.enemyVisualInitRot, Time.deltaTime * 3);
            }
        }

        this.rb.velocity = finalVelocity;

        this.distOrigin = Vector3.Distance(transform.position, this.startPos);

        if (this.distOrigin > 130)
        {

            Destroy(gameObject);
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

    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
