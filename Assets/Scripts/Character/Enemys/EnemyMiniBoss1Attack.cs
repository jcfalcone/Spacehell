using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiniBoss1Attack : CharacterTemplate 
{

    [SerializeField]
    Renderer[] enemyRenders;

    [SerializeField]
    int unitScore;

    [SerializeField]
    ParticleSystem chargingParticle;

    [SerializeField]
    ParticleSystem shockWaveParticle;

    [SerializeField]
    ParticleSystem[] smallAttackParticle;

    [SerializeField]
    GameObject gravityEffect;

    [SerializeField]
    GameObject laserBean;

    [SerializeField]
    EnemyMiniBoss1 parentController;

    [SerializeField]
    int id;

    [SerializeField]
    float gravityMoveX = 2;

    bool laserOn = false;

    bool startAttack = true;

    EnemyMiniBoss1.MiniBossState currAttack;

    GameObject player;
    Rigidbody playerRb;
    Player playerController;

    float mass;

    Vector3 originalPos;

    bool gravityDirectionMove = true;

    int gravityMoveTickTotal = 5;
    int gravityMoveTick = 0;

	// Use this for initialization
	void Start () 
    {
        base.Start();
        this.chargingParticle.Play();

        this.player = GameObject.FindGameObjectWithTag("Player");
        this.playerRb = this.player.GetComponent<Rigidbody>();
        this.playerController = this.player.GetComponent<Player>();

        this.originalPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (this.currAttack == EnemyMiniBoss1.MiniBossState.Dead)
        {
            for (int count = 0; count < this.enemyRenders.Length; count++)
            {
                this.enemyRenders[count].material.color = Color.Lerp(this.enemyRenders[count].material.color, Color.black, Time.deltaTime * 4);
            }

            if (startAttack)
            {
                this.gravityEffect.SetActive(true);

                if (this.gravityMoveTick > this.gravityMoveTickTotal)
                {

                    this.gravityDirectionMove = !this.gravityDirectionMove;

                    this.gravityMoveTick = 0;
                }

                this.gravityMoveTick++;
            }

            return;
        }
        else if (this.currAttack == EnemyMiniBoss1.MiniBossState.RotationLaser)
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
            else
            {
                this.laserBean.SetActive(false);
                this.laserOn = false;
            }
        }
        else if (this.currAttack == EnemyMiniBoss1.MiniBossState.SmallAttack)
        {
            if (startAttack)
            {
                if (!this.smallAttackParticle[0].isEmitting)
                {
                    for (int count = 0; count < this.smallAttackParticle.Length; count++)
                    {
                        this.smallAttackParticle[count].Play(true);
                    }
                }
            }
            else
            {
                for (int count = 0; count < this.smallAttackParticle.Length; count++)
                {
                    this.smallAttackParticle[count].Stop(false, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
        else if (this.currAttack == EnemyMiniBoss1.MiniBossState.RotateSmallAttack)
        {
            if (startAttack)
            {
                if (!this.smallAttackParticle[0].isEmitting)
                {
                    for (int count = 0; count < this.smallAttackParticle.Length; count++)
                    {
                        this.smallAttackParticle[count].Play(true);
                    }
                }

                for (int count = 0; count < this.smallAttackParticle.Length; count++)
                {
                    this.smallAttackParticle[count].transform.Rotate(0f, 3f, 0f);
                }
            }
            else
            {
                for (int count = 0; count < this.smallAttackParticle.Length; count++)
                {
                    this.smallAttackParticle[count].Stop(false, ParticleSystemStopBehavior.StopEmitting);
                    this.smallAttackParticle[count].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
        }
        else if (this.currAttack == EnemyMiniBoss1.MiniBossState.Gravity)
        {
            if (startAttack)
            {
                this.gravityEffect.SetActive(true);

                Vector3 newPos = this.originalPos;

                if (this.gravityMoveTick > this.gravityMoveTickTotal)
                {
                    newPos.x += this.gravityMoveX * ((gravityDirectionMove) ? 1 : -1);

                    transform.localPosition = newPos;

                    this.gravityDirectionMove = !this.gravityDirectionMove;

                    this.gravityMoveTick = 0;
                }

                this.gravityMoveTick++;
            }
            else
            {

                transform.localPosition = this.originalPos;
                this.gravityEffect.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (this.currAttack == EnemyMiniBoss1.MiniBossState.Gravity || this.currAttack == EnemyMiniBoss1.MiniBossState.Dead)
        {
            if (startAttack)
            {
                if (this.player && this.playerController)
                {
                    Vector3 direction = transform.position - this.player.transform.position;
                    float distance = direction.magnitude;

                    float forceMagnitude = (this.mass * this.playerRb.mass) / Mathf.Pow(distance, 2);

                    Vector3 force = direction.normalized * forceMagnitude;

                    this.playerController.addVelocity = force;
                }
            }
        }
    }

    override protected void OnParticleCollision (GameObject obj) 
    {   
        if (obj.transform.parent.CompareTag("Player"))
        {
            ControlMaster.instance.addScore(this.unitScore);

            if (!this.startFlash)
            {
                this.parentController.applyDamagePart(this.id, 1);
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
        if (collision.transform.CompareTag("Supper Bullet"))
        {
            ControlMaster.instance.addScore(this.unitScore * 2);

            if (!this.startFlash)
            {
                this.parentController.applyDamagePart(this.id, 2);
                this.flashCount = this.flashTimes;
            }

            if (!this.startFlash && this.flashCount > 0)
            {
                this.startFlash = true;
                StartCoroutine(flashChar());
            }
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


    public void StartAttack(EnemyMiniBoss1.MiniBossState attackType)
    {
        this.startAttack = true;
        this.currAttack = attackType;

        this.mass = parentController.mass;


        for (int count = 0; count < this.smallAttackParticle.Length; count++)
        {
            this.smallAttackParticle[count].Stop(false, ParticleSystemStopBehavior.StopEmitting);
            this.smallAttackParticle[count].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (this.currAttack == EnemyMiniBoss1.MiniBossState.RotationLaser)
        {
            if (!this.chargingParticle.isEmitting)
            {
                this.chargingParticle.Play();
            }
        }
    }


    public void StopAttack()
    {
        this.startAttack = false;
    }

    public override void Die()
    {
        //Destroy(gameObject);
        this.currAttack = EnemyMiniBoss1.MiniBossState.Dead;
        this.startAttack = false;
    }
}
